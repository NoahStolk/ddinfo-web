using DevilDaggersInfo.App;

AppDomain.CurrentDomain.UnhandledException += (_, args) => Root.Log.Fatal(args.ExceptionObject.ToString());

UpdateLogic.TryDeleteOldExecutableOnAppStart();

Application app = new();

app.Run();
app.Destroy();
