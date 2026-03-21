using WorkflowApproval.Contracts.Enums;

namespace WorkflowApproval.Contracts.Requests;

public class RequestContract
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required RequestStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}