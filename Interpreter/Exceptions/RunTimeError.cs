﻿namespace Interpreter.Exceptions
{
    internal class RunTimeError : ApplicationException
    {
        private readonly Token token;
        public RunTimeError(Token token, string message) : base(message)
        {
            this.token = token;
        }

        internal Token Token => token;
    }
}
