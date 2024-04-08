using AdvancedModLoader.ViewModels;
using AdvancedModLoader.Views;

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using SoloX.ExpressionTools.Parser.Impl;
using System;

namespace AdvancedModLoader;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT

        var expToParse = "(int x) => x > 0 ? 1 : -1";
        var expressionParser = new ExpressionParser();
        var lambdaExpression = expressionParser.Parse(expToParse);
        var expression = expressionParser.Parse<Func<int, int>>(expToParse);
        var f = expression.Compile();
        Console.WriteLine(f(4));
        Console.WriteLine(f(-4));
        BindingPlugins.DataValidators.RemoveAt(0);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel()
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = new MainViewModel()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
