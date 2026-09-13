using System.Globalization;
using System.IO.Compression;
using System.Text;
using MarketBasketAnalysis.Models;

namespace MarketBasketAnalysis.Benchmarks.Helpers;

public static class InstacartDatasetContext
{
    private const string InstacartDatasetAddress = "https://www.kaggle.com/api/v1/datasets/download/psparks/instacart-market-basket-analysis/";
    private const int FirstDigitChar = 0x0030;
    private const int LastDigitChar = 0x0039;

    private const string ProductsFileName = "products.csv";
    private const string OrderProductsPriorFileName = "order_products__prior.csv";
    private const string OrderProductsTrainFileName = "order_products__train.csv";
    private const int BufferSize = 65536;
    private const char Separator = ',';

    private static IReadOnlyDictionary<int, Item>? items;

    public static bool IsInitialized { get; private set; }

    private static readonly HttpClient HttpClient = new()
    {
        BaseAddress = new(InstacartDatasetAddress),
    };

    public static async Task InitializeAsync()
    {
        if (IsInitialized)
        {
            throw new InvalidOperationException("Already initialized.");
        }

        await DownloadAndUnpackArchiveIfNeedAsync(ProductsFileName);
        await DownloadAndUnpackArchiveIfNeedAsync(OrderProductsPriorFileName);
        await DownloadAndUnpackArchiveIfNeedAsync(OrderProductsTrainFileName);

        items = ReadItems();

        IsInitialized = true;
    }

    public static IEnumerable<IReadOnlyList<Item>> ReadTransactions()
    {
        if (!IsInitialized)
        {
            throw new InvalidOperationException("Not initialized");
        }

        var transactions = ReadTransactionFile(OrderProductsPriorFileName)
            .Concat(ReadTransactionFile(OrderProductsTrainFileName));

        foreach (var transaction in transactions)
        {
            yield return transaction;
        }
    }

    private static IReadOnlyDictionary<int, Item> ReadItems()
    {
        var itemsMap = new Dictionary<int, Item>();

        foreach (var line in ReadAllLines(ProductsFileName))
        {
            using var spans = line.SplitAny(Separator);

            spans.MoveNext();

            var itemIdSpan = spans.Current;
            var itemId = int.Parse(line[itemIdSpan], CultureInfo.InvariantCulture);

            spans.MoveNext();

            var itemNameSpan = spans.Current;
            var itemName = line[itemNameSpan];

            var item = new Item(itemId, itemName);

            itemsMap.Add(item.Id, item);
        }

        return itemsMap;
    }

    private static IEnumerable<IReadOnlyList<Item>> ReadTransactionFile(string path)
    {
        using var streamReader = new StreamReader(path, Encoding.UTF8, false, BufferSize);
        var prevTransactionId = -1;
        var transaction = new List<Item>();

        while (SkipToNewLine(streamReader))
        {
            if (!TryReadRow(streamReader, out var transactionId, out var itemId))
            {
                break;
            }

            if (transactionId == prevTransactionId || prevTransactionId == -1)
            {
                transaction.Add(items![itemId]);
            }
            else
            {
                yield return transaction;
                transaction = [items![itemId]];
            }

            prevTransactionId = transactionId;
        }

        yield return transaction;
    }

    private static IEnumerable<string> ReadAllLines(string filename)
    {
        using var streamReader = new StreamReader(filename, Encoding.UTF8, false, BufferSize);

        string? line;

        streamReader.ReadLine();

        while ((line = streamReader.ReadLine()) != null)
        {
            yield return line;
        }
    }

    private static bool TryReadRow(StreamReader streamReader, out int transactionId, out int itemId)
    {
        transactionId = 0;
        itemId = 0;

        var character = streamReader.Read();

        if (character < FirstDigitChar || character > LastDigitChar)
        {
            return false;
        }

        transactionId = character - FirstDigitChar;

        while ((character = streamReader.Read()) != ',')
        {
            transactionId = transactionId * 10 + (character - FirstDigitChar);
        }

        while ((character = streamReader.Read()) != ',')
        {
            itemId = itemId * 10 + (character - FirstDigitChar);
        }

        return true;
    }

    private static bool SkipToNewLine(StreamReader streamReader)
    {
        int character;

        while ((character = streamReader.Read()) != '\n')
        {
            if (character == -1)
            {
                return false;
            }
        }

        return true;
    }

    private static async Task DownloadAndUnpackArchiveIfNeedAsync(string filename)
    {
        if (File.Exists(filename))
        {
            return;
        }

        var fileNameUri = new Uri(filename);
        await using var responseStream = await HttpClient.GetStreamAsync(fileNameUri);

        var archivePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

        await using var archiveStream = File.Create(archivePath, BufferSize, FileOptions.DeleteOnClose);

        await responseStream.CopyToAsync(archiveStream, BufferSize);

        var archive = await ZipArchive.CreateAsync(archiveStream, ZipArchiveMode.Read, false, Encoding.UTF8);

        await archive.ExtractToDirectoryAsync(Environment.CurrentDirectory, true);
    }
}
