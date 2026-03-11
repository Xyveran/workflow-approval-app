using WorkflowApproval.Domain.Enums;

namespace WorkflowApproval.Application.DTOs;

public class RequestDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required StepStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}