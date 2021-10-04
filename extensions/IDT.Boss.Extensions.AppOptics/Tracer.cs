using System;
using System.Collections.Generic;
using AppOptics.Instrumentation;

namespace IDT.Boss.Extensions.AppOptics
{
    /// <summary>
    /// Custom tracer to trace events with AppOptics.
    /// </summary>
    public class Tracer : IDisposable
    {
        private bool _isDisposed;
        private readonly string _spanName;
        private readonly bool _isTransaction;

        private Tracer(string spanName, bool isTransaction)
        {
            _spanName = spanName;
            _isTransaction = isTransaction;
        }

        public static void AddInfo(string layerName, IDictionary<string, object> additionalValues = null)
        {
            var info = Trace.CreateInfoEvent(layerName);
            if (additionalValues != null)
            {
                info.AddInfo(additionalValues);
            }

            info.Report();
        }

        public static Tracer TraceMethod(string spanName, IDictionary<string, object> additionalValues = null, bool isTransaction = false)
        {
            var trace = isTransaction
                ? (ITraceEvent) Trace.StartTrace(spanName)
                : Trace.CreateEntryEvent(spanName);
            if (additionalValues != null)
            {
                trace.AddInfo(additionalValues);
            }
            trace.Report();
            return new Tracer(spanName, isTransaction);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            if (disposing)
            {
                // free managed resources
                if (_isTransaction)
                {
                    Trace.EndTrace(_spanName).Report();
                }
                else
                {
                    Trace.CreateExitEvent(_spanName).Report();
                }
            }

            _isDisposed = true;
        }
    }
}