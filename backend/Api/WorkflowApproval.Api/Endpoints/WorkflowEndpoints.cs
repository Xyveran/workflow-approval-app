using WorkflowApproval.Application.DTOs;
using WorkflowApproval.Application.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using WorkflowApproval.Api.Mappers;

namespace WorkflowApproval.Api.Endpoints;

public static class WorkflowEndpoints
{
    public static void MapWorkflowEndpoints(this WebApplication app)
    {
        app.MapGet("/workflow/bottlenecks", async (IWorkflowService workflowService) =>
        {
            var bottlenecks = await workflowService.GetWorkflowBottlenecks();
            return TypedResults.Ok(bottlenecks.ToContract());
        })
        .WithTags("Workflow")
        .WithName("GetWorkflowBottlenecks");
    }
}