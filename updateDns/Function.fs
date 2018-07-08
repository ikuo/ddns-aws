namespace updateDns

open System.Linq
open Amazon.Lambda.Core
open Amazon.Route53
open Amazon.Route53.Model
open System.Collections.Generic

type Request = { fqdn: string; ip: string; oldIp: string }

[<assembly: LambdaSerializer(typeof<Amazon.Lambda.Serialization.Json.JsonSerializer>)>]
()

module Handler =
  let getEnv name   = System.Environment.GetEnvironmentVariable name
  let region        = getEnv("AWS_REGION") |> Amazon.RegionEndpoint.GetBySystemName
  let hostZoneId    = getEnv("HOST_ZONE_ID")
  let fqdnWhitelist = getEnv("FQDN_WHITELIST").Split [|','|]

  let changeRequest (request: Request) =
    let change =
      new Change(
        Action = new ChangeAction("UPSERT"),
        ResourceRecordSet = new ResourceRecordSet(
          Type = RRType.A,
          TTL = 300L,
          Name = request.fqdn,
          ResourceRecords = new List<ResourceRecord>([new ResourceRecord(request.ip)])
        ))
    new Amazon.Route53.Model.ChangeResourceRecordSetsRequest(
      HostedZoneId = hostZoneId,
      ChangeBatch = new ChangeBatch(Changes = new List<Change>([change])))

  let update (request: Request) (context: ILambdaContext) =
    let log msg = context.Logger.LogLine msg
    log(sprintf "Request: %s ip=%s oldIp=%s" request.fqdn request.ip request.oldIp)
    if fqdnWhitelist.Contains(request.fqdn) then
      let route53: AmazonRoute53Client = new AmazonRoute53Client(region)
      changeRequest(request) |> route53.ChangeResourceRecordSetsAsync
      |> Async.AwaitTask |> Async.RunSynchronously |> ignore
    else
      log(sprintf "%s is not in white list" request.fqdn)
