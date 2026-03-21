using WorkflowApproval.Domain.Enums;

namespace WorkflowApproval.Application.DTOs;

public class PendingRequestDto
{
    public Guid RequestId { get; set; }
    public required string Title { get; set; }
    public RequestStatus Status { get; set; }
    public int CurrentStep { get; set; }
    public DateTime CreatedAt { get; set; }
}