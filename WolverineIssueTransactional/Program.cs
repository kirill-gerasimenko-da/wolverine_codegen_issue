using FluentValidation;
using Lamar;
using Marten;
using Npgsql;
using Oakton;
using Oakton.Resources;
using Weasel.Core;
using Wolverine;
using Wolverine.FluentValidation;
using Wolverine.Marten;
using WolverineIssueTransactional;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseWolverine(config =>
{
    // validation
    config.UseFluentValidation(RegistrationBehavior.ExplicitRegistration);
    config.Services.AddValidatorsFromAssemblies(
        assemblies: new[]
        {
            typeof(Program).Assembly,
        },
        lifetime: ServiceLifetime.Singleton);

    config.Services.AddMarten(store =>
        {
            var connectionString = new NpgsqlConnectionStringBuilder
            {
                Database = "test-wv-issue",
                Port = 5432,
                Host = "localhost",
                Username = "postgres",
                Password = "gv9f0IeQ6B"
            }.ToString();

            store.Connection(connectionString);
            store.AutoCreateSchemaObjects = AutoCreate.All;
            store.Schema.For<TestDocument>();
        })
        .UseLightweightSessions()
        .IntegrateWithWolverine();

    config.Policies.AutoApplyTransactions();

    config.Services.For<ITestDependency>().Use<TestDependency>();
    // config.Services.For<LambdaDependency>().Use(_ => new LambdaDependency());
    config.Services.For<LambdaDependency>().Use<LambdaDependency>();

    config.PublishMessage<FollowUpMessage>().Locally().UseDurableInbox();

}).ApplyOaktonExtensions().UseResourceSetupOnStartup();

var app = builder.Build();

app.MapGet("/", async (IMessageBus bus) => await bus.InvokeAsync<TestHandler.Response>(new TestHandler.Request()));

await app.RunOaktonCommands(args);