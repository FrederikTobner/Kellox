namespace Interpreter.Statements
{
    internal class LoxClass
    {
        public string Name { get; init; }

        public LoxClass(string Name)
        {
            this.Name = Name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}