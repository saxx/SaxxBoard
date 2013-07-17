
using System.Collections.Generic;

namespace SaxxBoard.Widgets
{
    public interface IConfiguration
    {
        int RefreshIntervalInSeconds { get; }
        int MaxDataPointsInChart { get; }
        int MaxDataPointsToStore { get; }

        double? MinTickSizeOnChart { get; }
        double? MaxValueOnChart { get; }

        bool HigherValueIsBetter { get; }
        bool SumInsteadOfAverage { get; }

        IEnumerable<string> SeriesLabels { get; }

        IWidget Widget { get; set; }
    }
}