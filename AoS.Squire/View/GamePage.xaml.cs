using System.Diagnostics;
using AoS.Squire.ViewModel;

namespace AoS.Squire.View;

public partial class GamePage : ContentPage
{
	public GamePage(GamePageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
        
    }

    protected override bool OnBackButtonPressed()
    {
        if (BindingContext is GamePageViewModel viewModel)
        {
             viewModel.EndGameCommand.ExecuteAsync(null);
        }

        return true;
    }
}