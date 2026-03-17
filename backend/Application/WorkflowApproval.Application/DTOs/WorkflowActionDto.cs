namespace WorkflowApproval.Application.DTOs;

public class WorkflowActionDto
{
    public required Guid RequestId { get; set; }
    public required Guid UserId { get; set; }
    public string? Comments { get; set; }
}