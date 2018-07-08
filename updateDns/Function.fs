namespace updateDns

open Amazon.Lambda.Core

type Request = { fqdn: string; ip: string }

[<assembly: LambdaSerializer(typeof<Amazon.Lambda.Serialization.Json.JsonSerializer>)>]
()

module Handler =
  let update (request: Request) (context: ILambdaContext) =
    context.Logger.LogLine(sprintf "Request: %s, %s" request.fqdn request.ip);