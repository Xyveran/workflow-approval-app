using WorkflowApproval.Application.DTOs;

namespace WorkflowApproval.Application.Interfaces;

public interface IWorkflowService
{
    Task<Guid> SubmitRequest(CreateRequestDto dto);
    Task ApproveRequest(Guid requestId, Guid userId);
    Task RejectRequest(Guid requestId, Guid userId);
}