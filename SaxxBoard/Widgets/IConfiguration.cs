
namespace SaxxBoard.Widgets
{
    public interface IConfiguration
    {
        int RefreshIntervalInSeconds { get; }
        int MaxDataPointsInChart { get; }
        int MaxDataPointsToStore { get; }

        bool IsScaledToPercents { get; }

        IWidget Widget { get; set; }
    }
}