using AoS.Squire.ViewModel;

namespace AoS.Squire.View;

public partial class FactionSelectionPage : ContentPage
{
	public FactionSelectionPage(FactionSelectionViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }
	
}