using System;
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

    public override string ToString() => $"({Condition} ==> {Fact})";
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

    public override string ToString() => $"({String.Join<Condition>(Type == ExpressionType.And ? " AND " : " OR ", Conditions)})";
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

    public override string ToString() => 
        Value is null
            ? $"Не определено '{FactName}'"
            : $"'{FactName} = {Value}'";
}

class Fact
{
    public string Name;
    public string? Value;
    public Input? Input;

    public bool IsResult => Value == null && Input == null;

    public override string ToString()
    {
        string value = $"'{Name}'";
        if (Value != null) value += $" = {Value}";
        if (Input != null) value += $" (Вопрос: {Input})";
        return value;
    }
}

struct Input
{
    public string Question;
    public string[] Values;

    public override string ToString()
    {
        string value = $"'{Question}'";
        if (Values != null) value += $" ({string.Join("/", Values)})";
        return value;
    }
}
