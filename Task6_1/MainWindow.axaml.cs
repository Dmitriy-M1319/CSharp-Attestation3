using System;
using System.Collections.Generic;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;

namespace Task6_1;

public partial class MainWindow : Window
{
    // Список типов, которые хранятся в библиотеке и являются не абстрактными
    private List<Type> _types;
    
    /// Список всех методов определенного класса с которым работаем
    /// в виде (static) (virtual) возвращаемый_тип название(параметры)
    private List<string> methods = new List<string>();
    
    /// <summary>
    /// Название текущего типа, с которым работаем, в виде строки
    /// </summary>
    private string _currentType;
    
    /// <summary>
    /// Объект типа, с которым работаем в данный момент (создается по мере необходимости)
    /// </summary>
    private object? _currentObject = null;
    public MainWindow()
    {
        InitializeComponent();
        _types = new List<Type>();
    }

    private void CreateDropPlaun()
    {
        // Проходимся по всем типам
        for (int i = 0; i < _types.Count; i++)
        {
            // Создаем в Grid новую колонку, а в ней кнопку, которая называется в коде и на форме названием класса
            classesGrid.ColumnDefinitions.Add(new ColumnDefinition(width: GridLength.Star));
            Button btn = new Button();
            btn.Name = _types[i].Name;
            btn.Content = _types[i].Name;
            btn.VerticalAlignment = VerticalAlignment.Center;
            btn.HorizontalAlignment = HorizontalAlignment.Center;
            // При нажатии на кнопку в другой Grid подгружаются все методы класса
            btn.Click += LoadMethodsFromClass;
            Grid.SetRow(btn, 0);
            Grid.SetColumn(btn, i);
            classesGrid.Children.Add(btn);
        }
    }

    private void LoadMethodsOnGrid()
    {
        // Очищаем Grid от предыдущих кнопок-методов
        MethodsGrid.Children.Clear();
        MethodsGrid.RowDefinitions.Clear();
        // А далее аналогично методу выше
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
        // Здесь создается диалоговое окно для ввода параметров метода (если они нужны)
        var inputParams = new InputParametersDialog();
        var parameters = await inputParams.ShowDialog<object[]>(this);
        var method = (Button)sender;
        // А потом название метода и параметры в виде строк отправляются на выполнение
        InvokeMethod(method.Name, parameters);
    }

    private void InvokeMethod(string methodName, object[] parameters)
    {
        BindingFlags flags = BindingFlags.Public;
        // Флаг для отображения, статический метод или нет
        bool staticFlag = false;
        // Убираем из названия static (если есть)
        if (methodName.StartsWith("static"))
        {
            methodName = methodName.Replace("static", " ");
            flags = flags | BindingFlags.Static;
            staticFlag = true;
        }
        // Убираем из названия virtual (если есть)
        if (methodName.StartsWith("virtual"))
            methodName = methodName.Replace("virtual", " ");

        methodName = methodName.Trim();
        // Если метод ничего не возвращает
        if (methodName.IndexOf("System.Void", StringComparison.Ordinal) != -1)
        {
            // Отсюда нам надо разбить метод по некоторым разделителям, чтобы вытащить имя
            var tokens = methodName.Split(new char[]{' ', '(', ')'});
            
            // через лямбда-выражения ищем в нашем списке типов текущий тип, с которым работаем
            Type? t = _types.Find((t) => t.Name == _currentType);
            
            // Получаем объект метода из этого класса
            var method = t.GetMethod(tokens[1]);
            if (staticFlag)
            {
                // Если метод статический, то просто вызываем его с переданными параметрами, больше ничего не надо
                method.Invoke(null, parameters);
            }
            else
            {
                // Иначе через специальную штуку Activator.CreateInstanse(Type) создаем экземпляр этого типа
                if (_currentObject == null || t != _currentObject.GetType())
                    _currentObject= Activator.CreateInstance(t);
                
                // И передаем в вызов метода
                method.Invoke(_currentObject, parameters);
            }
        }
        else
        {
            // Тут все тоже самое, что и выше, за исключением того, что у нас метод что-то возвращает
            // и это что-то мы должны получить в переменную типа object? (базовый самый класс для всего)
            var tokens = methodName.Split(new char[]{' ', '(', ')'});
            Type? t = _types.Find((t) => t.Name == _currentType);
            var method = t.GetMethod(tokens[1]);
            
            // Вот этот наш результат выполнения
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
            // Потом результат переводится в string и печатается в MessageBox
            var msgBox =
                MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("Result",
                    result.ToString());
            msgBox.Show();
        }
    }

    private void LoadMethodsFromClass(object? sender, RoutedEventArgs e)
    {
        Button btn = (Button)sender;
        // Вот здесь мы получили экземпляр кнопки, которая была нажата, и вытянули из нее имя типа, которая она
        // представляет
        var type = _types.Find((t) => t.Name == btn.Name);
        _currentType = type.Name;
        // Очищаем список от предыдущих методов
        methods.Clear();
        // Потом проходимся по каждому методу и вытаскиваем из него инфу, после чего приводим все в строку
        // формат которой указан в комментарии около списка выше
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
            // Пытаемся подгрузить динамически либу (например, пришло PrinterLib.dll
            var asm = Assembly.LoadFrom(dllPathBox.Text);
            foreach (var type in asm.GetTypes())
            {
                // Проходимся по типам, и если это не интерфейс и не абстрактный класс, то добавляем в наш список
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