using Microsoft.EntityFrameworkCore;
using WorkflowApproval.Application.Interfaces;
using WorkflowApproval.Domain.Entities;
using WorkflowApproval.Domain.Enums;
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
    public DbSet<WorkflowStepDefinition> WorkflowStepDefinitions { get; set; }
    public DbSet<WorkflowStepInstance> WorkflowStepInstances { get; set; }
    public DbSet<WorkflowInstance> WorkflowInstances { get; set; }
    public DbSet<ApprovalAction> ApprovalActions { get; set; }
    public DbSet<WorkflowRule> WorkflowRules { get; set; }

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
        var step3Id = Guid.Parse("99999999-9999-9999-9999-999999999998");

        // Relationships
        modelBuilder.Entity<ApprovalAction>()
            .HasOne(a => a.Request)
            .WithMany(r => r.ApprovalActions)
            .HasForeignKey(a => a.RequestId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ApprovalAction>()
            .HasIndex(a => a.RequestId);
        modelBuilder.Entity<ApprovalAction>()
            .HasIndex(a => a.StepOrder);
        modelBuilder.Entity<ApprovalAction>()
            .HasIndex(a => a.ActionDate);
        modelBuilder.Entity<ApprovalAction>()
            .HasIndex(r => r.CreatedAt);

        modelBuilder.Entity<WorkflowStepDefinition>()
            .HasOne(s => s.WorkflowDefinition)
            .WithMany(w => w.Steps)
            .HasForeignKey(s => s.WorkflowDefinitionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<WorkflowStepInstance>()
            .HasOne<WorkflowInstance>()
            .WithMany(i => i.Steps)
            .HasForeignKey(s => s.WorkflowInstanceId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<WorkflowRule>()
            .HasOne<WorkflowStepDefinition>()
            .WithMany(s => s.Rules)
            .HasForeignKey(s => s.WorkflowStepId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<WorkflowCondition>()
            .HasOne<WorkflowStepDefinition>()
            .WithMany(s => s.Conditions)
            .HasForeignKey(c => c.WorkflowStepId)
            .OnDelete(DeleteBehavior.Cascade);

        // Seed data
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = adminRoleId, Name = "Admin" },
            new Role { Id = managerRoleId, Name = "Manager" },
            new Role { Id = financeRoleId, Name = "Finance" },
            new Role { Id = employeeRoleId, Name = "Employee" }
        );

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                Name = "Test Manager",
                RoleId = managerRoleId,
                Department = "Manager"
            }
        );

        modelBuilder.Entity<RequestType>().HasData(
            new RequestType
            {
                Id          = purchaseRequestTypeId,
                Name        = "Purchase Request",
                Description = "Request to purchase goods or services"
            },
            new RequestType
            {
                Id          = leaveRequestTypeId,
                Name        = "Leave Request",
                Description = "Employee leave or vacation request"
            },
            new RequestType
            {
                Id          = expenseRequestTypeId,
                Name        = "Expense Reimbursement",
                Description = "Reimbursement of goods or services"
            }
        );

        modelBuilder.Entity<WorkflowDefinition>().HasData(
            new WorkflowDefinition
            {
                Id            = purchaseWorkflowId,
                RequestTypeId = purchaseRequestTypeId,
                Name          = "Purchase Request Workflow",
                IsActive      = true,
                Version       = 1
            }   
        );

        modelBuilder.Entity<WorkflowStepDefinition>().HasData(
            new WorkflowStepDefinition
            {
                Id                   = step1Id,
                WorkflowDefinitionId = purchaseWorkflowId,
                Name                 = "Manager Approval",
                StepOrder            = 1,
                RoleId               = managerRoleId,
                StepType             = StepType.Approval
            },
            new WorkflowStepDefinition
            {
                Id                   = step2Id,
                WorkflowDefinitionId = purchaseWorkflowId,
                Name                 = "Finance Approval",
                StepOrder            = 2,
                RoleId               = financeRoleId,
                StepType             = StepType.Approval
            },
            new WorkflowStepDefinition
            {
                Id                   = step3Id,
                WorkflowDefinitionId = purchaseWorkflowId,
                Name                 = "Admin Sign-off",
                StepOrder            = 3,
                RoleId               = adminRoleId,
                StepType             = StepType.Approval
            }
        );
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }
}

// Migrations:
// dotnet ef migrations add InitialCreate \
//   --project backend/Infrastructure/WorkflowApproval.Infrastructure \
//   --startup-project backend/Api/WorkflowApproval.Api
//
// dotnet ef database update \
//   --project backend/Infrastructure/WorkflowApproval.Infrastructure \
//   --startup-project backend/Api/WorkflowApproval.Api