using AoS.Squire.ViewModel;

namespace AoS.Squire.View;

public partial class GhbStatsPage : ContentPage
{
	public GhbStatsPage(GhbStatsPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}