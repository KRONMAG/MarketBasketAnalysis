using ConcurrentCollections;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.ContractsLight;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static MarketBasketAnalysis.DomainModel.Mining.ItemsetConversionResult;

namespace MarketBasketAnalysis.DomainModel.Mining;

public sealed class Miner : IMiner
{
    #region Fields and Properties

    public event EventHandler<double>? MiningProgressChanged;

    public event EventHandler<MiningStage>? MiningStageChanged;

    #endregion Fields and Properties

    #region Methods

    public IReadOnlySet<AssociationRule> Mine(IEnumerable<Transaction> transactions, double minSupport,
        double minConfidence, IItemsetConverter? itemsetConverter = null, IItemExcluder? itemExcluder = null,
        CancellationToken token = default)
    {
        Contract.Requires(minSupport >= 0);
        Contract.Requires(minConfidence >= 0);
        Contract.RequiresNotNull(itemsetConverter);
        Contract.RequiresNotNull(itemExcluder);

        try
        {
            MiningStageChanged?.Invoke(this, MiningStage.FrequentItemSearch);

            var frequentItems = SearchForFrequentItems(transactions, minSupport, itemsetConverter, itemExcluder, token,
                out var transactionCount);

            MiningStageChanged?.Invoke(this, MiningStage.FrequentItemsetSearch);

            var frequentItemsets = SearchForFrequentItemsets(transactions, frequentItems, itemsetConverter, minSupport,
                transactionCount, token);

            MiningStageChanged?.Invoke(this, MiningStage.AssociationRuleGeneration);

            return GenerateAssociationRules(frequentItemsets, frequentItems, minConfidence, transactionCount, token);
        }
        catch(Exception e)
        {
            throw new MinerException(Messages.Miner_MiningProcessFailed, e);
        }
    }

    private static IReadOnlyDictionary<string, int> SearchForFrequentItems(IEnumerable<Transaction> transactions,
        double minSupport, IItemsetConverter? itemsetConverter, IItemExcluder? itemExcluder, CancellationToken token,
        out int transactionCount)
    {
        var allowedItems = new ConcurrentHashSet<string>();
        var excludedItems = new ConcurrentHashSet<string>();
        var itemFrequencies = new ConcurrentDictionary<string, int>();

        transactionCount = transactions
            .AsParallel()
            .WithCancellation(token)
            .Count(transaction =>
            {
                foreach (var item in transaction.Items)
                {
                    var resultItem = itemsetConverter?.TryConvert(item, out var convertedItem) ?? false
                        ? convertedItem
                        : item;

                    if (itemExcluder != null)
                    {
                        if (excludedItems.Contains(resultItem))
                            continue;

                        if (!allowedItems.Contains(resultItem) &&
                            itemExcluder.ShouldExclude(resultItem))
                        {
                            excludedItems.Add(resultItem);

                            continue;
                        }
                        else
                        {
                            allowedItems.Add(resultItem);
                        }
                    }

                    itemFrequencies.AddOrUpdate(resultItem, 1, (_, value) => value + 1);
                }

                return true;
            });

        var frequencyThreshold = (int)Math.Ceiling(transactionCount * minSupport);

        return itemFrequencies
            .Where(keyValuePair => keyValuePair.Value >= frequencyThreshold)
            .ToDictionary(keyValuePair => keyValuePair.Key, keyValuePair => keyValuePair.Value);
    }

    private IReadOnlyDictionary<Itemset, int> SearchForFrequentItemsets(IEnumerable<Transaction> transactions,
        IReadOnlyDictionary<string, int> frequentItems, IItemsetConverter? itemsetConverter, double minSupport,
        int transactionCount, CancellationToken token)
    {
        var frequencyThreshold = (int)Math.Ceiling(transactionCount * minSupport);
        var itemsets = new ConcurrentDictionary<Itemset, int>();
        var processedTransactionCount = 0;

        using var timer = new Timer
        (
            callback: _ => MiningProgressChanged?.Invoke(this, processedTransactionCount / (double)transactionCount * 100),
            state: null,
            dueTime: 0,
            period: 100
        );

        transactions
            .AsParallel()
            .WithCancellation(token)
            .ForAll(transaction =>
            {
                foreach (var itemset in transaction.Itemsets)
                {
                    var resultItemset = itemsetConverter != null
                        ? itemsetConverter.TryConvert(itemset, out var convertedItemset) switch
                        {
                            ItemsetConverted => convertedItemset,
                            NoConversionRequired => itemset,
                            ConvertedItemsetHasSameItems => null,
                            _ => null
                        }
                        : itemset;

                    if (resultItemset != null &&
                        frequentItems.ContainsKey(resultItemset.FirstItem) &&
                        frequentItems.ContainsKey(resultItemset.SecondItem))
                    {
                        itemsets.AddOrUpdate(itemset, 1, (_, value) => value + 1);
                    }

                    processedTransactionCount++;
                }
            });

        return itemsets
            .Where(keyValuePair => keyValuePair.Value >= frequencyThreshold)
            .ToDictionary(keyValuePair => keyValuePair.Key, keyValuePair => keyValuePair.Value);
    }

    private static IReadOnlySet<AssociationRule> GenerateAssociationRules(IReadOnlyDictionary<Itemset, int> frequentItemsets,
        IReadOnlyDictionary<string, int> frequentItems, double minConfidence, int transactionCount, CancellationToken token)
    {
        IEnumerable<AssociationRule> GenerateAssociationRules(Itemset itemset, int itemsetFrequency)
        {
            var firstItemFrequency = frequentItems[itemset.FirstItem];
            var secondItemFrequency = frequentItems[itemset.SecondItem];

            if (itemsetFrequency / (double)firstItemFrequency > minConfidence)
            {
                yield return new AssociationRule(itemset.FirstItem, itemset.SecondItem, firstItemFrequency,
                    secondItemFrequency, itemsetFrequency, transactionCount);
            }

            if (itemsetFrequency / (double)secondItemFrequency > minConfidence)
            {
                yield return new AssociationRule(itemset.SecondItem, itemset.FirstItem, secondItemFrequency,
                    firstItemFrequency, itemsetFrequency, transactionCount);
            }
        }

        return frequentItemsets
            .AsParallel()
            .WithCancellation(token)
            .SelectMany(keyValuePair => GenerateAssociationRules(keyValuePair.Key, keyValuePair.Value))
            .ToHashSet();
    }

    #endregion Methods
}