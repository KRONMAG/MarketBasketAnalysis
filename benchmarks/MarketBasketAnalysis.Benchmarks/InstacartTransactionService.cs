using System.Globalization;
using System.IO.Compression;
using System.Text;
using MarketBasketAnalysis.Models;

namespace MarketBasketAnalysis.Benchmarks;

#pragma warning disable SA0001

internal static class InstacartTransactionService
{
    public static async Task DownloadDatasetAsync()
    {
        await DownloadAndUnpackDatasetFileAsync("products.csv").ConfigureAwait(false);
        await DownloadAndUnpackDatasetFileAsync("order_products__train.csv").ConfigureAwait(false);
        await DownloadAndUnpackDatasetFileAsync("order_products__prior.csv").ConfigureAwait(false);
    }

    public static async IAsyncEnumerable<Item> ReadItems()
    {
        await foreach (var line in File.ReadLinesAsync("products.csv", Encoding.UTF8).Skip(1).ConfigureAwait(false))
        {
            var regions = line.SplitAny(',');

            regions.MoveNext();

            var itemIdRegion = regions.Current;
            var itemId = int.Parse(line[itemIdRegion], CultureInfo.InvariantCulture);

            regions.MoveNext();

            var itemNameRegion = regions.Current;
            var itemName = line[itemNameRegion];

            yield return new Item(itemId, itemName);
        }
    }

    public static async IAsyncEnumerable<IReadOnlyList<Item>> ReadTransactions(IReadOnlyList<Item> items)
    {
        var itemsMap = items.ToDictionary(static i => i.Id);
        var transaction = new List<Item>();
        var prevOrderId = -1;

        await foreach (var line in File
            .ReadLinesAsync("order_products__train.csv", Encoding.UTF8)
            .Skip(1)
            .Concat(File.ReadLinesAsync("order_products__prior.csv", Encoding.UTF8)
            .Skip(1))
            .ConfigureAwait(false))
        {
            var regions = line.SplitAny(',');

            regions.MoveNext();

            var orderIdRegion = regions.Current;
            var orderId = int.Parse(line[orderIdRegion], CultureInfo.InvariantCulture);

            regions.MoveNext();

            var itemIdRegion = regions.Current;
            var itemId = int.Parse(line[itemIdRegion], CultureInfo.InvariantCulture);

            if (orderId == prevOrderId || prevOrderId == -1)
            {
                transaction.Add(itemsMap[itemId]);
            }
            else
            {
                yield return transaction;
                transaction = new List<Item>();
            }

            prevOrderId = orderId;
        }

        yield return transaction;
    }

    private static async Task DownloadAndUnpackDatasetFileAsync(string filename)
    {
        if (File.Exists(filename))
        {
            return;
        }

        using var httpClient = new HttpClient()
        {
            BaseAddress = new("https://www.kaggle.com/api/v1/datasets/download/psparks/instacart-market-basket-analysis"),
        };

        using var responseStream = await httpClient.GetStreamAsync(new Uri(filename)).ConfigureAwait(false);

        var archive = await ZipArchive.CreateAsync(responseStream, ZipArchiveMode.Read, true, Encoding.UTF8).ConfigureAwait(false);

        await archive.ExtractToDirectoryAsync(Environment.CurrentDirectory, true).ConfigureAwait(false);
    }
}
