using System;

namespace SaxxBoard.Widgets.NewRelicWidget
{
    public class NewRelicPresenter : SimplePresenter
    {
        protected override double? CalculateValue(double? rawValue)
        {
            var config = (NewRelicConfiguration)Widget.GetConfiguration();
            if (config.ValueIsBytes)
                return rawValue.HasValue ? new double?(Math.Round(rawValue.Value / 1024.0 / 1024.0, 2)) : null;
            return base.CalculateValue(rawValue);
        }

        public override string FormatValue(double? rawValue)
        {
            var config = (NewRelicConfiguration)Widget.GetConfiguration();
            if (rawValue.HasValue && config.ValueIsBytes)
                return rawValue.Value.ToString("N2") + " MB";
            if (rawValue.HasValue && config.ValueIsPercent)
                return rawValue.Value.ToString("N0") + " %";
            if (rawValue.HasValue && config.ValueIsApdex)
                return rawValue.Value.ToString("N3");
            if (rawValue.HasValue && config.ValueIsSeconds)
                return rawValue.Value.ToString("N2") + "s";
            return base.FormatValue(rawValue);
        }
    }
}