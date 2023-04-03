using System;
using System.Collections.Generic;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;

namespace Task6_1;

public partial class MainWindow : Window
{
    private List<Type> _types;
    private List<string> methods = new List<string>();
    private string _currentType;
    private object? _currentObject = null;
    public MainWindow()
    {
        InitializeComponent();
        _types = new List<Type>();
    }

    private void CreateDropPlaun()
    {
        for (int i = 0; i < _types.Count; i++)
        {
            classesGrid.ColumnDefinitions.Add(new ColumnDefinition(width: GridLength.Star));
            Button btn = new Button();
            btn.Name = _types[i].Name;
            btn.Content = _types[i].Name;
            btn.VerticalAlignment = VerticalAlignment.Center;
            btn.HorizontalAlignment = HorizontalAlignment.Center;
            btn.Click += LoadMethodsFromClass;
            Grid.SetRow(btn, 0);
            Grid.SetColumn(btn, i);
            classesGrid.Children.Add(btn);
        }
    }

    private void LoadMethodsOnGrid()
    {
        MethodsGrid.Children.Clear();
        MethodsGrid.RowDefinitions.Clear();
        for (int i = 0; i < methods.Count; i++)
        {
            MethodsGrid.RowDefinitions.Add(new RowDefinition(height: GridLength.Auto));
            Button btn = new Button();
            btn.Name = methods[i];
            btn.Content = methods[i];
            btn.VerticalAlignment = VerticalAlignment.Center;
            btn.Click += CreateDialogWithParameters;
            Grid.SetRow(btn, i);
            Grid.SetColumn(btn, 0);
            MethodsGrid.Children.Add(btn);
        }
    }

    private async void CreateDialogWithParameters(object? sender, RoutedEventArgs e)
    {
        var inputParams = new InputParametersDialog();
        var parameters = await inputParams.ShowDialog<object[]>(this);
        var method = (Button)sender;
        InvokeMethod(method.Name, parameters);
    }

    private void InvokeMethod(string methodName, object[] parameters)
    {
        BindingFlags flags = BindingFlags.Public;
        bool staticFlag = false;
        if (methodName.StartsWith("static"))
        {
            methodName = methodName.Replace("static", " ");
            flags = flags | BindingFlags.Static;
            staticFlag = true;
        }
        if (methodName.StartsWith("virtual"))
            methodName = methodName.Replace("virtual", " ");

        methodName = methodName.Trim();
        if (methodName.IndexOf("System.Void", StringComparison.Ordinal) != -1)
        {
            var tokens = methodName.Split(new char[]{' ', '(', ')'});
            Type? t = _types.Find((t) => t.Name == _currentType);
            var method = t.GetMethod(tokens[1]);
            var methods = t.GetMethods();
            if (staticFlag)
            {
                method.Invoke(null, parameters);
            }
            else
            {
                if (_currentObject == null || t != _currentObject.GetType())
                    _currentObject= Activator.CreateInstance(t);
                method.Invoke(_currentObject, parameters);
            }
        }
        else
        {
            var tokens = methodName.Split(new char[]{' ', '(', ')'});
            Type? t = _types.Find((t) => t.Name == _currentType);
            var method = t.GetMethod(tokens[1]);
            object? result = "";
            if (staticFlag)
            {
                result = method.Invoke(null, parameters);
            }
            else
            {
                if (_currentObject == null || t != _currentObject.GetType())
                    _currentObject= Activator.CreateInstance(t);
                result = method.Invoke(_currentObject, parameters);
            }
            var msgBox =
                MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("Result",
                    result.ToString());
            msgBox.Show();
        }
    }

    private void LoadMethodsFromClass(object? sender, RoutedEventArgs e)
    {
        Button btn = (Button)sender;
        var type = _types.Find((t) => t.Name == btn.Name);
        _currentType = type.Name;
        methods.Clear();
        foreach (var method in type.GetMethods())
        {
            string res = "";
            if (method.IsStatic)
                res += "static ";
            if (method.IsVirtual)
                res += "virtual ";
            res += method.ReturnType + " ";
            res += method.Name + "(";
            foreach (var parameter in method.GetParameters())
            {
                res += parameter.ParameterType.Name + " " + parameter.Name;
                res += ", ";
            }
            if(res[^1] == ' ')
                res = res.Remove(res.Length - 2);
            res += ")";
            methods.Add(res);
        }
        LoadMethodsOnGrid();
    }

    private void LoadDllBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            var asm = Assembly.LoadFrom(dllPathBox.Text);
            foreach (var type in asm.GetTypes())
            {
                if (type.GetInterfaces().Length != 0 && !type.IsAbstract)
                    _types.Add(type);
            }
            CreateDropPlaun();
        }
        catch (Exception exception)
        {
            var msgBox =
                MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("Error",
                    "Не удалось подключить библиотеку");
            msgBox.Show();
        }
    }
}