using System.Collections.Generic;
using System.Linq;

namespace CollectiveDecision
{
    // Абстрактная система коллективного решения на основе упорядоченного списка предпочтений
    public abstract class VoteOrderSystem
    {
        protected IEnumerable<IEnumerable<string>> VoteOrders;
        protected IEnumerable<string> Candidates;
        protected VoteOrderSystem(IEnumerable<IEnumerable<string>> voteOrders)
        {
            VoteOrders = voteOrders;
            Candidates = GetUniqueCandidates();
        }

        // Определение полного списка кандидатов по спискам предпочтений
        private IEnumerable<string> GetUniqueCandidates() => VoteOrders.SelectMany(_ => _).Distinct();
    }
}
