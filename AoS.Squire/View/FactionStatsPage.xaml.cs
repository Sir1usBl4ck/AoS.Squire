using AoS.Squire.ViewModel;

namespace AoS.Squire.View;

public partial class FactionStatsPage : ContentPage
{
	public FactionStatsPage(FactionStatsPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;

    }
}