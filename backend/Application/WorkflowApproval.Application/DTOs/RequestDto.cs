namespace WorkflowApproval.Application.DTOs;

public class RequestDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
}