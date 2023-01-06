using System.Collections.Generic;
using System.Linq;

namespace CollectiveDecision
{
    // Система принятия коллективного решения по принципу относительного большинства
    class PluralitySystem
    {
        private IEnumerable<string> Votes;
        // Проценты всех кандидатов. Ключ - кандидат, значение - процент
        private Dictionary<string, double> Percentages;
        public PluralitySystem(IEnumerable<string> votes)
        {
            Votes = votes;
            Percentages = GetPercentages();
        }

        // Подсчет процентов голосов для каждого кандидата
        private Dictionary<string, double> GetPercentages()
        {
            return Votes
                .GroupBy(_ => _)
                .OrderByDescending(votesGroup => votesGroup.Count())
                .ToDictionary(
                    votesGroup => votesGroup.Key,
                    votesGroup => (double) votesGroup.Count() / Votes.Count()
                );
        }

        // Вывод победителя и процентов голосов для каждого кандидата
        public string GetDecision()
        {
            var winnerPercentage = Percentages.First();
            var winner = HasSingleWinner()
                ? $"Победитель: {winnerPercentage.Key} с {winnerPercentage.Value:P} голосов."
                : "Невозможно определить победителя";
            return $"{winner}\n" +
                   string.Join("\n", Percentages.Select(percentage => $"{percentage.Key} - {percentage.Value:P}"));
        }

        // Проверка наличия победителя (Нет двух кандидатов, имеющих процент голосов, равный максимальному)
        private bool HasSingleWinner()
        {
            if (Percentages.Count <= 1) return true;
            var first = Percentages.First();
            var second = Percentages.Skip(1).First();
            return first.Value != second.Value;
        }
    }

}
