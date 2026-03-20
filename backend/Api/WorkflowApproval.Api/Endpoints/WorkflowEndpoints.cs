using WorkflowApproval.Application.DTOs;
using WorkflowApproval.Application.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WorkflowApproval.Api.Endpoints;

public static class WorkflowEndpoints
{
    public static void MapWorkflowEndpoints(this WebApplication app)
    {
        app.MapGet("/workflow/bottlenecks", async (IWorkflowService workflowService) =>
        {
            var data = await workflowService.GetWorkflowBottlenecks();
            return TypedResults.Ok(data);
        });
    }
}