using System.Collections.Generic;
using System.Linq;

namespace ExpertSystem;

class MemoryComponent
{
    public readonly Dictionary<Fact, Rule> Facts;
    public readonly Rule[] Rules;
    public readonly Dictionary<Rule, List<Fact>> Agenda = new ();
    public readonly Dictionary<Rule, List<Fact>> ActivatedRules = new ();

    public Rule[] NotActivatedRules => Rules.Where(rule => !ActivatedRules.ContainsKey(rule)).ToArray();

    public MemoryComponent(IEnumerable<Rule> rules)
    {
        Rules = rules.ToArray();
        Facts = new Dictionary<Fact, Rule>();
    }

    public void AddFact(Fact fact, Rule reason) => Facts.Add(fact, reason);

    public void ActivateRule(Rule rule, List<Fact> requirements) 
    {
        ActivatedRules.Add(rule, requirements);
        RemoveFromAgenda(rule);
    }

    public void AddToAgenda(Rule rule, List<Fact> requirements)
    {
        if (Agenda.ContainsKey(rule)) RemoveFromAgenda(rule);
        Agenda.Add(rule, requirements);
    }

    public void RemoveFromAgenda(Rule rule) => Agenda.Remove(rule);
}
