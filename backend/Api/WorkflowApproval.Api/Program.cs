using Microsoft.EntityFrameworkCore;
using WorkflowApproval.Application.Interfaces;
using WorkflowApproval.Application.Services;
using WorkflowApproval.Application.Workflow;
using WorkflowApproval.Infrastructure.Data;
using WorkflowApproval.Api.Endpoints;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;


var builder = WebApplication.CreateBuilder(args);

// Infrastructure
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IWorkflowDbContext>(provider => provider.GetRequiredService<AppDbContext>());

// Application Services
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

// Global Error Handler giving JSON errors
app.UseExceptionHandler(err => err.Run(async ctx =>
{
    ctx.Response.StatusCode = 500;
    ctx.Response.ContentType = "application/json";
    var error = ctx.Features.Get<IExceptionHandlerFeature>();
    await ctx.Response.WriteAsJsonAsync(new
    {
        Error = "An unexpected error occurred.",
        Detail = app.Environment.IsDevelopment() ? error?.Error.Message : null
    });
}));

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

// Endpoints
app.MapRequestEndpoints();
app.MapWorkflowEndpoints();
app.MapAnalyticsEndpoints();
app.Run();