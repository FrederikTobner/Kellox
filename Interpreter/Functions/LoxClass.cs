namespace Interpreter.Functions
{
    /// <summary>
    /// Models a class in the programming language lox
    /// </summary>
    internal class LoxClass : IFunction
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
                if (!methods.ContainsKey("init"))
                {
                    return 0;
                }
                return methods["init"].Arity;
            }
        }

        /// <summary>
        /// The methods of this class
        /// </summary>
        private readonly Dictionary<string, LoxFunction> methods;

        public LoxClass? SuperClass { get; init; }

        /// <summary>
        /// Constructor of the LoxClass class
        /// </summary>
        /// <param name="Name">The Name of the class</param>
        public LoxClass(string Name, Dictionary<string, LoxFunction> Methods, LoxClass? SuperClass)
        {
            this.Name = Name;
            this.methods = Methods;
            this.SuperClass = SuperClass;
        }

        public override string ToString()
        {
            return Name;
        }

        public LoxFunction? FindMethod(string name)
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
            LoxInstance instance = new(this);
            if (methods.ContainsKey("init"))
            {
                LoxFunction initializer = methods["init"];
                initializer.Bind(instance).Call(arguments);
            }
            return instance;
        }
    }
}