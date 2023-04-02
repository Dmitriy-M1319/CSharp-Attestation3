using System;
using System.Collections.Generic;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using MessageBox.Avalonia;
using Task6._1.Models;

namespace Task6._1;

public partial class MainWindow : Window
{
    private List<Type> _types;
    private List<string> methods = new List<string>();
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
        for (int i = 0; i < methods.Count; i++)
        {
            MethodsGrid.RowDefinitions.Add(new RowDefinition(height: GridLength.Auto));
            Button btn = new Button();
            btn.Name = methods[i];
            btn.Content = methods[i];
            btn.VerticalAlignment = VerticalAlignment.Center;
            Grid.SetRow(btn, i);
            Grid.SetColumn(btn, 0);
            MethodsGrid.Children.Add(btn);
        }
    }

    private void LoadMethodsFromClass(object? sender, RoutedEventArgs e)
    {
        MethodsGrid.Children.Clear();
        MethodsGrid.RowDefinitions.Clear();
        Button btn = (Button)sender;
        var type = _types.Find((t) => t.Name == btn.Name);
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
            res.Remove(res.Length - 2);
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