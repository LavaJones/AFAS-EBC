using Afas.Domain;
using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Afas
{

    public class PerformanceTiming : IDisposable
    {

        private class WatchData
        {
            public string Key { get; set; }

            public Stopwatch Watch { get; set; }

            public string MethodName { get; set; }

            public int NumberOfLaps { get; set; }

            public TimeSpan? LastLapEndTotalTime { get; set; }

        }

        private readonly string BaseLoggingFormat = "PerformanceTiming: RawTiming:[{0}]{1}, Class: [{2}], Method: [{3}], Section: [{4}], Message: [{5}]";

        private readonly string ActionCompletedFormat = "Action [{0}] Completed, taking [{1}]{2}; {3}";

        private readonly string ActionCompletedForItemsFormat = "Action [{0}] Completed, taking [{1}]{2}, Over [{3}] Items, with average time of [{4}]{5} per Item; {6}";
       
        private readonly string ActionCompletedForLapsFormat = "Action [{0}] Completed, taking [{1}]{2}, Over [{3}] Laps, with average time of [{4}]{5} per Lap; {6}";

        private readonly string LapCompletedFormat = "Lap [{0}] of action [{1}] Completed, taking [{2}]{3}, with average time of [{4}]{5}; {6}";

        private readonly ILog Log;

        private readonly string ClassName;

        private readonly bool IsDebugLoggingOn = false;

        private readonly bool IsInfoLoggingOn = false;

        private readonly Dictionary<string, WatchData> StopWatches = new Dictionary<string, WatchData>();

        public PerformanceTiming(Type ClassType, string MethodName = "", bool UsePerformanceLogger = false)
        {
            if (ClassType == null)
            {
                throw new ArgumentNullException("ClassType");
            }

            if (UsePerformanceLogger)
            {
                this.Log = LogManager.GetLogger(String.Format("PerformanceLogger.{0}", ClassType.FullName));
            }
            else
            {
                this.Log = LogManager.GetLogger(ClassType);
            }

            this.IsDebugLoggingOn = this.Log.IsDebugEnabled;
            this.IsInfoLoggingOn = this.Log.IsInfoEnabled;

            if (this.IsLogDisabled)
            {
                return;
            }

            this.ClassName = ClassType.Name;      

            this.MethodName = MethodName ?? string.Empty;        

            string LifetimeOf = this.MethodName.IsNullOrEmpty() ? this.ClassName : this.MethodName;
            this.StartTimer(LifetimeOf + " Lifetime", LifetimeOf);

            this.Log.Debug("Created Performance logger for Class: [" + this.ClassName + "], Method: [" + this.MethodName + "], with Debug: [" + IsDebugLoggingOn + "], and Info: [" + IsInfoLoggingOn + "]");

        }

        public string MethodName { get; set; }

        private WatchData GetData(string Key, string MethodName = null)
        {

            if (false == this.StopWatches.ContainsKey(Key))
            {

                this.StopWatches.Add(
                    Key,
                    new WatchData()
                    {
                        Key = Key,
                        Watch = new Stopwatch(),
                        MethodName = MethodName ?? this.MethodName,                  
                        NumberOfLaps = 0,
                        LastLapEndTotalTime = null
                    }
                    );

            }

            return this.StopWatches[Key];

        }

        private void RemoveWatch(string Key)
        {

            if (true == this.StopWatches.ContainsKey(Key))
            {

                this.StopWatches.Remove(Key);

            }

        }

        private bool IsLogDisabled
        {
            get
            {
                if (this.IsInfoLoggingOn || this.IsDebugLoggingOn)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        private bool CannotLog(bool InfoLevel)
        {
            if (false == ((InfoLevel && this.IsInfoLoggingOn) || (this.IsDebugLoggingOn)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool UseTicks(TimeSpan RawTimespan, bool TicksOverride = false)
        {

            if (RawTimespan.TotalMilliseconds < 1.0 || TicksOverride)
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        private bool TruncateMs(TimeSpan RawTimespan, bool TruncateOverride = false)
        {

            if (RawTimespan.TotalMilliseconds < 1000.0 || TruncateOverride || this.UseSeconds(RawTimespan))
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        private bool UseSeconds(TimeSpan RawTimespan, bool SecondsOverride = false)
        {
            if (RawTimespan.TotalSeconds < 10.0 || SecondsOverride)
            {
                return false;
            }
            else
            {
                return true;
            }

        }


        private void LogText(string Text, bool InfoLevel = false)
        {
            if (InfoLevel && this.IsInfoLoggingOn)
            {

                this.Log.Info(Text);

            }
            else if (this.IsDebugLoggingOn)
            {

                this.Log.Debug(Text);

            }
        }

        private string GetTextTime(TimeSpan RawTimespan, out string TimeUnit)
        {
            return this.GetTextTime(RawTimespan, out TimeUnit, this.UseTicks(RawTimespan), this.TruncateMs(RawTimespan), this.UseSeconds(RawTimespan));
        }

        private string GetTextTime(TimeSpan RawTimespan, out string TimeUnit, bool TickLevel, bool TruncateMs, bool UseSeconds)
        {

            string rawTime = string.Empty;
            TimeUnit = "";

            if (TickLevel)
            {
                rawTime = RawTimespan.Ticks.ToString();
                TimeUnit = "Ticks";
            }
            else if (TruncateMs)
            {
                rawTime = Math.Round(RawTimespan.TotalMilliseconds).ToString("N0");
                TimeUnit = "Miliseconds";

            }
            else if (UseSeconds)
            {
                rawTime = RawTimespan.TotalSeconds.ToString("N5");
                TimeUnit = "Seconds";
            }
            else
            {
                rawTime = RawTimespan.TotalMilliseconds.ToString("N5");
                TimeUnit = "MS";

            }

            return rawTime;
        }

        private void LogData(WatchData data, string Message, bool InfoLevel = false)
        {

            this.LogData(data, Message, this.UseTicks(data.Watch.Elapsed), this.TruncateMs(data.Watch.Elapsed), this.UseSeconds(data.Watch.Elapsed), InfoLevel);

        }

        private void LogData(WatchData data, string Message, bool TickLevel = false, bool TruncateMs = false, bool UseSeconds = false, bool InfoLevel = false)
        {

            string timeUnit = "ms";
            string rawTime = this.GetTextTime(data.Watch.Elapsed, out timeUnit, TickLevel, TruncateMs, UseSeconds);

            string fromatted = string.Format(this.BaseLoggingFormat, rawTime, timeUnit, this.ClassName, data.MethodName, data.Key, Message);

            this.LogText(fromatted, InfoLevel);

        }

        private TimeSpan Lap(WatchData Data)
        {
            TimeSpan lastLap = Data.Watch.Elapsed - (Data.LastLapEndTotalTime ?? new TimeSpan());

            Data.NumberOfLaps++;

            Data.LastLapEndTotalTime = Data.Watch.Elapsed;

            return lastLap;

        }

        public void StartLapTimer(string Key, string MethodName = null)
        {
            if (this.IsLogDisabled)
            {
                return;
            }

            WatchData data = this.GetData(Key, MethodName);

            data.Watch.Start();

        }

        public void UnpauseLapTimer(string Key)
        {
            if (this.IsLogDisabled)
            {
                return;
            }

            WatchData data = this.GetData(Key);

            data.Watch.Start();

        }


        public void StartLapTimerPaused(string Key, string MethodName = null)
        {
            if (this.IsLogDisabled)
            {
                return;
            }

            WatchData data = this.GetData(Key, MethodName);

            data.Watch.Stop();

        }

        public TimeSpan Lap(string Key)
        {
            if (this.IsLogDisabled)
            {
                return new TimeSpan();
            }

            WatchData data = this.GetData(Key);

            TimeSpan lapTime = this.Lap(data);

            data.Watch.Start();

            return lapTime;
        }

        public void LapAndSwitchTimers(string PauseKey, string UnpauseKey)
        {
            if (this.IsLogDisabled)
            {
                return;
            }

            this.LapAndPause(PauseKey);
            this.UnpauseLapTimer(UnpauseKey);
        }

        public TimeSpan LapAndPause(string Key)
        {
            if (this.IsLogDisabled)
            {
                return new TimeSpan();
            }

            WatchData data = this.GetData(Key);

            data.Watch.Stop();

            return this.Lap(data);
        }

        public void PauseLap(string Key)
        {
            if (this.IsLogDisabled)
            {
                return;
            }

            WatchData data = this.GetData(Key);

            data.Watch.Stop();

        }

        public void ResumeLap(string Key)
        {
            if (this.IsLogDisabled)
            {
                return;
            }

            WatchData data = this.GetData(Key);

            data.Watch.Start();

        }

        public TimeSpan LapPauseAndLog(string Key, string AdditionalText = "", bool InfoLevel = false)
        {
            if (this.IsLogDisabled)
            {
                return new TimeSpan();
            }

            return this.LapAndLog(Key, true, AdditionalText, InfoLevel);
        }

        public TimeSpan LapAndLog(string Key, string AdditionalText = "", bool InfoLevel = false)
        {
            if (this.IsLogDisabled)
            {
                return new TimeSpan();
            }

            return this.LapAndLog(Key, false, AdditionalText, InfoLevel);
        }

        public TimeSpan LapLogAndDispose(string Key, bool LogTotal = true, string AdditionalText = "", bool InfoLevel = false)
        {
            if (this.IsLogDisabled)
            {
                return new TimeSpan();
            }

            WatchData data = this.GetData(Key);

            data.Watch.Stop();

            if (LogTotal)
            {
                string timeUnit = string.Empty;
                string timeText = this.GetTextTime(data.Watch.Elapsed, out timeUnit);

                string message = string.Format(this.ActionCompletedFormat, data.Key, timeText, timeUnit, AdditionalText);

                AdditionalText = message;

            }

            TimeSpan lapTime = this.Lap(data);

            this.LogLap(data, lapTime, AdditionalText, InfoLevel);

            this.RemoveWatch(Key);

            return lapTime;
        }

        public void LogAllLapsAndDispose(string Key, string AdditionalText = "", bool InfoLevel = false)
        {
            if (this.IsLogDisabled)
            {
                return;
            }

            WatchData data = this.GetData(Key);

            data.Watch.Stop();

            this.LogTimeWithLaps(data, AdditionalText, InfoLevel);

            this.RemoveWatch(Key);

        }


        private TimeSpan LapAndLog(string Key, bool pause = false, string AdditionalText = "", bool InfoLevel = false)
        {
            if (this.CannotLog(InfoLevel))
            {
                return new TimeSpan();
            }

            WatchData data = this.GetData(Key);

            if (pause)
            {
                data.Watch.Stop();
            }

            TimeSpan lapTime = this.Lap(data);

            this.LogLap(data, lapTime, AdditionalText, InfoLevel);

            return lapTime;

        }

        private void LogLap(WatchData Data, TimeSpan LapTime, string AdditionalText = "", bool InfoLevel = false)
        {

            string lapTimeUnit = string.Empty;
            string lapTimeText = this.GetTextTime(LapTime, out lapTimeUnit);

            string averageLapUnit = string.Empty;
            string averageLapTime = string.Empty;

            if (Data.NumberOfLaps > 0)
            {
                averageLapTime = this.GetTextTime(new TimeSpan(Data.Watch.Elapsed.Ticks / Data.NumberOfLaps), out averageLapUnit);
            }

            string message = string.Format(this.LapCompletedFormat, Data.NumberOfLaps, Data.Key, lapTimeText, lapTimeUnit, averageLapTime, averageLapUnit, AdditionalText);

            this.LogData(Data, message, InfoLevel);

        }

        public void StartTimer(string Key, string MethodName = null)
        {
            if (this.IsLogDisabled)
            {
                return;
            }

            WatchData data = this.GetData(Key, MethodName);

            data.Watch.Start();

        }

        private void LogTime(WatchData Data, string AdditionalText = "", bool InfoLevel = false)
        {

            string timeUnit = string.Empty;
            string timeText = this.GetTextTime(Data.Watch.Elapsed, out timeUnit);

            string message = string.Format(this.ActionCompletedFormat, Data.Key, timeText, timeUnit, AdditionalText);

            this.LogData(Data, message, InfoLevel);

        }

        public void LogTime(string Key, string AdditionalText = "", bool InfoLevel = false)
        {
            if (this.IsLogDisabled)
            {
                return;
            }

            this.LogTime(Key, false, AdditionalText, InfoLevel);

        }

        public void LogTimeAndPause(string Key, string AdditionalText = "", bool InfoLevel = false)
        {
            if (this.IsLogDisabled)
            {
                return;
            }

            this.LogTime(Key, true, AdditionalText, InfoLevel);

        }

        public void LogTimeAndDispose(string Key, string AdditionalText = "", bool InfoLevel = false)
        {
            if (this.IsLogDisabled)
            {
                return;
            }

            this.LogTime(Key, false, AdditionalText, InfoLevel);
            this.RemoveWatch(Key);

        }

        private void LogTime(string Key, bool pause = false, string AdditionalText = "", bool InfoLevel = false)
        {
            if (this.CannotLog(InfoLevel))
            {
                return;
            }

            WatchData data = this.GetData(Key);

            if (pause)
            {
                data.Watch.Stop();
            }

            this.LogTime(data, AdditionalText, InfoLevel);

        }

        public void LogTimePerItem(string Key, int ItemCount, string AdditionalText = "", bool InfoLevel = false)
        {
            if (this.IsLogDisabled)
            {
                return;
            }

            this.LogTimePerItem(Key, ItemCount, false, AdditionalText, InfoLevel);

        }

        public void LogTimePerItemAndPause(string Key, int ItemCount, string AdditionalText = "", bool InfoLevel = false)
        {
            if (this.IsLogDisabled)
            {
                return;
            }

            this.LogTimePerItem(Key, ItemCount, true, AdditionalText, InfoLevel);

        }

        public void LogTimePerItemAndDispose(string Key, int ItemCount, string AdditionalText = "", bool InfoLevel = false)
        {
            if (this.IsLogDisabled)
            {
                return;
            }

            this.LogTimePerItem(Key, ItemCount, false, AdditionalText, InfoLevel);
            this.RemoveWatch(Key);

        }

        private void LogTimePerItem(string Key, int ItemCount, bool pause = false, string AdditionalText = "", bool InfoLevel = false)
        {
            if (this.CannotLog(InfoLevel))
            {
                return;
            }

            WatchData data = this.GetData(Key);

            if (pause)
            {
                data.Watch.Stop();
            }

            this.LogTimePerItem(data, ItemCount, AdditionalText, InfoLevel);

        }

        private void LogTimePerItem(WatchData Data, int ItemCount, string AdditionalText = "", bool InfoLevel = false)
        {
            string timeUnit = string.Empty;
            string timeText = this.GetTextTime(Data.Watch.Elapsed, out timeUnit);

            string averageUnit = string.Empty;
            string averageTime = string.Empty;

            if (ItemCount > 0)
            {
                averageTime = this.GetTextTime(new TimeSpan(Data.Watch.Elapsed.Ticks / ItemCount), out averageUnit);
            }

            string message = string.Format(this.ActionCompletedForItemsFormat, Data.Key, timeText, timeUnit, ItemCount, averageTime, averageUnit, AdditionalText);

            this.LogData(Data, message, InfoLevel);

        }

        private void LogTimeWithLaps(WatchData Data, string AdditionalText = "", bool InfoLevel = false)
        {
            string timeUnit = string.Empty;
            string timeText = this.GetTextTime(Data.Watch.Elapsed, out timeUnit);

            string averageUnit = string.Empty;
            string averageTime = string.Empty;

            if (Data.NumberOfLaps > 0)
            {
                averageTime = this.GetTextTime(new TimeSpan(Data.Watch.Elapsed.Ticks / Data.NumberOfLaps), out averageUnit);
            }

            string message = string.Format(this.ActionCompletedForLapsFormat, Data.Key, timeText, timeUnit, Data.NumberOfLaps, averageTime, averageUnit, AdditionalText);

            this.LogData(Data, message, InfoLevel);

        }

        #region IDisposable Support
        private bool disposedValue = false;     

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    List<string> Keys = this.StopWatches.Keys.Reverse().ToList();

                    foreach (string key in Keys)
                    {
                        this.LogTimeAndDispose(key, "Clear all Timers", true);
                    }
                }

                disposedValue = true;
            }
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
        }
        #endregion


    }
}