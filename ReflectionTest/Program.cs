
using System.Reflection;
using System.Security.Cryptography;

var asm = Assembly.LoadFrom("PrinterLib.dll");

Console.WriteLine(asm.FullName);
var types = asm.GetTypes();

foreach (var type in types)
{
    if (type.GetInterfaces().Length != 0 && !type.IsAbstract)
    {
        Console.WriteLine(type.FullName);
        Console.WriteLine("Methods:");
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
            res.Remove(res.Length - 1);
            res += ")";
            Console.WriteLine(res);
        }

        Console.WriteLine();
    }
}
    
