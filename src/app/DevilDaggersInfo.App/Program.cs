using DevilDaggersInfo.App;
using DevilDaggersInfo.App.Ui.Config;

AppDomain.CurrentDomain.UnhandledException += (_, args) => Root.Log.Fatal(args.ExceptionObject.ToString());

UpdateLogic.TryDeleteOldExecutableOnAppStart();

Application app = new();

app.Run();
app.Destroy();
