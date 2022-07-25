# Running apps locally

There are currently four apps:
- DDIAM
- DDAE
- DDCL
- DDRE

The fifth, DDSE, will be added later at some point.

In order to run these apps locally, make sure to:

1. Install the .NET SDK for the version specified in the `TargetFramework` property in the `Directory.Build.props` file.
2. Download [tailwindcss](https://github.com/tailwindlabs/tailwindcss/releases) for your operating system.
3. Place the downloaded executable in the `src` folder and rename it to `tw.exe`. If this doesn't work for your operating system, you can exclude `.exe` from the file name and change the file name in the `Exec` task in the `tailwind build` `Target` in the app's corresponding UI library's .csproj file. The UI libraries for apps can be found in the `src/razor` directory.
4. Navigate to the app's executing project, for example `src/app/DevilDaggersInfo.App.ReplayEditor.Photino`.
5. Execute `dotnet run` from the terminal.
