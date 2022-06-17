namespace Lox.i18n;

internal static class Messages
{

    #region ErrorMessages

    public const string AtEndOfFileErrorMessage = " at end of file";
    public const string ExpectSemicolonAfterValueErrorMessage = "Expect \';\' after value";
    public const string ExpectSemicolonAfterReturnErrorMessage = "Expect \';\' after return value";
    public const string ExpectClosingBraceAfterBlockErrorMessage = "Expected \'}\' after Block";
    public const string ExpectClassNameErrorMessage = "Expect class name.";
    public const string ExpectSuperClassNameErrorMessage = "Expet superclass name.";
    public const string ExpectLeftBracketBeforeClassBodyErrorMessage = "Expect \'{\' before class body.";
    public const string ExpectRightBracketAfterClassBodyErrorMessage = "Expect \'}\' after class body.";
    public const string InvalidAssignmentTargetErrorMessage = "Invalid assignment target.";
    public const string MaxParametersExcededErrorMessage = "Can't have more than 255 parameters.";
    public const string ExpectRightParenAfterParamsErrorMessage = "Expect \')\' after parameters";
    public const string ExpectParameterNameErrorMessage = "Expect parameter name.";
    public const string ExpectLeftParenAfterWhileErrorMessage = "Expect \'(\' after while.";
    public const string ExpectSemicolonAfterLoopConditionErrorMessage = "Expect \';\' after loop condition";
    public const string ExpectLeftParenAfterIfErrorMessage = "Expect \'(\' after \'if\'.";
    public const string ExpectRightPareAfterrIfErrorMessage = "Expect \')\' after if condition.";
    public const string ExpectPropertyNameAfterDotErrorMessage = "Expect property name after \'.\'.";
    public const string ExpectRightParenAfterForErrorMessage = "Expect \')\' after for.";
    public const string InvalidOperatorErrorMessage = "Invalid Operator";
    public const string ExpectRightParenAfterConditionErrorMessage = "Expect \')\' after condition.";
    public const string ExpectSuperclassMethodNameErrorMessage = "Expect superclass method name";
    public const string ExpectLeftParenAfterForErrorMessage = "Expect \'(\' after for.";
    public const string ExpectVariableNameErrorMessage = "Expect variable name";
    public const string ExpectRightParenAfterArgsErrorMessage = "Expect \')\' after arguments.";
    public const string ExpectDotAfterSuperErrorMessage = "Expect \'.\' after \'super\'";
    public const string ExpectRightParenAfterExpressionErrorMessage = "Expect \')\' after expression.";
    public const string ExpectExpressionErrorMessage = "Expect expression";
    public const string ClassInheritsFromItselfErrorMessage = "A class can't inherit from itself";
    public const string VariableAlreadyDefinedFirstHalfErrorMessage = "A variable with this name (";
    public const string VariableAlreadyDefinedSecondHalfErrorMessage = ") was already defined in this scope";
    public const string UsingSuperOutsideOfClassErrorMessage = "Can't use \'super\' outside of a class";
    public const string ReturnFromTopLevelErrorMessage = "Can't return from top level code";
    public const string UsingSuperWithoutSuperClassErrorMessage = "Can't use \'super\' with no superclass";
    public const string ReturnInInitializerErrorMessage = "Can't return from an initializer";
    public const string ThisOutsideOfClassErrorMessage = "Cant use \'this\' outside of a class";
    public const string ReadsLocalVariableInInitErrorMessage = "Can't read local variable in it's own initializer";
    public const string toMuchArgsErrorMessage = "To much args";
    public const string VariableNotDefinedAssignmentErrorMessage = "Variable not defined yet. Assignment impossible";
    public const string OperatorNotSupportedErrorMessage = "Operator not supported";
    public const string BlockCommentNotClosedErrorMessage = "Blockcomment was never closed, no \"*/\" found";
    public const string UnexpectedCharErrorMessage = "Unexpected character.";
    public const string UnterminatedStringErrorMessage = "Unterminated string.";
    public const string ThereIsNoSuperClassDefinedErrorMessage = "There is no superclass defined";
    public const string OnlyInstancesHaveFieldsErrorMessage = "Only instances have fields";
    public const string SuperClassMustBeAClassErrorMessage = "Superclass must be a class";
    public const string CallOnNonFunctionOrClassErrorMessage = "Can only call functions and classes";

    #endregion ErrorMessages
}
