## About

A dynamic DNS client for AWS (Route53) built with .NET Core.

1. A CLI app [./chkip](./chkip) reports IP change to a Lambda function [./updateDns](./updateDns)
2. The Lambda function updates Route53 zone

The use of Lambda is for finer grained authorization on `route53:ChangeResourceRecordSets`.
