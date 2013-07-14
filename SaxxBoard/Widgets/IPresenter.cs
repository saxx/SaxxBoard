using Raven.Client;
using System;
using System.Collections.Generic;

namespace SaxxBoard.Widgets
{
    public interface IPresenter
    {
        IEnumerable<IPresenterDataPoint> GetData(IDocumentSession dbSession, DateTime startDate, DateTime endDate);
        IWidget Widget { get; }
    }
}