using Kellox.Interpreter;
using System;
using System.Reflection;
using Xunit.Sdk;

namespace KelloxTests
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    internal class ResetEnvironmentAfterTest : BeforeAfterTestAttribute
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
