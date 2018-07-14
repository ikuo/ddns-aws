## About

A CLI app to report IP change to a Lambda function.

## Building

Run `dotnet publish` and check output undler `./bin/Release/<framework>/<platform>/publish/chkip[.exe]`.

### Windows

Install [.NET Framework for Developers](https://docs.microsoft.com/ja-jp/dotnet/framework/install/guide-for-developers) 4.7.2, and

```
dotnet publish -c release -r win10-x64 -f net472
```

### macOS

```
dotnet publish -c release -r osx-x64 -f netcoreapp2.1
```

### Linux

```
dotnet publish -c release -r linux-x64 -f netcoreapp2.1
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

`./bin/release/<framework>/<platform>/publish/chkip[.exe]` or,
`dotnet bin/release/<framework>/<platform>/chkip.dll`
