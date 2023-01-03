using System;
using System.IO;
using System.Text.Json;
using ExpertSystem;

// Загрузка набора правил из файла
var options = new JsonSerializerOptions { IncludeFields = true, IgnoreReadOnlyFields = true, IgnoreReadOnlyProperties = true };
options.Converters.Add(new ConditionConverter());
var jsonString = File.ReadAllText("../../../Rules.json");
var rules = JsonSerializer.Deserialize<Rule[]>(jsonString, options);

// Создание компонентов экспертной системы: память, механизм вывода, компонент объяснения
var memory = new MemoryComponent(rules);
var inferenceComponent = new InferenceComponent(memory);
var explanationComponent = new ExplanationComponent(memory);

// Запуск работы системы: общение с пользователем, вывод результата и объяснения
var result = inferenceComponent.GetResult();
if (result is Fact resultFact)
{
    Console.WriteLine($"Результат:\n{resultFact.Name}");
    Console.ReadKey();
    explanationComponent.GetReasoning(resultFact);
}
else
{
    Console.WriteLine("Результат не определен");
}
