using WorkflowApproval.Contracts.Enums;

namespace WorkflowApproval.Contracts.Responses;

public class PendingRequestContract
{
    public Guid RequestId { get; set; }
    public required string Title { get; set; }
    public RequestStatus Status { get; set; }
    public int CurrentStep { get; set; }
    public DateTime CreatedAt { get; set; }
}