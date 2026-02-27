//
//  Copyright (c) 2022 Sendbird, Inc.
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Sendbird.Chat
{
    internal static class JsonMemoryProfiler
    {
        internal struct Snapshot
        {
            internal string Label;
            internal long MemoryBytes;
            internal long DeltaFromPrevious;
            internal long DeltaFromBaseline;
            internal long InputSizeBytes;
            internal long ElapsedMs;
        }

        internal static bool IsEnabled { get; set; } = true;

        private static readonly List<Snapshot> _snapshots = new List<Snapshot>();
        private static long _baselineMemory;
        private static long _previousMemory;
        private static readonly Stopwatch _stopwatch = new Stopwatch();

        [Conditional("SB_LOG_INFO_BELOW")]
        internal static void BeginSession(string inLabel)
        {
            if (!IsEnabled)
                return;

            _snapshots.Clear();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            _baselineMemory = GC.GetTotalMemory(true);
            _previousMemory = _baselineMemory;
            _stopwatch.Restart();

            _snapshots.Add(new Snapshot
            {
                Label = $"[Session Start] {inLabel}",
                MemoryBytes = _baselineMemory,
                DeltaFromPrevious = 0,
                DeltaFromBaseline = 0,
                InputSizeBytes = 0,
                ElapsedMs = 0
            });
        }

        [Conditional("SB_LOG_INFO_BELOW")]
        internal static void TakeSnapshot(string inLabel, long inInputSizeBytes = 0)
        {
            if (!IsEnabled)
                return;

            long currentMemory = GC.GetTotalMemory(false);
            long elapsedMs = _stopwatch.ElapsedMilliseconds;

            _snapshots.Add(new Snapshot
            {
                Label = inLabel,
                MemoryBytes = currentMemory,
                DeltaFromPrevious = currentMemory - _previousMemory,
                DeltaFromBaseline = currentMemory - _baselineMemory,
                InputSizeBytes = inInputSizeBytes,
                ElapsedMs = elapsedMs
            });

            _previousMemory = currentMemory;
        }

        internal static string EndSessionAndGetReport()
        {
            if (!IsEnabled)
                return string.Empty;

            _stopwatch.Stop();

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            long finalMemory = GC.GetTotalMemory(true);
            long totalElapsedMs = _stopwatch.ElapsedMilliseconds;

            long peakDelta = 0;
            foreach (Snapshot snapshot in _snapshots)
            {
                if (snapshot.DeltaFromBaseline > peakDelta)
                    peakDelta = snapshot.DeltaFromBaseline;
            }

            long retainedDelta = finalMemory - _baselineMemory;

            // Find the max input size from snapshots for multiplier calculation
            long maxInputSizeBytes = 0;
            foreach (Snapshot snapshot in _snapshots)
            {
                if (snapshot.InputSizeBytes > maxInputSizeBytes)
                    maxInputSizeBytes = snapshot.InputSizeBytes;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("========== JSON Memory Profile Report ==========");

            foreach (Snapshot snapshot in _snapshots)
            {
                sb.AppendFormat("  [{0,6}ms] {1}", snapshot.ElapsedMs, snapshot.Label);
                if (snapshot.InputSizeBytes > 0)
                    sb.AppendFormat(" (input: {0:F2} MB)", snapshot.InputSizeBytes / (1024.0 * 1024.0));
                sb.AppendLine();

                sb.AppendFormat("           Memory: {0:F2} MB | Delta: {1:+0.00;-0.00;+0.00} MB | From baseline: {2:+0.00;-0.00;+0.00} MB",
                    snapshot.MemoryBytes / (1024.0 * 1024.0),
                    snapshot.DeltaFromPrevious / (1024.0 * 1024.0),
                    snapshot.DeltaFromBaseline / (1024.0 * 1024.0));
                sb.AppendLine();
            }

            sb.AppendLine("  ------------------------------------------------");
            sb.AppendFormat("  Peak from baseline: {0:F2} MB", peakDelta / (1024.0 * 1024.0));
            sb.AppendLine();
            sb.AppendFormat("  Retained after GC:  {0:F2} MB", retainedDelta / (1024.0 * 1024.0));
            sb.AppendLine();
            sb.AppendFormat("  Total time:         {0} ms", totalElapsedMs);
            sb.AppendLine();

            if (maxInputSizeBytes > 0)
            {
                double inputSizeMB = maxInputSizeBytes / (1024.0 * 1024.0);
                double peakMultiplier = peakDelta / (double)maxInputSizeBytes;
                double retainedMultiplier = retainedDelta > 0 ? retainedDelta / (double)maxInputSizeBytes : 0;
                sb.AppendLine("  ---- Analysis ----");
                sb.AppendFormat("  Input size:         {0:F2} MB", inputSizeMB);
                sb.AppendLine();
                sb.AppendFormat("  Peak / Input:       {0:F2}x", peakMultiplier);
                sb.AppendLine();
                sb.AppendFormat("  Retained / Input:   {0:F2}x", retainedMultiplier);
                sb.AppendLine();
            }

            sb.AppendLine("=================================================");

            return sb.ToString();
        }

        [Conditional("SB_LOG_INFO_BELOW")]
        internal static void EndSessionAndLogReport()
        {
            string report = EndSessionAndGetReport();
            if (!string.IsNullOrEmpty(report))
            {
                Logger.Info(Logger.CategoryType.Json, report);
            }
        }

        [Conditional("SB_LOG_INFO_BELOW")]
        internal static void LogReport(string inReport)
        {
            if (!string.IsNullOrEmpty(inReport))
            {
                Logger.Info(Logger.CategoryType.Json, inReport);
            }
        }

        internal static List<Snapshot> GetSnapshots()
        {
            return new List<Snapshot>(_snapshots);
        }

        [Conditional("SB_LOG_INFO_BELOW")]
        internal static void Reset()
        {
            _snapshots.Clear();
            _baselineMemory = 0;
            _previousMemory = 0;
            _stopwatch.Reset();
        }
    }
}
