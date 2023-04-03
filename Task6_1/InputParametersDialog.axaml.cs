using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Task6_1;

public partial class InputParametersDialog : Window
{
    private TextBox _paramsBox;
    public InputParametersDialog()
    {
        InitializeComponent();
        _paramsBox = this.Find<TextBox>("paramsBox");
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void sendDataBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        if (_paramsBox.Text == null)
        {
            Close(null);
        }
        else
        {
            string[] results = _paramsBox.Text.Split(";");
            Close(results);
        }
    }
}