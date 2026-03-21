using WorkflowApproval.Application.Interfaces;
using WorkflowApproval.Api.Mappers;

namespace WorkflowApproval.Api.Endpoints;

public static class AnalyticsEndpoints
{
    public static void MapAnalyticsEndpoints(this WebApplication app)
    {
        app.MapGet("/workflow/analytics", async (IWorkflowService workflowService) =>
        {
            var analytics = await workflowService.GetWorkflowAnalytics();
            return TypedResults.Ok(analytics.ToContract());
        })
        .WithTags("Analytics")
        .WithName("GetWorkflowAnalytics");
    }
}