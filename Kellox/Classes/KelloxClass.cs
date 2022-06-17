using Lox.Functions;
using Lox.i18n;

namespace Lox.Classes;

/// <summary>
/// Models a class in the programming language lox
/// </summary>
internal class KelloxClass : IFunction
{
    /// <summary>
    /// The Name of the class
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Parameters for the Constructor
    /// </summary>
    public int Arity
    {
        get
        {
            if (!methods.ContainsKey(Constants.InitKeyword))
            {
                return 0;
            }
            return methods[Constants.InitKeyword].Arity;
        }
    }

    /// <summary>
    /// The methods of this class
    /// </summary>
    private readonly Dictionary<string, KelloxFunction> methods;

    public KelloxClass? SuperClass { get; init; }

    /// <summary>
    /// Constructor of the LoxClass class
    /// </summary>
    /// <param name="Name">The Name of the class</param>
    public KelloxClass(string Name, Dictionary<string, KelloxFunction> Methods, KelloxClass? SuperClass)
    {
        this.Name = Name;
        methods = Methods;
        this.SuperClass = SuperClass;
    }

    public override string ToString()
    {
        return Name;
    }

    public KelloxFunction? FindMethod(string name)
    {
        if (methods.ContainsKey(name))
        {
            return methods[name];
        }
        if (SuperClass is not null)
        {
            return SuperClass.FindMethod(name);
        }
        return null;
    }

    /// <summary>
    /// Creates a new Instance of the Class
    /// </summary>
    /// <param name="arguments">Arguments passed in the constructor -> ignored at the moment</param>
    public object? Call(List<object?> arguments)
    {
        KelloxInstance instance = new(this);
        if (methods.ContainsKey(Constants.InitKeyword))
        {
            KelloxFunction initializer = methods[Constants.InitKeyword];
            initializer.Bind(instance).Call(arguments);
        }
        return instance;
    }
}
