using System;
using System.IO;
using System.Text.Json;
using ExpertSystem;

var options = new JsonSerializerOptions { IncludeFields = true, IgnoreReadOnlyFields = true, IgnoreReadOnlyProperties = true };
options.Converters.Add(new ConditionConverter());

var jsonString = File.ReadAllText("../../../Rules.json");
var rules = JsonSerializer.Deserialize<Rule[]>(jsonString, options);

var memory = new MemoryComponent(rules);
var inferenceComponent = new InferenceComponent(memory);

var result = inferenceComponent.GetResult();

if (result is Fact resultFact)
{
    Console.WriteLine($"Результат:\n{resultFact.Name}");
    Console.ReadKey();
}
else
{
    Console.WriteLine("Результат не определен");
}
