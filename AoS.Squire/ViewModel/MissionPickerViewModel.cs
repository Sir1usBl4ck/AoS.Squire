﻿using System.Collections.ObjectModel;
using AoS.Squire.Model;
using AoS.Squire.Store;
using AoS.Squire.View;
using CommunityToolkit.Mvvm.Input;

namespace AoS.Squire.ViewModel;

public partial class MissionPickerViewModel : BaseViewModel
{
    private readonly GameStore _gameStore;

    public MissionPickerViewModel(GameStore gameStore)
    {
        _gameStore = gameStore;
        if (Missions.Any())
        {
            Missions.Clear();
        }
        Missions = new ObservableCollection<Mission>();
        foreach (var mission in gameStore.Ghb.Missions)
        {
            Missions.Add(mission);
        }
    }

    [RelayCommand]
    private async Task SelectionChangedAsync(Mission mission)
    {
        if (mission==null)
        {
            return;
        }
        _gameStore.SelectedMission = mission;
        await Shell.Current.GoToAsync(nameof(GameSetupPage));
    }

    public ObservableCollection<Mission> Missions { get; set; } = new();
}