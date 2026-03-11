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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var adminRoleId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var managerRoleId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        var financeRoleId = Guid.Parse("33333333-3333-3333-3333-333333333333");
        var employeeRoleId = Guid.Parse("44444444-4444-4444-4444-444444444444");

        var purchaseRequestTypeId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var leaveRequestTypeId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
        var expenseRequestTypeId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");

        var purchaseWorkflowId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd");

        var step1Id = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee");
        var step2Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff");
        var step3Id = Guid.Parse("99999999-9999-9999-9999-999999999999");

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                Name = "Test Manager",
                RoleId = managerRoleId,
                Department = "Manager"
            }
        );

        modelBuilder.Entity<Role>().HasData(
            new Role { Id = adminRoleId, Name = "Admin" },
            new Role { Id = managerRoleId, Name = "Manager" },
            new Role { Id = financeRoleId, Name = "Finance" },
            new Role { Id = employeeRoleId, Name = "Employee" }
        );

        modelBuilder.Entity<RequestType>().HasData(
            new RequestType
            {
                Id = purchaseRequestTypeId,
                Name = "Purchase Request",
                Description = "Request to purchase goods or services"
            },
            new RequestType
            {
                Id = leaveRequestTypeId,
                Name = "Leave Request",
                Description = "Employee leave or vacation request"
            },
            new RequestType
            {
                Id = expenseRequestTypeId,
                Name = "Expense Reimbursement",
                Description = "Reimbursement of goods or services"
            }
        );

        modelBuilder.Entity<WorkflowDefinition>().HasData(
            new WorkflowDefinition
            {
                Id = purchaseWorkflowId,
                RequestTypeId = purchaseRequestTypeId,
                Name = "Purchase Request Workflow"
            }
        );

        modelBuilder.Entity<WorkflowStep>().HasData(
            new WorkflowStep
            {
                Id = step1Id,
                WorkflowDefinitionId = purchaseWorkflowId,
                StepOrder = 1,
                RoleId = managerRoleId
            },
            new WorkflowStep
            {
                Id = step2Id,
                WorkflowDefinitionId = purchaseWorkflowId,
                StepOrder = 2,
                RoleId = financeRoleId
            },
            new WorkflowStep
            {
                Id = step3Id,
                WorkflowDefinitionId = purchaseWorkflowId,
                StepOrder = 3,
                RoleId = adminRoleId
            }
        );
    }

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