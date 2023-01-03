using System;
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

    // Алгоритм вывода выбирает одно из правил, готовых к активации (условия которых выполнены),
    // добавляет правую часть этого правила к списку действующих фактов
    // если добавленный факт является результатом - завершает работу.
    public Fact? GetResult()
    {
        UpdateAgenda();
        while (Memory.Agenda.Any())
        {
            var (ruleToActivate, requirements) = GetOrderedAgenda().First();

            Memory.ActivateRule(ruleToActivate, requirements);
            if (ruleToActivate.Fact.Input is Input input) ruleToActivate.Fact.Value = AskQuestion(input.Question, input.Values);

            Memory.AddFact(ruleToActivate.Fact, ruleToActivate);
            if (ruleToActivate.Fact.IsResult) return ruleToActivate.Fact;

            UpdateAgenda();
        }

        return null;
    }

    // Алгоритм разрешения конфликтов
    // Правила отсортированы по порядку выполнения их условия (сначала самые новые)
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

    // Принимает введенное пользователем значение, валидирует
    // и при необходимости задает вопрос повторно.
    public static string AskQuestion(string question, string[] inputValues)
    {
        Console.WriteLine(question);
        for (int i = 0; i < inputValues.Length; i++)
        {
            Console.WriteLine($"{i + 1} - {inputValues[i]}");
        }

        string? value = null;
        while (value == null)
        {
            var input = Console.ReadLine();
            if (input == null) continue;

            if (ParseInt(input) is int index && index >= 1 && index <= inputValues.Length)
            {
                value = inputValues[index - 1];
            }
            else
            {
                input = input.ToLower();
                var foundValue = inputValues.FirstOrDefault(value => value.ToLower() == input);
                if (foundValue != null) value = foundValue;
            }
        }

        return value;
    }

    private static int? ParseInt(string input)
    {
        try
        {
            return int.Parse(input);
        }
        catch (FormatException)
        {
            return null;
        }
    }
}
