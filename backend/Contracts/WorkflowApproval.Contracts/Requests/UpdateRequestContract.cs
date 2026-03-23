using System.ComponentModel.DataAnnotations;

namespace WorkflowApproval.Contracts.Requests;

public class UpdateRequestContract
{
    [Required]
    public Guid RequestId { get; set; }
    // Placeholder until auth is implemented
    [Required]
    public Guid RequestedBy { get; set; }
    [MaxLength(200)]
    public string? Title { get; set; }
    [MaxLength(2000)]
    public string? Description { get; set; }
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
    public decimal? Amount { get; set; }
    public List<Guid>? AttachmentIds { get; set; }
}