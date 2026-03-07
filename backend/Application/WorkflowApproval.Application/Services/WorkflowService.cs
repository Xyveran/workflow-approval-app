using WorkflowApproval.Application.Interfaces;
using WorkflowApproval.Application.DTOs;
using WorkflowApproval.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace WorkflowApproval.Application.Services;


public class WorkflowService : IWorkflowService
{
    private readonly IWorkflowDbContext _dbContext;

    public WorkflowService(IWorkflowDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Guid> SubmitRequest(CreateRequestDto dto)
    {
        var request = new Request
        {
            Id = Guid.NewGuid(),
            RequestTypeId = dto.RequestTypeId,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow,
            CurrentStep = 1,
        };

        _dbContext.Requests.Add(request);
        await _dbContext.SaveChangesAsync();

        return request.Id;
    }
    public async Task ApproveRequest(Guid requestId, Guid userId)
    {
        // approval logic
    }
    public async Task RejectRequest(Guid requestId, Guid userId)
    {
        // rejection logic
    }
}