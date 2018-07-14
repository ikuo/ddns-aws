## About

A dynamic DNS client for AWS (Route53) built with .NET Framework and .NET Core.

1. A CLI app [./chkip](./chkip) reports IP change to an AWS Lambda function
2. The AWS Lambda function [./updateDns](./updateDns) updates a Route53 host zone

The use of Lambda is for finer grained authorization on `route53:ChangeResourceRecordSets` action.
