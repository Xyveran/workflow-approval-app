using WorkflowApproval.Application.DTOs;
using WorkflowApproval.Contracts.Requests;
using WorkflowApproval.Contracts.Responses;

namespace WorkflowApproval.Api.Mappers;

public static class RequestMapper
{
    // Inbound: Request Contract -> Internal DTO

    public static CreateRequestDto ToInternalDto(this CreateRequestContract contract) =>
        new()
        {
            RequestTypeId = contract.RequestTypeId,
            Title = contract.Title,
            Description = contract.Description,
            Amount = contract.Amount,
            AttachmentIds = contract.AttachmentIds,
            SubmittedBy = contract.SubmittedBy // wired from contract currently
        };

    public static CommentDto ToInternalDto(this CommentContract contract) =>
        new()
        {
            RequestId = contract.RequestId,
            UserId = contract.UserId,
            Comments = contract.Comments
        };

    // WorkflowActionContract fields are passed as primitives directly to the service methods
    // (ApproveRequest / RejectRequest), so no DTO mapping needed

    // Outbound: Internal DTO -> Response Contract

    public static RequestTimelineContract ToContract(this RequestTimelineDto dto) =>
        new()
        {
            RequestId = dto.RequestId,
            Status = (Contracts.Enums.RequestStatus)dto.Status,
            CurrentStep = dto.CurrentStep,
            CreatedAt = dto.CreatedAt,
            Timeline = dto.Timeline.Select(a => a.ToContract()).ToList(),
            Steps = dto.Steps.Select(s => s.ToContract()).ToList()

        };

    public static ApprovalActionContract ToContract(this ApprovalActionDto dto) => 
        new()
        {
            UserId = dto.UserId,
            StepOrder = dto.StepOrder,
            Action = dto.Action,
            Comments = dto.Comments,
            ActionDate = dto.ActionDate        
        };

    public static TimelineStepContract ToContract(this TimelineStepDto dto) =>
        new()
        {
            StepOrder = dto.StepOrder,
            RoleId = dto.RoleId,
            Status = (Contracts.Enums.StepStatus)dto.Status,
            CompletedAt = dto.CompletedAt,
            Actions = dto.Actions.Select(a => a.ToContract()).ToList()        
    };
    
    public static TimelineActionContract ToContract(this TimelineActionDto dto) =>
        new()
        {
            UserId = dto.UserId,
            Action = (Contracts.Enums.ApprovalActionType)dto.Action,
            Comments = dto.Comments,
            Timestamp = dto.Timestamp  
        };

    public static PendingRequestContract ToContract(this PendingRequestDto dto) =>
        new()
        {
            RequestId = dto.RequestId,
            Title = dto.Title,
            Status = (Contracts.Enums.StepStatus)dto.Status,
            CurrentStep = dto.CurrentStep,
            CreatedAt = dto.CreatedAt
        };
}