open System
open System.IO
open System.Linq
open Microsoft.Extensions.Configuration
open Hopac
open HttpFs.Client
open DnsClient

let configuration (path: String) =
  (new ConfigurationBuilder()).AddJsonFile(path)
    .Build()

let ipOfMe (): String =
  let body = Request.createUrl Get "http://checkip.amazonaws.com" |> Request.responseAsString |> run
  body.Trim()

let ipByDns (fqdn: String): String =
  let client = new LookupClient()
  client.UseCache <- false
  client.Query(fqdn, QueryType.A).Answers.ARecords().FirstOrDefault().Address.ToString()

[<EntryPoint>]
let main argv =
  let config = configuration(Path.Combine[| Directory.GetCurrentDirectory(); "config.json" |])
  let fqdn: String = config.["FQDN"]
  let ip = ipOfMe()
  let ipDns = ipByDns(fqdn)
  if ip.Equals(ipDns) then
    printfn "Skipping because %s already points to %s" fqdn ip
  else
    printfn "Reporting IP change of %s: %s --to--> %s" fqdn ipDns ip
  0