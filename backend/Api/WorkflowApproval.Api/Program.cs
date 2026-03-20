using Microsoft.EntityFrameworkCore;
using WorkflowApproval.Application.Interfaces;
using WorkflowApproval.Application.Services;
using WorkflowApproval.Application.Workflow;
using WorkflowApproval.Infrastructure.Data;
using WorkflowApproval.Api.Endpoints;


var builder = WebApplication.CreateBuilder(args);

// Infrastructure
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IWorkflowDbContext>(provider => provider.GetRequiredService<AppDbContext>());

// Application Services
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<WorkflowExecutionEngine>();
builder.Services.AddScoped<AnalyticsService>();
builder.Services.AddScoped<IWorkflowService, WorkflowService>();

// API / Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "TodoAPI";
    config.Title = "WorkflowApprovalAPI v1";
    config.Version = "v1";
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
       config.DocumentTitle = "WorkflowApprovalAPI";
       config.Path = "/swagger";
       config.DocumentPath = "/swagger/{documentName}/swagger.json";
       config.DocExpansion = "list"; 
    });
}

app.MapGet("/", () => "Workflow Approval API");
app.UseSwagger();
app.UseSwaggerUI();

// Endpoints
app.MapRequestEndpoints();
app.MapWorkflowEndpoints();
app.MapAnalyticsEndpoints();
app.Run();



;





