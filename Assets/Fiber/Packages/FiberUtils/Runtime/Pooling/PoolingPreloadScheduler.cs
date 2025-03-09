using System.Collections.Generic;

namespace FiberUtils
{
    public class PoolingPreloadScheduler
    {
        private struct PreloadScheduleDate
        {
            public IPreload Preloadable;
            public int Count;

            public PreloadScheduleDate(IPreload preloadable, int count)
            {
                Preloadable = preloadable;
                Count = count;
            }
        }

        private static PoolingPreloadScheduler _instance = new();
        public static PoolingPreloadScheduler Instance { get => _instance; private set { _instance = value; } }
        private static readonly float BUDGET_PERCENTAGE_THRESHOLD = 0.75f;

        private List<PreloadScheduleDate> _queuedPreloadScheduleData = new();
        private int _workLoopSubId = -1;

        PoolingPreloadScheduler()
        {
            _instance = this;

            MonoBehaviourHelper.AddOnUpdateHandler(Update);
        }

        ~PoolingPreloadScheduler()
        {
            MonoBehaviourHelper.RemoveOnUpdateHandler(_workLoopSubId);
        }

        private void Update(float deltaTime)
        {
            var preloadedCount = 0;
            for (var i = 0; i < _queuedPreloadScheduleData.Count; ++i)
            {
                var preloadScheduleData = _queuedPreloadScheduleData[i];

                if (TimeBudgetManager.Instance.HasBudgetLeft(percentage: BUDGET_PERCENTAGE_THRESHOLD))
                {
                    TimeBudgetManager.Instance.StartTimer();
                    preloadScheduleData.Preloadable.Preload(preloadScheduleData.Count);
                    preloadedCount++;
                    TimeBudgetManager.Instance.StopTimer();
                }
                else
                {
                    break;
                }
            }

            if (preloadedCount > 0)
            {
                _queuedPreloadScheduleData.RemoveRange(0, preloadedCount);
            }
        }

        public void SchedulePreload(IPreload preloadable, int count)
        {
            if (TimeBudgetManager.Instance.HasBudgetLeft(percentage: BUDGET_PERCENTAGE_THRESHOLD))
            {
                TimeBudgetManager.Instance.StartTimer();
                preloadable.Preload(count);
                TimeBudgetManager.Instance.StopTimer();
                return;
            }

            _queuedPreloadScheduleData.Add(new PreloadScheduleDate(preloadable, count));
        }
    }
}