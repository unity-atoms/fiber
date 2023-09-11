using Signals;

namespace SilkUI
{
    public enum IconSize
    {
        Tiny = 0,
        Small = 1,
        Medium = 2,
        Large = 3,
        XL = 4
    }

    public class IconTokens : BaseSignal<IconTokens>
    {
        public Signal<int> Tiny { get; private set; }
        public Signal<int> Small { get; private set; }
        public Signal<int> Medium { get; private set; }
        public Signal<int> Large { get; private set; }
        public Signal<int> XL { get; private set; }

        public IconTokens(
            int tiny = 2,
            int small = 3,
            int medium = 5,
            int large = 9,
            int xl = 14
        )
        {
            Tiny = new(tiny);
            Tiny.RegisterDependent(this);
            Small = new(small);
            Small.RegisterDependent(this);
            Medium = new(medium);
            Medium.RegisterDependent(this);
            Large = new(large);
            Large.RegisterDependent(this);
            XL = new(xl);
            XL.RegisterDependent(this);
        }

        ~IconTokens()
        {
            Tiny.UnregisterDependent(this);
            Small.UnregisterDependent(this);
            Medium.UnregisterDependent(this);
            Large.UnregisterDependent(this);
            XL.UnregisterDependent(this);
        }

        public Signal<int> GetIconSignal(IconSize size)
        {
            return size switch
            {
                IconSize.Tiny => Tiny,
                IconSize.Small => Small,
                IconSize.Medium => Medium,
                IconSize.Large => Large,
                IconSize.XL => XL,
                _ => throw new System.Exception("Invalid icon size")
            };
        }

        public override IconTokens Get() => this;
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