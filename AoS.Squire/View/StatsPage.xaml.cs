using AoS.Squire.ViewModel;

namespace AoS.Squire.View;

public partial class StatsPage : ContentPage
{
	public StatsPage(StatsPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

   
}