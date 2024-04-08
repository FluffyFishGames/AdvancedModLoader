using AdvancedModLoader.Models;

namespace AdvancedModLoader.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public string Greeting => "And so it begins...";
    private int _TestValue = 1;
    public int TestValue { get => _TestValue; set { SetProperty<int>(ref _TestValue, value, "TestValue"); } }
    public GameCover Cover { get; set; } = new GameCover();
}
