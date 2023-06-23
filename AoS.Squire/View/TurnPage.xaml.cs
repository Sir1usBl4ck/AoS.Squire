using AoS.Squire.ViewModel;

namespace AoS.Squire.View;

public partial class TurnPage : ContentPage
{
	public TurnPage(TurnPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
        
    }
}