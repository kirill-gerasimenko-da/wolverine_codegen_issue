namespace WolverineIssueTransactional;

using FluentValidation;

public class TestHandler
{
    public record Request;
    public record Response;

    public class Validator : AbstractValidator<Request>
    {}

    public static async Task Before(Request request, ITestDependency dep)
    {
        dep.TestMethod();
    }

    public static async Task<Response> Handle(Request request, ITestDependency dep, CancellationToken token)
    {
        await dep.DoSomething();
        return new Response();
    }
}