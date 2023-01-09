using System.Collections.Generic;
using System.Linq;

namespace CollectiveDecision
{
    // Система принятия коллективного решения по модели Борда
    class BordaSystem : VoteOrderSystem
    {
        private Dictionary<string, int> Points;
        
        public BordaSystem(IEnumerable<IEnumerable<string>> voteOrders) : base(voteOrders)
        {
            Points = GetPoints();
        }

        // Вычисление очков для каждого кандидата: в каждом списке предпочтительности кандидату дается столько
        // очков, сколько кандидатов оказываются менее предпочтительными для данного выборщика
        private Dictionary<string,int> GetPoints()
        {
            return Candidates
                .ToDictionary(
                    candidate => candidate,
                    GetPoints
                );
        }

        private int GetPoints(string candidate)
        {
            int points = 0;

            foreach (var voteOrder in VoteOrders)
            {
                int index = voteOrder.ToList().IndexOf(candidate);
                points += Candidates.Count() - index - 1;
            }

            return points;
        }

        // Вывод результата коллективного решения
        public string GetDecision()
        {
            var winners = Candidates.Where(Wins).ToArray();
            var winner = winners.Length == 1
                ? $"Победитель: {winners.First()} с {Points[winners.First()]} очков."
                : "Невозможно определить победителя";
            
            return
                $"{winner}\n" +
                "Распределение очков между кандидатами:\n" +
                string.Join("\n", Candidates.Select(candidate => $"{candidate,30}  {Points[candidate],5}"));
        }

        // Побеждает кандидат с максимальным количеством очков
        protected virtual bool Wins(string candidate) => Points[candidate] == Points.Values.Max();
    }
}
