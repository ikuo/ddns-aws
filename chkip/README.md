## About

A CLI app to report IP change to a Lambda function.

## Building

```
dotnet build --configuration Release --runtime win10-x64
```

## Setup

Copy config.example.json to config.json and update it.

Then [Configure AWS credentials](https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/net-dg-config.html).
For example, put access key and secret key to `%HOME%/.aws/credentials`:

```
[ddns]
aws_access_key_id = {accessKey}
aws_secret_access_key = {secretKey}
```

## Running

```
bin\Release\netcoreapp2.0\win10-x64\chkip.exe
```

or

```
dotnet bin\Release\netcoreapp2.0\chkip.dll
```

To run periodicaly, use windows task scheduler.
