## About

A CLI app to report IP change to a Lambda function.

## Building

```
dotnet publish -c Release --framework netcoreapp2.1
```

## Setup

Copy [./config.example.json](./config.example.json) to `./config.json` and edit it.

Then [Configure AWS credentials](https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/net-dg-config.html),
for example put access key and secret key to `%HOME%/.aws/credentials`:

```
[ddns]
aws_access_key_id = {accessKey}
aws_secret_access_key = {secretKey}
```

## Running

```sh
dotnet bin/Release/netcoreapp2.1/chkip.dll
```
