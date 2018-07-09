## Files
- serverless.template - an AWS CloudFormation Serverless Application Model template file
- Function.fs - Code file containing the F# function mapped to the single function declared in the template file
- aws-lambda-tools-defaults.json - default argument settings

## Setup

Get dependencies:

```
dotnet restore
```

Make and edit a config file:

```
cp aws-lambda-tools-defaults.json aws-lambda-tools.json
```

Deploy to AWS with CloudFormation with stack name.

```
dotnet lambda deploy-serverless --stack-name update-dns -cfg aws-lambda-tools.json
```
