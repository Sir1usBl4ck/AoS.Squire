using AoS.Squire.ViewModel;

namespace AoS.Squire.View;

public partial class TacticsPickerPage : ContentPage
{
	public TacticsPickerPage(TacticsPickerViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}