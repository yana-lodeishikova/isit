using System.Collections.Generic;
using System.Linq;

namespace CollectiveDecision
{
    // Система принятия коллективного решения по модели Кондорсе
    public class CondorcetSystem : VoteOrderSystem
    {
        private Dictionary<string, Dictionary<string, int>> CandidatePairValues;
        
        public CondorcetSystem(IEnumerable<IEnumerable<string>> voteOrders) : base(voteOrders)
        {
            CandidatePairValues = GetPairValues();
        }

        // Вычисление попарных предпочтений: для каждой (упорядоченной) пары вычисляется,
        // как часто первый элемент оказывается более предпочтительным, чем второй
        private Dictionary<string, Dictionary<string, int>> GetPairValues()
        {
            return Candidates
                .ToDictionary(
                    firstCandidate => firstCandidate,
                    firstCandidate => Candidates
                        .Where(secondCandidate => firstCandidate != secondCandidate)
                        .ToDictionary(
                            secondCandidate => secondCandidate,
                            secondCandidate => GetPairValue(firstCandidate, secondCandidate)
                        )
                );
        }

        private int GetPairValue(string first, string second) 
        {
            var (timesFirstHigher, timesSecondHigher) = GetPairDifference(first, second);
            return timesFirstHigher - timesSecondHigher;
        }

        // Вычисление количества случаев, когда первый кандидат оказывается предпочтительнее второго, и наоборот
        private (int timesFirstHigher, int timesSecondHigher) GetPairDifference(string first, string second)
        {
            int timesFirstHigher = 0;
            int timesSecondHigher = 0;

            foreach (var voteOrder in VoteOrders)
            {
                var voteOrderList = voteOrder.ToList();
                int indexFirst = voteOrderList.IndexOf(first);
                int indexSecond = voteOrderList.IndexOf(second);
                if (indexFirst < indexSecond)
                {
                    timesFirstHigher++;
                }
                else if (indexFirst > indexSecond)
                {
                    timesSecondHigher++;
                }
            }

            return (timesFirstHigher, timesSecondHigher);
        }

        // Вывод результата коллективного решения
        public string GetDecision()
        {
            var winners = Candidates.Where(Wins).ToArray();
            var winner = winners.Length == 1
                ? $"Победитель: {winners.First()}, т.к. большинство выборщиков предпочитает именно этого кандидата при попарном сравнении с остальными."
                : "Невозможно определить победителя";
            
            return
                $"{winner}\n" +
                "Матрица попарного предпочтения:\n" +
                string.Join("\n", Candidates.Select(GetMatrixLine));
        }

        private string GetMatrixLine(string firstCandidate)
        {
            return $"{firstCandidate,30}  " +
                string.Join(" ", Candidates.Select(secondCandidate => GetMatrixPairValue(firstCandidate, secondCandidate)));
        }

        private string GetMatrixPairValue(string firstCandidate, string secondCandidate)
        {
            int value = firstCandidate == secondCandidate
                ? 0
                : CandidatePairValues[firstCandidate][secondCandidate];
            return value.ToString().PadLeft(5);
        }

        // Условие победы: кандидат оказывается предпочтительнее любого другого кандидата в большинстве случаев
        private bool Wins(string candidate) => CandidatePairValues[candidate].All(pairDifference => pairDifference.Value > 0);
    }
}
