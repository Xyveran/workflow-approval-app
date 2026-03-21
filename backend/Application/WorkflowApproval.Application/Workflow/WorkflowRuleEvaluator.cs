using System.Runtime.InteropServices;
using WorkflowApproval.Domain.Entities;

namespace WorkflowApproval.Application.Workflow;
public static class WorkflowRuleEvaluator
{
    public static bool Evaluate(decimal amount, WorkflowRule rule)
    {
        // Fail safely if given malformed rule data
        if (!decimal.TryParse(rule.Value, out var threshold))
            throw new InvalidOperationException(
                $"WorkflowRule '{rule.Id}' has a non-numeric value '{rule.Value}'. "
                + "Check seed data or migrations for this rule."
            );

        return rule.Operator switch
        {
            ">" => amount > threshold,
            "<" => amount < threshold,
            ">=" => amount >= threshold,
            "<=" => amount <= threshold,
            _ => throw new InvalidOperationException(
                $"WorkflowRule '{rule.Id}' has an unsupported operator '{rule.Operator}'."
            )
        };
    }
}