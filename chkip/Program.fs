open System
open System.IO
open System.Linq
open Microsoft.Extensions.Configuration
open Hopac
open HttpFs.Client
open DnsClient
open Amazon.Lambda
open Amazon.Lambda.Model

let configuration (path: String) = (new ConfigurationBuilder()).AddJsonFile(path).Build()

let ipOfMe (): String =
  let body = Request.createUrl Get "http://checkip.amazonaws.com" |> Request.responseAsString |> run
  body.Trim()

let ipByDns (fqdn: String): String =
  let client = new LookupClient()
  client.UseCache <- false
  client.Query(fqdn, QueryType.A).Answers.ARecords().FirstOrDefault().Address.ToString()

let reportIpChange (config: IConfigurationRoot, fqdn: String, oldIp: String, newIp: String): unit =
  printfn "Reporting IP change of %s: %s --to--> %s"  fqdn oldIp newIp
  let cnf = config.GetSection("Lambda")
  let payload = sprintf """{ "fqdn": "%s", "ip": "%s", "oldIp": "%s" }""" fqdn newIp oldIp
  let lambda = config.GetAWSOptions().CreateServiceClient<IAmazonLambda>()
  let result =
    new InvokeRequest(
      FunctionName = cnf.["FunctionName"],
      InvocationType = InvocationType.Event,
      Payload = payload
    ) |> lambda.InvokeAsync |> Async.AwaitTask |> Async.RunSynchronously
  if not (result.FunctionError |> String.IsNullOrEmpty)
  then printfn "Failed to invoke function due to: %s" result.FunctionError

[<EntryPoint>]
let main argv =
  let config = configuration(Path.Combine[| Directory.GetCurrentDirectory(); "config.json" |])
  let fqdn: String = config.["FQDN"]
  let ip = ipOfMe()
  let ipDns = ipByDns(fqdn)
  if ip.Equals(ipDns) then
    printfn "Skipping because %s already points to %s" fqdn ip
  else
    reportIpChange(config, fqdn, ipDns, ip)
  0