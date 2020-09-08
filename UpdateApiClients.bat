CALL :run_nswag ../DevilDaggersSurvivalEditor/DevilDaggersSurvivalEditor/Clients
CALL :run_nswag ../DevilDaggersAssetEditor/DevilDaggersAssetEditor.Wpf/Clients
CALL :run_nswag ../DevilDaggersCustomLeaderboards/DevilDaggersCustomLeaderboards/Clients
CALL :run_nswag ../DiscordBotDdInfo/DiscordBotDdInfo/Clients
PAUSE

:run_nswag
CALL nswag run %~1/DevilDaggersInfoApiClient.nswag /runtime:NetCore31