## About

A CLI app to report IP change to a Lambda function.

## Building

```
dotnet restore
dotnet build --configuration Release --runtime win10-x64
```

`win10-x64` part can be replaced with `osx-x64`, `linux-x64` etc.
See also [RID Catalog](https://docs.microsoft.com/dotnet/core/rid-catalog).

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

Run a binary under `bin/Release` directory.

Windows:

```cmd
bin\Release\netcoreapp2.1\win10-x64\chkip.exe
```
or
```cmd
dotnet bin\Release\netcoreapp2.1\chkip.dll
```

macOS:

```sh
./bin/Release/netcoreapp2.1/osx-x64/chkip
```
