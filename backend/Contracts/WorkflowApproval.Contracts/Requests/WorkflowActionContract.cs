namespace WorkflowApproval.Contracts.Requests;

public class WorkflowActionContract
{
    public required Guid RequestId { get; set; }
    public required Guid UserId { get; set; }
    public string? Comments { get; set; }
}