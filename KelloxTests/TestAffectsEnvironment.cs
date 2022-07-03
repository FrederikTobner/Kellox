using Kellox.Interpreter;
using System;
using System.Reflection;
using Xunit.Sdk;

namespace KelloxTests
{
    /// <summary>
    /// Atribut for a tests that affects the environment
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    internal class TestAffectsEnvironment : BeforeAfterTestAttribute
    {
        public override void Before(MethodInfo methodUnderTest)
        {

        }

        public override void After(MethodInfo methodUnderTest)
        {
            KelloxInterpreter.ResetEnvironment();
        }
    }
}
