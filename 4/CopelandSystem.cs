using System.Collections.Generic;
using System.Linq;

namespace CollectiveDecision
{
    // Система принятия коллективного решения по модели Кондорсе с модификацией по правилу Копленда
    class CopelandSystem : CondorcetSystem
    {
        private Dictionary<string, int> CandidatesPoints;

        // Вычисление оценки Копленда: при сравнении в каждой паре кандидату начисляется 1 очко,
        // если он предпочтительнее для большинства выборщиков, и вычитается 1 очко, если для меньшинства
        public CopelandSystem(IEnumerable<IEnumerable<string>> voteOrders) : base(voteOrders)
        {
            CandidatesPoints = CandidatePairValues
                .Select(candidate => new
                {
                    Candidate = candidate.Key,
                    Points = candidate.Value.Aggregate(
                        0,
                        (i, pairDifference) =>
                            pairDifference.Value < 0 ? i - 1 :
                            pairDifference.Value > 0 ? i + 1 :
                            i
                    ),
                })
                .ToDictionary(
                    candidatePoints => candidatePoints.Candidate,
                    candidatePoints => candidatePoints.Points
                );
        }

        // Побеждает кандидат с максимальным значением оценки Копленда
        protected override bool Wins(string candidate) => CandidatesPoints[candidate] == MaximumPoints;

        private int MaximumPoints => CandidatesPoints
            .Select(candidatePoints => candidatePoints.Value)
            .Max();

        protected override string GetMatrixLine(string firstCandidate)
        {
            return base.GetMatrixLine(firstCandidate) + $"     Оценка Копленда: {CandidatesPoints[firstCandidate],5}";
        }
    }
}
