using System.Diagnostics;

namespace FiberUtils
{
    public class TimeBudgetManager
    {
        private static TimeBudgetManager _instance = new();
        public static TimeBudgetManager Instance { get => _instance; private set { _instance = value; } }

        private readonly Stopwatch _stopWatch;
        private int _lateUpdateSubId = -1;

        public const long DEFAULT_TIME_BUDGET_MS = 4;
        private long _timeBudgetMs;

        TimeBudgetManager(
            long timeBudgetMs = DEFAULT_TIME_BUDGET_MS
        )
        {
            _timeBudgetMs = timeBudgetMs;
            _stopWatch = new Stopwatch();

            _lateUpdateSubId = MonoBehaviourHelper.AddOnLateUpdateHandler(LateUpdate);
        }

        ~TimeBudgetManager()
        {
            MonoBehaviourHelper.RemoveOnLateUpdateHandler(_lateUpdateSubId);
        }

        private void LateUpdate(float deltaTime)
        {
            _stopWatch.Reset();
        }

        public void SetTimeBudget(long timeBudgetMs)
        {
            _timeBudgetMs = timeBudgetMs;
        }

        public void StartTimer()
        {
            _stopWatch.Start();
        }

        public void StopTimer()
        {
            _stopWatch.Stop();
        }

        public bool HasBudgetLeft(float percentage = 1.0f)
        {
            return _stopWatch.ElapsedMilliseconds < _timeBudgetMs * percentage;
        }

        public bool StartTimerIfNotAlreadytRunning()
        {
            if (!_stopWatch.IsRunning)
            {
                _stopWatch.Start();
                return true;
            }
            return false;
        }

        public void StopTimerIf(bool condition)
        {
            if (condition)
            {
                _stopWatch.Stop();
            }
        }
    }
}