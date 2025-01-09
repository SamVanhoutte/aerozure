using System;
using System.Collections.Generic;
using Flurl.Util;
using Serilog.Context;

namespace Sfinx.Backend.Core.Observability
{
    public class TraceContext : IDisposable
    {
        private List<IDisposable> disposables = new List<IDisposable>();

        public TraceContext(Dictionary<string, object> traceContext)
        {
            disposables = new List<IDisposable>();
            foreach (var traceProperty in traceContext)
            {
                disposables.Add(LogContext.PushProperty(traceProperty.Key, traceProperty.Value));
            }
        }

        public void Dispose()
        {
            if (disposables != null)
            {
                foreach (var disposable in disposables)
                {
                    disposable.Dispose();
                }
            }
        }
    }
}