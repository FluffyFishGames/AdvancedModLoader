using AdvancedModLoader.i18n;
using AdvancedModLoader.ViewModels;
using Avalonia.Controls;

namespace AdvancedModLoader.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var n = (DataContext as MainViewModel);
        n.TestValue++;
    }

    private void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Language.Instance.LoadLanguage("de-DE");
    }
}
