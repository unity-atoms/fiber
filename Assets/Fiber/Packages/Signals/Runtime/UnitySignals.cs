using UnityEngine;
using FiberUtils;

namespace Signals
{
    public struct ScreenSize
    {
        public int PixelWidth;
        public int PixelHeight;
        public float DPI;
        public float DPWidth;
        public float DPHeight;

        public ScreenSize(int pixelWidth, int pixelHeight, float dpi, float dpWidth, float dpHeight)
        {
            PixelWidth = pixelWidth;
            PixelHeight = pixelHeight;
            DPI = dpi;
            DPWidth = dpWidth;
            DPHeight = dpHeight;
        }
    }

    public class ScreenSizeSignal : BaseSignal<ScreenSize>
    {
        private const float REFERENCE_DPI = 160f;
        private const float DEFAULT_FALLBACK_DPI = 160f;
        private ScreenSize _value;
        public ScreenSize Value => _value;
        private int _updateLoopSubId = -1;

        public ScreenSizeSignal()
        {
            _value = new ScreenSize();
            _updateLoopSubId = MonoBehaviourHelper.AddOnUpdateHandler(Update);
        }
        ~ScreenSizeSignal()
        {
            if (_updateLoopSubId != -1)
            {
                MonoBehaviourHelper.RemoveOnUpdateHandler(_updateLoopSubId);
            }
            _updateLoopSubId = -1;
        }

        // Retrieve density per inch
        float GetDPI()
        {
            var dpi = Screen.dpi;
            var platform = Application.platform;

            switch (platform)
            {
                case RuntimePlatform.WebGLPlayer:
                    return REFERENCE_DPI; // In web builds Screen.width and Screen.height are in CSS pixels, which are density-independent
                default:
                    {
                        if (dpi > 25f && dpi < 1000f)
                        {
                            return dpi;
                        }

                        // TODO: Use Application.platform (https://docs.unity3d.com/ScriptReference/RuntimePlatform.html) 
                        // For better defaults per platform

                        return DEFAULT_FALLBACK_DPI;
                    }
            }
        }

        void Update(float deltaTime)
        {
            var pixelWidth = Screen.width;
            var pixelHeight = Screen.height;
            var dpi = GetDPI();

            if (pixelWidth != _value.PixelWidth || pixelHeight != _value.PixelHeight || dpi != _value.DPI)
            {
                var (dpWidth, dpHeight) = CalcDpScreenSize(pixelWidth, pixelHeight, dpi);
                _value = new ScreenSize(pixelWidth, pixelHeight, dpi, dpWidth, dpHeight);
                NotifySignalUpdate();
            }
        }

        (float, float) CalcDpScreenSize(int pixelWidth, int pixelHeight, float dpi)
        {
            // Density-independent pixels (based of Android docs - https://developer.android.com/guide/topics/resources/more-resources.html#Dimension)
            // Baseline: 1dp = 1px at 160dpi
            // Calculate dp screen size: (160 / dpi) * px size = dp size
            var dpWidth = REFERENCE_DPI / dpi * pixelWidth;
            var dpHeight = REFERENCE_DPI / dpi * pixelHeight;

            return (dpWidth, dpHeight);
        }

        protected override sealed void OnNotifySignalUpdate() { _dirtyBit++; }
        public override sealed ScreenSize Get() => _value;
        public override sealed bool IsDirty(byte otherDirtyBit) => DirtyBit != otherDirtyBit;
    }

}