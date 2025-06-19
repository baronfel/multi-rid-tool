<h3 align="center">Toolsay</h3>

<p align="center"> A platform-specific .NET SDK Tool demonstrating the use of .NET 10 preview 6 features
    <br> 
</p>

## 📝 Table of Contents

- [📝 Table of Contents](#-table-of-contents)
- [🧐 About ](#-about-)
- [🏁 Getting Started ](#-getting-started-)
  - [Prerequisites](#prerequisites)
- [🎈 Usage ](#-usage-)
- [🚀 Packaging ](#-packaging-)

## 🧐 About <a name = "about"></a>

.NET Tools are a useful way to distribute and use platform-independent executables. 
In .NET 10 we're adding a new way of packaging and using Tools that leans into platform-specificity. 
This can lead to smaller, faster, and more efficient tools that are tailored to the platform they run on, while still being easy to use and distribute.

This project is a demonstration of how to create a platform-specific .NET SDK Tool using the new features in .NET 10 preview 6. The tool uses [Spectre.Console][spectre] to echo back some input text as ASCII art.

## 🏁 Getting Started <a name = "getting_started"></a>

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See [deployment](#deployment) for notes on how to deploy the project on a live system.

### Prerequisites

* [.NET 10 preview 6 SDK][download-dotnet-10]


## 🎈 Usage <a name="usage"></a>

To use the tool, use the new `dnx` command to run the tool from the command line. This will download and run the latest version of the tool, which will echo back the input text as ASCII art.

```bash
> dnx toolsay "Hello, World!"
Tool package toolsay@1.0.0 will be downloaded from source <source>.
Proceed? [y/n] (y): y
  _   _          _   _                __        __                 _       _   _
 | | | |   ___  | | | |   ___         \ \      / /   ___    _ __  | |   __| | | |
 | |_| |  / _ \ | | | |  / _ \         \ \ /\ / /   / _ \  | '__| | |  / _` | | |
 |  _  | |  __/ | | | | | (_) |  _      \ V  V /   | (_) | | |    | | | (_| | |_|
 |_| |_|  \___| |_| |_|  \___/  ( )      \_/\_/     \___/  |_|    |_|  \__,_| (_)
                                |/
```

## 🚀 Packaging <a name = "deployment"></a>

The framework-dependent version of the tool, which will run on any system that has .NET 10 preview 6 runtimes installed, can be packaged using the `dotnet pack` command on the `toolsay` project. This will create a NuGet package in `./artifacts/package` that can be distributed and installed using the `dnx` command. If you're testing the local build of the package, you can use the `--source` option to specify the local path to the package.

```bash
> dotnet pack toolsay
Restore complete (0.6s)
You are using a preview version of .NET. See: https://aka.ms/dotnet-support-policy
  toolsay net10.0 succeeded (0.2s) → artifacts\publish\toolsay\release\
  toolsay succeeded (3.4s) → artifacts\bin\toolsay\release\toolsay.dll

Build succeeded in 4.5s
> dotnet tool exec --source .\artifacts\package\ toolsay "Hello, World!"
  _   _          _   _                __        __                 _       _   _
 | | | |   ___  | | | |   ___         \ \      / /   ___    _ __  | |   __| | | |
 | |_| |  / _ \ | | | |  / _ \         \ \ /\ / /   / _ \  | '__| | |  / _` | | |
 |  _  | |  __/ | | | | | (_) |  _      \ V  V /   | (_) | | |    | | | (_| | |_|
 |_| |_|  \___| |_| |_|  \___/  ( )      \_/\_/     \___/  |_|    |_|  \__,_| (_)
                                |/
```

This package can be pushed to NuGet.org or another package source for easier use.

[spectre]: https://spectreconsole.net/
[download-dotnet-10]: https://dotnet.microsoft.com/en-us/download/dotnet/10.0