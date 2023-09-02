using Signals;

namespace Fiber.Theme
{
    public class SpacingTokens : BaseSignal<SpacingTokens>
    {
        public Signal<int> Baseline;

        public SpacingTokens(int baseline = 4)
        {
            Baseline = new(baseline);
            Baseline.RegisterDependent(this);
        }

        public override SpacingTokens Get() => this;
        public override bool IsDirty(byte otherDirtyBit)
        {
            return DirtyBit != otherDirtyBit;
        }

        protected override sealed void OnNotifySignalUpdate()
        {
            _dirtyBit++;
        }
    }
}