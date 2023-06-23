using AoS.Squire.View;

namespace AoS.Squire;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(GameSetupPage), typeof(GameSetupPage));
		Routing.RegisterRoute(nameof(FactionSelectionPage), typeof(FactionSelectionPage));
		Routing.RegisterRoute(nameof(MissionPickerPage), typeof(MissionPickerPage));
		Routing.RegisterRoute(nameof(GamePage), typeof(GamePage));
        Routing.RegisterRoute(nameof(TurnPage), typeof(TurnPage));
        Routing.RegisterRoute(nameof(TacticsPickerPage), typeof(TacticsPickerPage));
        Routing.RegisterRoute(nameof(AllianceSelectionPage), typeof(AllianceSelectionPage));
        Routing.RegisterRoute(nameof(EndGamePage), typeof(EndGamePage));
    }
}
