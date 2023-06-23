using AoS.Squire.ViewModel;

namespace AoS.Squire.View;

public partial class MissionPickerPage : ContentPage
{
	public MissionPickerPage(MissionPickerViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}