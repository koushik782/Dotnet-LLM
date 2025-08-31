namespace DevAssistant.Api.Services;

public class PromptTemplateService
{
    private const string SystemPrompt = @"You are DevAssistant, a precise .NET code assistant.
- Prefer C#, ASP.NET Core, EF Core, and SQL Server examples.
- Return runnable, minimal code with comments.
- Warn against unsafe or insecure practices.
- Format code properly with syntax highlighting.
- Be concise but thorough in explanations.";

    private readonly Dictionary<string, string> _templates = new()
    {
        ["general"] = SystemPrompt + "\n\nUser query: {0}",
        
        ["error-explain"] = SystemPrompt + @"

**Task**: Explain the following error and provide a solution.

**Instructions**:
1. Identify the root cause of the error
2. Explain what the error means in simple terms
3. Provide a step-by-step solution with code examples
4. Suggest best practices to prevent similar errors

**Error to analyze**:
{0}",

        ["refactor"] = SystemPrompt + @"

**Task**: Refactor the provided code to improve its quality, performance, and maintainability.

**Instructions**:
1. Analyze the current code for issues (performance, readability, maintainability)
2. Suggest specific improvements
3. Provide the refactored code with explanations
4. Highlight the benefits of each change
5. Ensure the refactored code follows .NET best practices

**Code to refactor**:
{0}",

        ["sql-helper"] = SystemPrompt + @"

**Task**: Help with SQL Server database operations and queries.

**Instructions**:
1. Analyze the SQL requirement or issue
2. Provide optimized SQL Server queries
3. Include Entity Framework Core code if applicable
4. Suggest indexing strategies if relevant
5. Ensure queries follow SQL Server best practices
6. Warn about potential performance issues

**SQL requirement**:
{0}",

        ["code-review"] = SystemPrompt + @"

**Task**: Perform a thorough code review of the provided code.

**Instructions**:
1. Check for bugs, security issues, and performance problems
2. Verify adherence to .NET coding standards
3. Suggest improvements for readability and maintainability
4. Identify potential edge cases or error conditions
5. Recommend testing strategies

**Code to review**:
{0}",

        ["unit-test"] = SystemPrompt + @"

**Task**: Generate comprehensive unit tests for the provided code.

**Instructions**:
1. Create unit tests using xUnit and appropriate mocking frameworks
2. Cover happy path, edge cases, and error scenarios
3. Use AAA pattern (Arrange, Act, Assert)
4. Include meaningful test names and documentation
5. Suggest test data builders if complex objects are involved

**Code to test**:
{0}"
    };

    public string GetPrompt(string templateKey, string userInput)
    {
        if (string.IsNullOrWhiteSpace(templateKey) || !_templates.ContainsKey(templateKey))
        {
            templateKey = "general";
        }

        return string.Format(_templates[templateKey], userInput);
    }

    public IEnumerable<PromptTemplate> GetAvailableTemplates()
    {
        return new[]
        {
            new PromptTemplate("general", "General Assistant", "General development questions and guidance"),
            new PromptTemplate("error-explain", "Error Explainer", "Analyze and explain errors with solutions"),
            new PromptTemplate("refactor", "Code Refactoring", "Improve code quality and maintainability"),
            new PromptTemplate("sql-helper", "SQL Helper", "SQL Server queries and database operations"),
            new PromptTemplate("code-review", "Code Review", "Comprehensive code analysis and suggestions"),
            new PromptTemplate("unit-test", "Unit Test Generator", "Generate comprehensive unit tests")
        };
    }
}

public record PromptTemplate(string Key, string Name, string Description); 