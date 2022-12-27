using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpertSystem;

class ExplanationComponent
{
    private MemoryComponent Memory;

    public ExplanationComponent(MemoryComponent memory)
    {
        Memory = memory;
    }

    public void GetReasoning(Fact resultFact)
    {
        Stack<Fact> stack = new();
        stack.Push(resultFact);

        while (stack.Any())
        {
            var fact = stack.Pop();
            Console.WriteLine($"\nФакт: {fact}");
                
            var reason = Memory.Facts[fact];
            Console.WriteLine($"Добавлен в результате срабатывания правила: {reason}");
                
            var requirements = reason is Rule rule ? Memory.ActivatedRules[rule] : null;

            if (requirements is List<Fact> facts && facts.Any())
            {
                Console.WriteLine($"В связи с наличием фактов:\n{string.Join("\n", requirements)}");
                facts.ForEach(requirement => stack.Push(requirement));
            }
        }
    }
}
