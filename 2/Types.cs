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
    public abstract bool Evaluate(IEnumerable<Fact> facts);
}

class ConditionExpression : Condition
{
    public string Operator;
    public Condition[] Conditions;

    public ExpressionType Type =>
        Operator == "AND" ? ExpressionType.And : ExpressionType.Or;

    public override bool Evaluate(IEnumerable<Fact> facts) =>
        Type == ExpressionType.And
            ? Conditions.All(condition => condition.Evaluate(facts))
            : Conditions.Any(condition => condition.Evaluate(facts));
}
    
class ValueCondition : Condition
{
    public string FactName;
    public string? Value;

    public override bool Evaluate(IEnumerable<Fact> facts)
    {
        var foundFact = facts.FirstOrDefault(fact => fact.Name == FactName);

        return foundFact is null
            ? Value is null
            : Value == foundFact.Value;
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
