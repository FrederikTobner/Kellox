using Kellox.Interpreter;
using System;
using System.Reflection;
using Xunit.Sdk;

namespace KelloxTests
{
    /// <summary>
    /// Atribut for a tests that affect the environment and could produce an error
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    internal class KelloxTestAttribute : BeforeAfterTestAttribute
    {
        public override void Before(MethodInfo methodUnderTest)
        {

        }

        public override void After(MethodInfo methodUnderTest)
        {
            KelloxInterpreter.RunTimeErrorOccurred = false;
            KelloxInterpreter.ErrorOccurred = false;
            KelloxInterpreter.ResetEnvironment();
        }
    }
}
