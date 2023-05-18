namespace WolverineIssueTransactional;

public class FollowUpHandler
{


    public void Handle(FollowUpMessage message, ILogger<FollowUpHandler> logger)
    {
        logger.LogInformation("Got to followup handler");
    }
}