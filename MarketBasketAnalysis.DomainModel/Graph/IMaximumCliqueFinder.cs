using System.Collections.Generic;

namespace MarketBasketAnalysis.DomainModel.Graph;

public interface IMaximumCliqueFinder
{
    IReadOnlyCollection<Graph> Find(Graph graph, int minCliqueSize, int maxCliqueSize,
        bool considerDirection);
}