using WorkflowApproval.Domain.Enums;

namespace WorkflowApproval.Application.DTOs;

public class RequestDto
{
    public Guid Id { get; set; }
    public Guid RequestTypeId { get; set; }
    public required string Title { get; set; }
    public RequestStatus Status { get; set; }
    public int CurrentStep { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}