using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
#pragma warning disable CA1062 // Validate arguments of public methods

namespace MarketBasketAnalysis.Benchmarks.Benchmarks;

public class AssociationRulesColumn : IColumn
{
    public const string MarkerPrefix = "// AssociationRules:";

    public string Id => nameof(AssociationRulesColumn);

    public string ColumnName => "AssociationRules";

    public bool AlwaysShow => true;

    public ColumnCategory Category => ColumnCategory.Custom;

    public bool IsNumeric => true;

    public UnitType UnitType => UnitType.Dimensionless;

    public string Legend => "Number of found association rules";

    public bool IsAvailable(Summary summary) => true;

    public bool IsDefault(Summary summary, BenchmarkCase benchmarkCase) => true;

    public int PriorityInCategory => 0;

    public string GetValue(Summary summary, BenchmarkCase benchmarkCase) =>
        GetValue(summary, benchmarkCase, SummaryStyle.Default);

    public string GetValue(Summary summary, BenchmarkCase benchmarkCase, SummaryStyle style)
    {
        var report = summary.Reports.FirstOrDefault(r => r.BenchmarkCase == benchmarkCase);

        if (report == null)
        {
            return "?";
        }

        var columnLine = report.ExecuteResults
            .SelectMany(executeResult => executeResult.StandardOutput)
            .FirstOrDefault(l => l.StartsWith(MarkerPrefix, StringComparison.Ordinal));

        if (columnLine == null)
        {
            return "-";
        }

        return columnLine.Substring(MarkerPrefix.Length).Trim();
    }
}
