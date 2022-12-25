using System.Collections.Generic;
using System.Linq;

namespace ExpertSystem;

enum ExpressionType
{
    And, Or,
}

struct Rule
{
    public Condition Condition;
    public Fact Fact;
}

abstract class Condition
{
    public abstract (bool CanActivate, List<Fact> Requirements) Evaluate(IEnumerable<Fact> facts);
}

class ConditionExpression : Condition
{
    public string Operator;
    public Condition[] Conditions;

    public ExpressionType Type =>
        Operator == "AND" ? ExpressionType.And : ExpressionType.Or;

    public override (bool CanActivate, List<Fact> Requirements) Evaluate(IEnumerable<Fact> facts)
    {
        var results = Conditions.Select(condition => condition.Evaluate(facts));
        if (Type == ExpressionType.And)
        {
            var requirements = results
                .SelectMany(result => result.Requirements)
                .ToList();
            return (results.All(result => result.CanActivate), requirements);
        }
    
        foreach (var result in results)
        {
            if (result.CanActivate) return result;
        }
    
        return (false, new());
    }
}

class ValueCondition : Condition
{
    public string FactName;
    public string? Value;

    public override (bool CanActivate, List<Fact> Requirements) Evaluate(IEnumerable<Fact> facts)
    {
        var foundFact = facts.FirstOrDefault(fact => fact.Name == FactName);

        return foundFact is null
            ? (Value is null, new List<Fact>() )
            : (Value == foundFact.Value, new List<Fact> { foundFact } );
    }
}

class Fact
{
    public string Name;
    public string? Value;
    public Input? Input;

    public bool IsResult => Value == null && Input == null;
}

struct Input
{
    public string Question;
    public string[] Values;
}
