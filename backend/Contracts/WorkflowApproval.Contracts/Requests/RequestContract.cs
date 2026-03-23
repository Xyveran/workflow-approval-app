using WorkflowApproval.Contracts.Enums;

namespace WorkflowApproval.Contracts.Requests;

public class RequestContract
{
    public Guid Id { get; set; }
    public Guid RequestTypeId { get; set; }
    public required string Title { get; set; }
    public RequestStatus Status { get; set; }
    public int CurrentStep { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}