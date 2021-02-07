# Project structure

The nebulon powershell module consists of the following projects:

- **nebSharp** - a .NET Standard library implementing the nebulon ON API
- **nebPowerAutomation** - the powershell module, makes use of nebSharp. Targets the .NET 4.8 Framework.

The .NET library may be used in addition to the PowerShell module, i.e. for a
.NET Core web-application or other Microsoft centric management tools,
i.e. a [Windows Admin Center gateway plugin](https://docs.microsoft.com/en-us/windows-server/manage/windows-admin-center/extend/develop-gateway-plugin).

# Building the project
This section includes instructions to setup a build server using Windows 10. Your
build server needs the .NET Framwork Dev Pack installed in order to build the
libraries and the powershell module.

In the project root directory follow these steps:

## Restore nuget packages

```shell
nuget restore
```

## Build the solution components

```shell
MSBuild.exe NebPowerAutomation.sln -verbosity:quiet
```
