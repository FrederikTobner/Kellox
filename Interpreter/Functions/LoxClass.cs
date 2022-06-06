﻿namespace Interpreter.Functions
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
        public int Arity => 0;

        public Dictionary<string, LoxFunction> Methods { get; init; }

        /// <summary>
        /// Constructor of the LoxClass class
        /// </summary>
        /// <param name="Name">The Name of the class</param>
        public LoxClass(string Name, Dictionary<string, LoxFunction> Methods)
        {
            this.Name = Name;
            this.Methods = Methods;
        }

        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Creates a new Instance of the Class
        /// </summary>
        /// <param name="arguments">Arguments passed in the constructor -> ignored at the moment</param>
        public object? Call(List<object?> arguments) => new LoxInstance(this);

    }
}