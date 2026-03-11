using WorkflowApproval.Application.DTOs;
using WorkflowApproval.Application.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WorkflowApproval.Api.Endpoints;

public static class RequestEndpoints
{
    public static void MapRequestEndpoints(this WebApplication app)
    {
        // Submit a request
        app.MapPost("/requests", async Task<Results<Ok<object>, BadRequest<object>>>(
            [FromBody] CreateRequestDto dto, [FromServices] IWorkflowService workflowService) =>
        {
            if (dto == null)
                return TypedResults.BadRequest<object>(new { Error = "Request payload is required." });

            var requestId = await workflowService.SubmitRequest(dto);
            return TypedResults.Ok<object>(new { RequestId = requestId });
        })
        .WithTags("Requests")
        .WithName("SubmitRequest");
        
        // Approve a request
        app.MapPost("/requests/approve", async Task<Results<Ok<object>, BadRequest<object>, NotFound<object>>>(
            [FromBody] WorkflowActionDto dto, [FromServices] IWorkflowService workflowService) =>
        {
            if (dto == null)
                return TypedResults.BadRequest<object>(new { Error = "Request payload is required." });

            var success = await workflowService.ApproveRequest(dto.RequestId, dto.UserId, dto.Comments);
            
            if (!success)
                return TypedResults.NotFound<object>(new { Error = "Request not found or already processed." });            
            
            return TypedResults.Ok<object>(new { Message = "Request approved" });
        })
        .WithTags("Requests")
        .WithName("ApproveRequest");

        // Reject a request
        app.MapPost("/requests/reject", async Task<Results<Ok<object>, BadRequest<object>, NotFound<object>>>(
            [FromBody] WorkflowActionDto dto, [FromServices] IWorkflowService workflowService) =>
        {
            if (dto == null)
                return TypedResults.BadRequest<object>(new { Error = "Request payload is required." });

            var success = await workflowService.RejectRequest(dto.RequestId, dto.UserId, dto.Comments);

            if (!success)
                return TypedResults.NotFound<object>(new { Message = "Request not found or already processed." });

            return TypedResults.Ok<object>(new { Message = "Request rejected." });
        })
        .WithTags("Requests")
        .WithName("RejectRequest");
        
        // Add a comment
        app.MapPost("/requests/comment", async Task<Results<Ok<object>, BadRequest<object>, NotFound<object>>>(
            [FromBody] CommentDto dto, [FromServices] IWorkflowService workflowService) =>
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Comment))
                return TypedResults.BadRequest<object>(new { Error = "Comment cannot be empty." });

            var success = await workflowService.CommentOnRequest(dto.RequestId, dto.UserId, dto.Comments);

            if (!success)
                return TypedResults.NotFound<object>(new { Message = "Request not found." });

            return TypedResults.Ok<object>(new { Message = "Comment added." });
        })
        .WithTags("Requests")
        .WithName("CommentOnRequest");
    }
}