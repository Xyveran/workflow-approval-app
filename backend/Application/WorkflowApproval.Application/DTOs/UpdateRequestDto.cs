namespace WorkflowApproval.Application.DTOs;

public class UpdateRequestDto
{
    public Guid RequestId { get; set; }

    // Must match original submitter. Enforced in the service until auth implemented
    public Guid RequestedBy { get; set; }

    public string? Title { get; set; }
    public string? Description { get; set; }

    // Updating amount may cause workflow rules to be re-evaluated on the next step
    // advance. Current step is not reset.
    public decimal? Amount { get; set; }

    public List<Guid>? AttachmentIds { get; set; }
}