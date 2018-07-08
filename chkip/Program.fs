open System
open System.IO
open System.Linq
open Microsoft.Extensions.Configuration
open Hopac
open HttpFs.Client
open DnsClient

let configuration (): IConfigurationRoot =
  let path = Directory.GetCurrentDirectory()
  (new ConfigurationBuilder()).AddJsonFile(Path.Combine[| path; "config.json" |])
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
  let config = configuration()
  let fqdn: String = config.["FQDN"]
  let ip = ipOfMe()
  let ipDns = ipByDns(fqdn)
  printfn "ZoneID = %s" (config.["ZoneID"])
  if ip.Equals(ipDns) then
    printfn "Skipping because %s already points to %s" fqdn ip
  else
    printfn "Updating %s; %s (old) --to--> %s (new)" fqdn ipDns ip
  0
