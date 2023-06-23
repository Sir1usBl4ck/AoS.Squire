using AoS.Squire.ViewModel;

namespace AoS.Squire.View;

public partial class GameSetupPage : ContentPage
{
	public GameSetupPage(GameSetupViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}