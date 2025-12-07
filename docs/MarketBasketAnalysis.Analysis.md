# <a id="MarketBasketAnalysis_Analysis"></a> Namespace MarketBasketAnalysis.Analysis

### Classes

 [MaximalClique<TVertex\>](MarketBasketAnalysis.Analysis.MaximalClique\-1.md)

Represents a maximal clique in a graph, defined as a set of vertices where every two distinct vertices are adjacent,
and no additional vertex can be added without breaking this property.

 [MaximalCliqueFindingParameters](MarketBasketAnalysis.Analysis.MaximalCliqueFindingParameters.md)

Represents the parameters used for finding maximal cliques in a graph of association rules.

### Interfaces

 [IMaximalCliqueAlgorithm](MarketBasketAnalysis.Analysis.IMaximalCliqueAlgorithm.md)

Defines an interface for algorithm that find all maximal cliques in an undirected graph.

 [IMaximalCliqueFinder](MarketBasketAnalysis.Analysis.IMaximalCliqueFinder.md)

Defines an interface for finding maximal cliques in a graph of association rules.
A maximal clique is a subset of association rules where every rule is connected to every other rule,
and no additional rules can be added without breaking this property.

