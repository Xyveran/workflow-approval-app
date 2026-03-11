namespace WorkflowApproval.Application.DTOs;

public class CreateRequestDto
{
    public Guid RequestTypeId { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public decimal? Amount { get; set; }
    public List<Guid>? AttachmentIds { get; set; }
}

// Future structure to use metadata fields so this class will work for all request types
// public Dictionary<string, string> FormData { get; set; }