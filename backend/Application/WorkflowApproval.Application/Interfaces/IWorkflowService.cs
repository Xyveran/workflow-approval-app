using WorkflowApproval.Application.DTOs;

namespace WorkflowApproval.Application.Interfaces;

public interface IWorkflowService
{
    Task<Guid> SubmitRequest(CreateRequestDto dto);
    Task<bool> ApproveRequest(Guid requestId, Guid userId, string? comments);
    Task<bool> RejectRequest(Guid requestId, Guid userId, string? comments);
    Task<bool> CommentOnRequest(Guid requestId, Guid userId, string comment);
    Task<RequestTimelineDto?> GetRequestTimeline(Guid requestId);
    Task<List<RequestTimelineDto>> GetPendingRequests(Guid roleId);
    Task<WorkflowAnalyticsDto> GetWorkflowAnalytics();
    Task<WorkflowBottleneckDto> GetWorkflowBottlenecks();
}