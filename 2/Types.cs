using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpertSystem;

// Конъюнкция и дизъюнкция
enum ExpressionType
{
    And, Or,
}

// Правило состоит из условия, при котором правило активируется (левая часть правила),
// и факта, который добавляется в базу после активации правила (правая часть)
struct Rule
{
    public Condition Condition;
    public Fact Fact;

    public override string ToString() => $"({Condition} ==> {Fact})";
}

// Условие может быть либо простым условием сравнения факта со значением,
// либо конъюнкцией/дизъюнкцией набора других условий
abstract class Condition
{
    // Выполняется ли условие при наличии данного набора фактов
    // В том числе возвращает список конкретных фактов, которых достаточно для выполнения условия
    public abstract (bool CanActivate, List<Fact> Requirements) Evaluate(IEnumerable<Fact> facts);
}

// Конъюнкция или дизъюнкция набора условий
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

    public override string ToString() => $"({String.Join<Condition>(Type == ExpressionType.And ? " И " : " ИЛИ ", Conditions)})";
}

// Проверка, определен или не определен факт с заданным именем, или сравнение факта с заданным значением
class ValueCondition : Condition
{
    public string FactName;
    public string? Value;

    public override (bool CanActivate, List<Fact> Requirements) Evaluate(IEnumerable<Fact> facts)
    {
        var foundFact = facts.FirstOrDefault(fact => fact.Name == FactName);

        // Условие выполняется, если факт с заданным именем не найден и условие - "Факт не определен",
        // либо значение факта равно значению условия
        return foundFact is null
            ? (Value is null, new List<Fact>() )
            : (Value == foundFact.Value, new List<Fact> { foundFact } );
    }

    public override string ToString() => 
        Value is null
            ? $"Не определено '{FactName}'"
            : $"'{FactName} = {Value}'";
}

// Факт может иметь определенное значение, его значение может определяться вопросом к пользователю,
// либо (при отсутствии значения и вопроса) факт является искомым результатом
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

// Вопрос к пользователю с выбором одного из предложенных значений
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
