using System.ComponentModel.DataAnnotations;

namespace WorkflowApproval.Contracts.Requests;

public class CommentContract
{
    public required Guid RequestId { get; set; }
    public required Guid UserId { get; set; }
    public required string Comments { get; set;}
}