using Microsoft.EntityFrameworkCore;
using WorkflowApproval.Application.Interfaces;
using WorkflowApproval.Application.Services;
using WorkflowApproval.Infrastructure.Data;
using WorkflowApproval.Api.Endpoints;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IWorkflowDbContext>(provider => provider.GetRequiredService<AppDbContext>());

builder.Services.AddScoped<IWorkflowService, WorkflowService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "Workflow Approval API");
app.MapRequestEndpoints();

app.Run();
