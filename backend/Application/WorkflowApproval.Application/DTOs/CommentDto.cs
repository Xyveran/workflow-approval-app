namespace WorkflowApproval.Application.DTOs;

public class CommentDto
{
    public required Guid RequestId { get; set; }
    public required Guid UserId { get; set; }

    public required string Comments { get; set;}

}