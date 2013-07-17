using Raven.Client;
using System;
using System.Collections.Generic;

namespace SaxxBoard.Widgets
{
    public interface IPresenter
    {
        IEnumerable<IPresenterSeries> GetData(IDocumentSession dbSession);
        string FormatValue(double? rawValue);
        IWidget Widget { get; set; }
    }
}