namespace WorkflowApproval.Application.DTOs;

public class ApprovalActionDto
{
    public Guid UserId { get; set; }
    public int StepOrder { get; set; }
    public string? Action { get; set;}
    public string? Comments { get; set; }
    public DateTime ActionDate { get; set; }
}