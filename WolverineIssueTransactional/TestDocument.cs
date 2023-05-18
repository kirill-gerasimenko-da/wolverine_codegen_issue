namespace WolverineIssueTransactional;

using Marten.Schema;

public class TestDocument
{
    [Identity] public Guid Id { get; set; }
    public string Name { get; set; }
}