# EnvDotNet

[![Build](https://github.com/renatodellosso/EnvDotNet/actions/workflows/build.yml/badge.svg)](https://github.com/renatodellosso/EnvDotNet/actions/workflows/build.yml)
[![Unit Tests](https://github.com/renatodellosso/EnvDotNet/actions/workflows/test.yml/badge.svg)](https://github.com/renatodellosso/EnvDotNet/actions/workflows/test.yml)
[![codecov](https://codecov.io/gh/renatodellosso/EnvDotNet/graph/badge.svg?token=1UAPEM0QKO)](https://codecov.io/gh/renatodellosso/EnvDotNet)
[![DeepSource](https://app.deepsource.com/gh/renatodellosso/EnvDotNet.svg/?label=active+issues&show_trend=true&token=Ed3Ahkw7q3fP1rBtMhDZatXn)](https://app.deepsource.com/gh/renatodellosso/EnvDotNet/)

Env.Net is a simple library to read environment variables from config files.

## Usage
First, structure your env file like so:
```env
key1=value1
key2=value2
```

Then, you can read the environment variables like so:
```csharp
using EnvDotNet;

Env.Init(); // Env.Init() loads the env file and makes it available globally. The method defaults to using .env as the file name, but can take a string parameter to specify the file name.
string key1 = Env.Get("key1");
```

Env.Net has numerous other features, such as:

**Using multiple env files**
```csharp
Env file1 = new Env("file1.env");
Env file2 = new Env("file2.env");

string key1 = file1["key1"];
string key2 = file2["key2"];
```

**Using default values**
```csharp
Env.Init();
string key = Env.Get("key", "default");
```

**Contains Key**
```csharp
Env.Init();
Env local = new Env("local.env");
bool containsKey = Env.ContainsKey("key");
bool containsLocalKey = local.Contains("key");
```