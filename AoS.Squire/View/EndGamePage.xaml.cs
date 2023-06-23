using AoS.Squire.ViewModel;

namespace AoS.Squire.View;

public partial class EndGamePage : ContentPage
{
	public EndGamePage(EndGameViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }

    protected override bool OnBackButtonPressed()
    {
        if (BindingContext is EndGameViewModel viewModel)
        {
            viewModel.QuitCommand.ExecuteAsync(null);
        }

        return true;
    }
}