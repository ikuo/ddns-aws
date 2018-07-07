open System
open System.IO
open Microsoft.Extensions.Configuration
open Hopac
open HttpFs.Client

let ipAddress (): String =
  let body = Request.createUrl Get "http://checkip.amazonaws.com" |> Request.responseAsString |> run
  body.Trim()

let configuration () =
  let path = Directory.GetCurrentDirectory()
  (new ConfigurationBuilder()).AddJsonFile(Path.Combine[| path; "config.json" |])
    .Build()

[<EntryPoint>]
let main argv =
  let config = configuration()
  printfn "ZoneID = %s" (config.["ZoneID"])
  printfn "ip = %s" (ipAddress())
  0
