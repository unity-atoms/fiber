using Signals;

namespace SilkUI
{
    public class SpacingTokens : BaseSignal<SpacingTokens>
    {
        public Signal<int> Baseline { get; private set; }

        public SpacingTokens(int baseline = 4)
        {
            Baseline = new(baseline);
            Baseline.RegisterDependent(this);
        }
        ~SpacingTokens()
        {
            Baseline.UnregisterDependent(this);
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