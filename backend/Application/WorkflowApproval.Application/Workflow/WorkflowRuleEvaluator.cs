using WorkflowApproval.Domain.Entities;

namespace WorkflowApproval.Application.Workflow;
public static class WorkflowRuleEvaluator
{
    public static bool Evaluate(decimal amount, WorkflowRule rule)
    {
        return rule.Operator switch
        {
            ">" => amount > decimal.Parse(rule.Value),
            "<" => amount < decimal.Parse(rule.Value),
            ">=" => amount >= decimal.Parse(rule.Value),
            "<=" => amount <= decimal.Parse(rule.Value),
            _ => false
        };
    }
}