using System.Reflection;
using Xunit.Abstractions;

namespace MicroserviceTemplate.Test;

public class GlobalTest(ITestOutputHelper outputHelper)
{
    private readonly ITestOutputHelper _outputHelper = outputHelper;
    
    [Theory]
    [InlineData("MicroserviceTemplate.Api")]
    [InlineData("MicroserviceTemplate.Domain")]
    [InlineData("MicroserviceTemplate.Repository")]
    [InlineData("MicroserviceTemplate.Service")]
    public void Member_Of_Async_Execution_Should_Have_Async_Prefix(string assemblyName)
    {
        var assembly = Assembly.Load(assemblyName);
        var isValid = true;

        foreach (var type in assembly.DefinedTypes)
        {
            var methods = type.GetMethods();

            foreach (var method in methods)
            {
                if (method.ReturnType.BaseType?.Name == "Task" && !method.Name.EndsWith("Async") && method.Name != "Handle")
                {
                    _outputHelper.WriteLine(
                        $"[Task] Name of async method in {method.DeclaringType!.FullName} without 'Async' postfix and names as: {method.Name}");
                    isValid = false;
                }

                if (method.ReturnType.BaseType?.Name.StartsWith("ValueTask") == true && !method.Name.EndsWith("Async"))
                {
                    _outputHelper.WriteLine(
                        $"[ValueTask] Name of async method in {method.DeclaringType!.FullName} without 'Async' postfix and names as: {method.Name}");
                    isValid = false;
                }
            }
        }
        
        Assert.True(isValid);
    }
}
