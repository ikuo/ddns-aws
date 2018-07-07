// Learn more about F# at http://fsharp.org

open System
open Hopac
open HttpFs.Client

let ipAddress (): String =
  let body = Request.createUrl Get "http://checkip.amazonaws.com" |> Request.responseAsString |> run
  body.Trim()

[<EntryPoint>]
let main argv =
  printfn "ip = %s" (ipAddress())
  0
