open System
open System.IO
open System.Linq
open Microsoft.Extensions.Configuration
open Hopac
open HttpFs.Client
open DnsClient
open Amazon.Lambda
open Amazon.Lambda.Model
let inline (|?) (a: 'a option) b = if a.IsSome then a.Value else b

let configuration (pathOpt: string option) =
  let path = pathOpt |? Path.Combine[| Directory.GetCurrentDirectory(); "config.json" |]
  (new ConfigurationBuilder()).AddJsonFile(path).Build()

let ipOfMe (): string =
  let body = Request.createUrl Get "http://checkip.amazonaws.com" |> Request.responseAsString |> run
  body.Trim()

let ipByDns (fqdn: string): string option =
  let client = new LookupClient()
  client.UseCache <- false
  client.Query(fqdn, QueryType.A).Answers.ARecords().FirstOrDefault()
    |> Option.ofObj |> Option.map (fun r -> r.Address.ToString())

let reportIpChange (config: IConfigurationRoot, fqdn: string, newIp: string, oldIp: string): unit =
  printfn "Reporting IP change of %s: %s --to--> %s"  fqdn oldIp newIp
  let cnf = config.GetSection("Lambda")
  let payload = sprintf """{ "fqdn": "%s", "ip": "%s", "oldIp": "%s" }""" fqdn newIp oldIp
  let lambda = config.GetAWSOptions().CreateServiceClient<IAmazonLambda>()
  let result =
    new InvokeRequest(
      FunctionName   = cnf.["FunctionName"],
      InvocationType = InvocationType.Event,
      Payload        = payload
    ) |> lambda.InvokeAsync |> Async.AwaitTask |> Async.RunSynchronously
  if not (result.FunctionError |> String.IsNullOrEmpty)
  then printfn "Failed to invoke function due to: %s" result.FunctionError

[<EntryPoint>]
let main argv =
  let config = configuration(argv |> Array.tryHead)
  let fqdn   = config.["FQDN"]
  let ip     = ipOfMe()
  match ipByDns(fqdn) with
  | Some(ipDns) when ip.Equals(ipDns) -> printfn "%s already points to %s" fqdn ip
  | Some(ipDns) -> reportIpChange(config, fqdn, ip, ipDns)
  | None        -> reportIpChange(config, fqdn, ip, "")
  0