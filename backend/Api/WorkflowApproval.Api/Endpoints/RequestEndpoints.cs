using WorkflowApproval.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using WorkflowApproval.Contracts.Requests;
using WorkflowApproval.Contracts.Responses;
using WorkflowApproval.Api.Mappers;

namespace WorkflowApproval.Api.Endpoints;

public static class RequestEndpoints
{
    public static void MapRequestEndpoints(this WebApplication app)
    {
        // Submit a new request
        app.MapPost("/requests", 
        async Task<Results<Ok<object>, BadRequest<object>>>(
            [FromBody] CreateRequestContract contract, 
            [FromServices] IWorkflowService workflowService) =>
        {
            if (contract == null)
                return TypedResults.BadRequest<object>(new { Error = "Request payload is required." });

            var requestId = await workflowService.SubmitRequest(contract.ToInternalDto());
            return TypedResults.Ok<object>(new { RequestId = requestId });
        })
        .WithTags("Requests")
        .WithName("SubmitRequest");

        // Approve a request
        app.MapPost("/requests/approve", 
        async Task<Results<Ok<object>, BadRequest<object>, NotFound<object>>>(
            [FromBody] WorkflowActionContract contract,
            [FromServices] IWorkflowService workflowService) =>
        {
            if (contract == null)
                return TypedResults.BadRequest<object>(new { Error = "Request payload is required." });

            var success = await workflowService.ApproveRequest(contract.RequestId, contract.UserId, contract.Comments);
            
            if (!success)
                return TypedResults.NotFound<object>(new { Error = "Request not found or already processed." });            
            
            return TypedResults.Ok<object>(new { Message = "Request approved" });
        })
        .WithTags("Requests")
        .WithName("ApproveRequest");

        // Reject a request
        app.MapPost("/requests/reject", 
        async Task<Results<Ok<object>, BadRequest<object>, NotFound<object>>>(
            [FromBody] WorkflowActionContract contract,
            [FromServices] IWorkflowService workflowService) =>
        {
            if (contract == null)
                return TypedResults.BadRequest<object>(new { Error = "Request payload is required." });

            var success = await workflowService.RejectRequest(contract.RequestId, contract.UserId, contract.Comments);

            if (!success)
                return TypedResults.NotFound<object>(new { Message = "Request not found or already processed." });

            return TypedResults.Ok<object>(new { Message = "Request rejected." });
        })
        .WithTags("Requests")
        .WithName("RejectRequest");
        
        // Add a comment to a request
        app.MapPost("/requests/comment", 
        async Task<Results<Ok<object>, BadRequest<object>, NotFound<object>>>(
            [FromBody] CommentContract contract,
            [FromServices] IWorkflowService workflowService) =>
        {
            if (contract == null || string.IsNullOrWhiteSpace(contract.Comments))
                return TypedResults.BadRequest<object>(new { Error = "Comment cannot be empty." });

            var internalDto = contract.ToInternalDto();
            var success = await workflowService.CommentOnRequest(internalDto.RequestId, internalDto.UserId, internalDto.Comments);

            if (!success)
                return TypedResults.NotFound<object>(new { Message = "Request not found." });

            return TypedResults.Ok<object>(new { Message = "Comment added." });
        })
        .WithTags("Requests")
        .WithName("CommentOnRequest");

        // Get a single request's full timeline by ID
        app.MapGet("/requests/{id:guid}", 
        async Task<Results<Ok<RequestTimelineContract>, NotFound<object>>>(
            Guid id, [FromServices] IWorkflowService workflowService) =>
        {
            var timeline = await workflowService.GetRequestTimeline(id);
            if (timeline == null)
                return TypedResults.NotFound<object>(new { Error = "Request not found."});
            
            return TypedResults.Ok(timeline.ToContract());
        })
        .WithTags("Requests")
        .WithName("GetRequestTimeline");

        // Get all pending requests visible to a given role
        app.MapGet("/requests/pending/{roleId:guid}", 
        async (Guid roleId, [FromServices] IWorkflowService workflowService) =>
        {
            var requests = await workflowService.GetPendingRequests(roleId);
            return TypedResults.Ok(requests.Select(r => r.ToContract()).ToList());
        })
        .WithTags("Requests")
        .WithName("GetPendingRequests");;
    }
}