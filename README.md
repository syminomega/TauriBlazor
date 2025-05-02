# Tauri + Blazor

This template should help get you started developing with Tauri in Blazor, C#, CSS and Javascript.

## Recommended IDE Setup

- [VS Code](https://code.visualstudio.com/) + [Tauri](https://marketplace.visualstudio.com/items?itemName=tauri-apps.tauri-vscode) + [Rust Analyzer](https://marketplace.visualstudio.com/items?itemName=rust-lang.rust-analyzer) + [C#](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp) + [Blazor](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.blazorwasm-companion)
- [RustRover](https://www.jetbrains.com/rust/) + [Rider](https://www.jetbrains.com/rider/)

## Getting Started

1. run `dotnet watch` in the `tauri-blazor-demo/src/` directory to start the frontend.
2. run `cargo tauri dev` in the `tauri-blazor-demo/src-tauri/` directory to start the application in development mode.

## How To Use
1. Add nuget package to your project.
```
dotnet add package SyminStudio.TauriApi --version 0.4.3
```
1. Add following contents to your `Program.cs` file.
```csharp
using TauriApi;
using TauriApi.Plugins;
// ...
builder.Services.AddTauriApi();
builder.Services.AddTauriPlugin<TauriOpener>();
// ...
```
2. Add global using to the `_Imports.razor` file.
```razor
@using TauriApi
@using TauriApi.Plugins
```
3. Now you can inject and use api in your components.
```razor
@inject Tauri Tauri
@inject TauriOpener Opener
<YourComponents/>
@code{
    private string? GreetInput { get; set; }
    private string? GreetMsg { get; set; }

    private async Task GreetAsync()
    {
        GreetMsg = await Tauri.Core.Invoke<string>("greet", new { name = GreetInput });
        await Opener.OpenPath("https://demosite")
    }
}
```

## Supported APIs
> The module `@tauri-apps/api/{moduleName}` is mapped to `Tauri.{ModuleName}` property.
> The constructor of the class is mapped to `Tauri.{ModuleName}.Create{ClassName}()` method. 
> The plugins are mapped to `Tauri{PluginName}` class. 

WIP

## Warning

This package is not yet ready for production. It is a work in progress and should not be used in production applications. It is intended for development and testing purposes only. Use at your own risk. 
This package **only** supports Tauri 2.0 since version 0.3.0. 