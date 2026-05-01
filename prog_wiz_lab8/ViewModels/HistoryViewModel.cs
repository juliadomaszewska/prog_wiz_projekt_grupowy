using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;

namespace prog_wiz_lab8.ViewModels;

public partial class HistoryViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _historyText = "Historia jest pusta.";

    public HistoryViewModel()
    {
        if (File.Exists("historia_makao.txt"))
        {
            HistoryText = File.ReadAllText("historia_makao.txt");
        }
    }
}