using CommunityToolkit.Mvvm.ComponentModel;
using MemoryGame.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MemoryGame.ViewModels;

public partial class HistoryWindowViewModel : ViewModelBase
{
    public HistoryWindowViewModel(ObservableCollection<GameHistoryEntry> entries)
    {
        Entries = entries;
    }

    public ObservableCollection<GameHistoryEntry> Entries { get; }
}
