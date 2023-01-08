using System.Collections.Generic;
using System.Linq;

namespace CollectiveDecision
{
    // Система принятия коллективного решения по модели Кондорсе с модификацией по правилу Симпсона
    class SimpsonSystem : CondorcetSystem
    {
        private Dictionary<string, int> CandidatesPoints;

        // Вычисление оценки Симпсона для каждого кандидата: в каждой паре кандидатов подсчитывается количество выборщиков,
        // считающих данного кандидата предпочтительнее, из всех таких чисел выбирается минимальное
        public SimpsonSystem(IEnumerable<IEnumerable<string>> voteOrders) : base(voteOrders)
        {
            CandidatesPoints = CandidatePairValues
                .Select(candidate => new
                {
                    Candidate = candidate.Key,
                    Points = candidate.Value.Values.Min(),
                })
                .ToDictionary(
                    candidatePoints => candidatePoints.Candidate,
                    candidatePoints => candidatePoints.Points
                );
        }

        protected override int GetPairValue(string first, string second) => GetPairDifference(first, second).timesFirstHigher;

        // Побеждает кандидат с максимальным значением оценки Симпсона
        protected override bool Wins(string candidate) => CandidatesPoints[candidate] == MaximumPoints;

        private int MaximumPoints => CandidatesPoints
            .Select(candidatePoints => candidatePoints.Value)
            .Max();
        
        protected override string GetMatrixLine(string firstCandidate)
        {
            return base.GetMatrixLine(firstCandidate) + $"     Оценка Симпсона: {CandidatesPoints[firstCandidate],5}";
        }
    }
}
