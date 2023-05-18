namespace WolverineIssueTransactional;

using Marten;
using Wolverine;

public interface ITestDependency
{
    void TestMethod();
    Task DoSomething();
}

public record LambdaDependency;

public class TestDependency : ITestDependency
{
    private readonly IDocumentSession _session;
    private readonly IMessageBus _bus;

    public TestDependency(IDocumentSession session, IMessageBus bus, LambdaDependency d)
    {
        _session = session;
        _bus = bus;
    }

    public void TestMethod()
    {
    }

    public async Task DoSomething()
    {
        _session.Store(new TestDocument());
        await _bus.PublishAsync(new FollowUpMessage());
    }
}