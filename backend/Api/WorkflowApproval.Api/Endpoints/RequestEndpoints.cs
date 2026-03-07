using WorkflowApproval.Application.DTOs;
using WorkflowApproval.Application.Interfaces;

namespace WorkflowApproval.Api.Endpoints;

public static class RequestEndpoints
{
    public static void MapRequestEndpoints(this WebApplication app)
    {
        app.MapPost("/requests", async (
            CreateRequestDto dto,
            IWorkflowService service) =>
        {
            var id = await service.SubmitRequest(dto);
            return Results.Ok(id);
        });
    }
}