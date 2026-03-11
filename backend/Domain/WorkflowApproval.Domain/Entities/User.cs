namespace WorkflowApproval.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public Guid RoleId { get; set; }
    public Role? Role { get; set; }
    public required string Name { get; set; }
    public string? Email { get; set; }
    public string? Department { get; set; }
}