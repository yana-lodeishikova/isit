using System.Collections.Generic;
using System.Linq;

namespace ExpertSystem;

class MemoryComponent
{
    // Список действующих фактов. Каждому факту в соответствие поставлено правило,
    // в результате активации которого получен данный факт.
    public readonly Dictionary<Fact, Rule> Facts;

    // Все правила базы знаний
    public readonly Rule[] Rules;

    // Список правил, условия которых уже выполнены, и которые могут быть активированы в данный момент
    // (вместе со списком фактов, наличие которых обращает условие данного правила в истину)
    public readonly Dictionary<Rule, List<Fact>> Agenda = new ();

    // Активированные правила, правая часть которых уже внесена в список действующих фактов
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
