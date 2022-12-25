using System.Collections.Generic;
using System.Linq;

namespace ExpertSystem;

class InferenceComponent
{
    private MemoryComponent Memory;

    public InferenceComponent(MemoryComponent memory)
    {
        Memory = memory;
    }

    public Fact? GetResult()
    {
        UpdateAgenda();
        while (Memory.Agenda.Any())
        {
            var (ruleToActivate, requirements) = GetOrderedAgenda().First();

            Memory.ActivateRule(ruleToActivate, requirements);

            Memory.AddFact(ruleToActivate.Fact, ruleToActivate);
            if (ruleToActivate.Fact.IsResult) return ruleToActivate.Fact;

            UpdateAgenda();
        }

        return null;
    }

    private Dictionary<Rule, List<Fact>> GetOrderedAgenda() =>
        Memory.Agenda
            .Select((rule, i) => new
            {
                Rule = rule.Key,
                OrderInAgenda = i,
                Requirements = rule.Value,
            })
            .OrderByDescending(ruleWithOrdering => ruleWithOrdering.OrderInAgenda)
            .ToDictionary(
                ruleWithOrdering => ruleWithOrdering.Rule, 
                ruleWithOrdering => ruleWithOrdering.Requirements
            );

    private void UpdateAgenda()
    {
        foreach (var (rule, _) in Memory.Agenda)
        {
            var result = rule.Condition.Evaluate(Memory.Facts.Keys);
            if (!result.CanActivate)
            {
                Memory.RemoveFromAgenda(rule);
            }
        }

        foreach (var rule in Memory.NotActivatedRules)
        {
            var result = rule.Condition.Evaluate(Memory.Facts.Keys);
            if (result.CanActivate)
            {
                Memory.AddToAgenda(rule, result.Requirements);
            }
        }
    }
}
