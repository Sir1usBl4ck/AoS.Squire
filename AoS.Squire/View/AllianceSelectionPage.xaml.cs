using AoS.Squire.ViewModel;

namespace AoS.Squire.View;

public partial class AllianceSelectionPage : ContentPage
{
	public AllianceSelectionPage(AllianceSelectionViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}