using Kellox.Functions;
using Kellox.i18n;
using Kellox.Tokens;

namespace Kellox.Classes;

/// <summary>
/// Models a class in the programming language lox
/// </summary>
internal class KelloxClass : IFunction
{
    /// <summary>
    /// The Name of the class
    /// </summary>
    private readonly string name;

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

    /// <summary>
    /// The superclass / parentclass -> null if the class has no parent
    /// </summary>
    private readonly KelloxClass? superClass;

    /// <summary>
    /// Constructor of the LoxClass class
    /// </summary>
    /// <param name="name">The Name of the class</param>
    /// <param name="methods">Parameters for the Constructor</param>
    /// <param name="superClass">The superclass / parentclass</param>
    public KelloxClass(string name, Dictionary<string, KelloxFunction> methods, KelloxClass? superClass)
    {
        this.name = name;
        this.methods = methods;
        this.superClass = superClass;
    }

    public override string ToString() => name;

    /// <summary>
    /// Looks up a method in the dictionary and returns it if it was present, otherwise null is returned
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool TryFindMethod(string name, out KelloxFunction? function)
    {
        if (this.methods.ContainsKey(name))
        {
            function = this.methods[name];
            return true;
        }
        if (this.superClass is not null)
        {
            return this.superClass.TryFindMethod(name, out function);
        }
        function = null;
        return false;
    }

    /// <summary>
    /// Creates a new Instance of the Class
    /// </summary>
    /// <param name="arguments">Arguments passed in the constructor -> ignored at the moment</param>
    public object? Call(List<object?> arguments, Token paren)
    {
        KelloxInstance instance = new(this);
        if (methods.ContainsKey(Constants.InitKeyword))
        {
            KelloxFunction initializer = methods[Constants.InitKeyword];
            initializer.Bind(instance).Call(arguments, paren);
        }
        return instance;
    }
}
