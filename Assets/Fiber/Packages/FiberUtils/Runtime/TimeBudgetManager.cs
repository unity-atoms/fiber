using System.Diagnostics;

namespace FiberUtils
{
    public class TimeBudgetManager
    {
        private static TimeBudgetManager _instance = new();
        public static TimeBudgetManager Instance { get => _instance; private set { _instance = value; } }

        private readonly Stopwatch _stopWatch;
        private int _workLoopSubId = -1;

        public const long DEFAULT_TIME_BUDGET_MS = 4;
        private long _timeBudgetMs;

        public TimeBudgetManager(
            long timeBudgetMs = DEFAULT_TIME_BUDGET_MS
        )
        {
            _timeBudgetMs = timeBudgetMs;
            _stopWatch = new Stopwatch();

            _workLoopSubId = MonoBehaviourHelper.AddOnLateUpdateHandler(LateUpdate);
        }

        ~TimeBudgetManager()
        {
            MonoBehaviourHelper.RemoveOnLateUpdateHandler(_workLoopSubId);
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

        public bool HasBudgetLeft()
        {
            return _stopWatch.ElapsedMilliseconds < _timeBudgetMs;
        }
    }
}