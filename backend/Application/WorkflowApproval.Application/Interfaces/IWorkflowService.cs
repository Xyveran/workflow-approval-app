using WorkflowApproval.Application.DTOs;

namespace WorkflowApproval.Application.Interfaces;

public interface IWorkflowService
{
    Task<Guid> SubmitRequest(CreateRequestDto dto);
    Task ApproveRequest(Guid requestId, Guid userId, string? comments);
    Task RejectRequest(Guid requestId, Guid userId, string? comments);
    Task CommentOnRequest(Guid requestId, Guid userId, string comment);
}