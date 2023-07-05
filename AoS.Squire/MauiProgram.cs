using AoS.Squire.Services;
using AoS.Squire.Store;
using AoS.Squire.View;
using AoS.Squire.ViewModel;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace AoS.Squire;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("icomoon.ttf", "Icons");
            });

        builder.Services.AddSingleton<RemoteDataService>();
        builder.Services.AddSingleton<GameStore>();
        builder.Services.AddSingleton<GameService>();
        builder.Services.AddSingleton<LocalRepository>();
        builder.Services.AddSingleton<ReportService>();

        builder.Services.AddSingleton<MainPageViewModel>();
        builder.Services.AddTransient<GameSetupViewModel>();
        builder.Services.AddTransient<FactionSelectionViewModel>();
        builder.Services.AddTransient<MissionPickerViewModel>();
        builder.Services.AddTransient<GamePageViewModel>();
        builder.Services.AddTransient<TurnPageViewModel>();
        builder.Services.AddTransient<TacticsPickerViewModel>();
        builder.Services.AddTransient<StatsPageViewModel>();
        builder.Services.AddTransient<AllianceSelectionViewModel>();
        builder.Services.AddTransient<EndGameViewModel>();

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddTransient<GameSetupPage>();
        builder.Services.AddTransient<FactionSelectionPage>();
        builder.Services.AddTransient<MissionPickerPage>();
        builder.Services.AddTransient<GamePage>();
        builder.Services.AddTransient<TurnPage>();
        builder.Services.AddTransient<TacticsPickerPage>();
        builder.Services.AddTransient<StatsPage>();
        builder.Services.AddTransient<AllianceSelectionPage>();
        builder.Services.AddTransient<EndGamePage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif
       return builder.Build();
	}
}
