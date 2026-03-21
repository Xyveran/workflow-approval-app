using System.ComponentModel.DataAnnotations;

namespace WorkflowApproval.Contracts.Requests;

public class CreateRequestContract
{
    [Required] // Validation Attribute from DataAnnotations enforced at API boundary
    public Guid RequestTypeId { get; set; }

    [Required, MaxLength(200)]
    public required string Title { get; set; }

    [MaxLength(2000)]
    public string? Description { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
    public decimal? Amount { get; set; }
    public List<Guid>? AttachmentIds { get; set; }

    // Contract Placeholder until authentication is implemented
    [Required]
    public Guid SubmittedBy { get; set; }
}

// Future structure to use metadata fields so this class will work for all request types
// public Dictionary<string, string> FormData { get; set; }