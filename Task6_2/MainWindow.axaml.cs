using System.Collections.Generic;
using Avalonia.Controls;
using Task6_2.Models;

namespace Task6_2;

public partial class MainWindow : Window
{
    private List<Factory> _factories;
    private List<SugarEquipment> _equipments;
    private bool _isFactoryCreate = false, _isLoaderCreate = false, _isEquipmentCreate = false;
    public MainWindow()
    {
        InitializeComponent();
        _factories = new List<Factory>();
        _equipments = new List<SugarEquipment>();
    }
    
    
}