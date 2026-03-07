using Microsoft.EntityFrameworkCore;
using WorkflowApproval.Application.Interfaces;
using WorkflowApproval.Domain.Entities;
using WorkflowApproval.Domain.Constants;
using System.Runtime.InteropServices;

namespace WorkflowApproval.Infrastructure.Data;

public class AppDbContext : DbContext, IWorkflowDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Request> Requests { get; set; }
    public DbSet<RequestType> RequestTypes { get; set; }
    public DbSet<WorkflowDefinition> WorkflowDefinitions { get; set; }
    public DbSet<WorkflowStep> WorkflowSteps { get; set; }
    public DbSet<ApprovalAction> ApprovalActions { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }
}

// First migration
// dotnet ef migrations add InitialCreate \
// --project backend/Infrastructure/WorkflowApproval.Infrastructure \
// --startup-project backend/Api/WorkflowApproval.Api

// dotnet ef database update