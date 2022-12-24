using System.Collections.Generic;

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
}
    
class ValueCondition : Condition
{
    public string FactName;
    public string? Value;
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
