using WorkflowApproval.Application.DTOs;
using WorkflowApproval.Application.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WorkflowApproval.Api.Endpoints;

public static class AnalyticsEndpoints
{
    public static void MapAnalyticsEndpoints(this WebApplication app)
    {
        app.MapGet("/workflow/analytics", async (IWorkflowService workflowService) =>
        {
            var analytics = await workflowService.GetWorkflowAnalytics();
            return TypedResults.Ok(analytics);
        });
    }
}