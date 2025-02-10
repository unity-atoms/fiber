using System.Collections.Generic;
using UnityEngine;
using Signals;
using FiberUtils;

namespace Fiber.UIElements
{
    public static partial class BaseComponentExtensions
    {
        public static ScalingProviderComponent ScalingProvider(
            this BaseComponent component,
            VirtualBody children = default,
            float referenceDPI = ScalingProviderComponent.DEFAULT_REFERENCE_DPI,
            float fallbackDPI = ScalingProviderComponent.DEFAULT_FALLBACK_DPI,
            float multiplier = 1f
        )
        {
            return new ScalingProviderComponent(
                children: children,
                referenceDPI: referenceDPI,
                fallbackDPI: fallbackDPI,
                multiplier: multiplier
            );
        }
    }

    public struct ScreenSize
    {
        // Pixel width and height is not the same as CSS / USS pixel size.
        // It's the actual pixel size of the screen, without compensating for density.
        public int PixelWidth;
        public int PixelHeight;
        public float DPI;
        // Density-independent pixels (dp) will be the same as CSS / USS pixel size.
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
        private readonly float _referenceDPI;
        private readonly float _fallbackDPI;
        private ScreenSize _value;
        public ScreenSize Value => _value;
        private int _updateLoopSubId = -1;

        public ScreenSizeSignal(float referenceDPI, float fallbackDPI)
        {
            _referenceDPI = referenceDPI;
            _fallbackDPI = fallbackDPI;
            UpdateScreenSize();
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

        // Retrieve density per inch. Sometimes also referred to as ppi (pixels per inch).
        float GetDPI()
        {
            var dpi = Screen.dpi;
            var platform = Application.platform;

            switch (platform)
            {
                default:
                    {
                        if (dpi > 25f && dpi < 1000f)
                        {
                            return dpi;
                        }

                        // TODO: Use Application.platform (https://docs.unity3d.com/ScriptReference/RuntimePlatform.html) 
                        // For better defaults per platform

                        return _fallbackDPI;
                    }
            }
        }

        void Update(float deltaTime)
        {
            UpdateScreenSize();
        }

        void UpdateScreenSize()
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
            // Calculate dp screen size: (referenceDPI / actualDPI) * px size = dp size
            var dpWidth = _referenceDPI / dpi * pixelWidth;
            var dpHeight = _referenceDPI / dpi * pixelHeight;

            return (dpWidth, dpHeight);
        }

        public override sealed ScreenSize Get() => _value;
    }

    public class ScalingConfig
    {
        public float ReferenceDPI { get; private set; }
        public float FallbackDPI { get; private set; }
        public float Multiplier { get; private set; }

        public ScalingConfig(float referenceDPI, float fallbackDPI, float multiplier)
        {
            ReferenceDPI = referenceDPI;
            FallbackDPI = fallbackDPI;
            Multiplier = multiplier;
        }
    }

    public class ScalingContext
    {
        public ScreenSizeSignal ScreenSizeSignal { get; private set; }
        private ScalingConfig _config;
        public float ReferenceDPI => _config.ReferenceDPI;
        public float FallbackDPI => _config.FallbackDPI;

        public ScalingContext(ScreenSizeSignal screenSizeSignal, ScalingConfig config)
        {
            ScreenSizeSignal = screenSizeSignal;
            _config = config;
        }
    }

    // A scaling provider for UI Toolkit for UI:s using scale mode "constant physical size"
    public class ScalingProviderComponent : BaseComponent
    {
        public const float DEFAULT_REFERENCE_DPI = 96f;
        public const float DEFAULT_FALLBACK_DPI = 96f;
        private float _referenceDPI;
        private float _fallbackDPI;
        private float _multiplier;
        public float ReferenceDPI => _referenceDPI;
        public float Multiplier => _multiplier;
        public ScreenSizeSignal ScreenSizeSignal { get; private set; }

        public ScalingProviderComponent(
            VirtualBody children,
            float referenceDPI = DEFAULT_REFERENCE_DPI,
            float fallbackDPI = DEFAULT_FALLBACK_DPI,
            float multiplier = 1f
        ) : base(children)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            // From testing it seems like Screen.dpi in WebGL returns 96f * window.devicePixelRatio. 
            // By hard coding reference dpi to 96f, 1px in Unity Toolkit will be 1px in CSS pixels
            _referenceDPI = 96f;
#else
            _referenceDPI = referenceDPI;
#endif
            _fallbackDPI = fallbackDPI;
            _multiplier = multiplier;
        }
        public override VirtualBody Render()
        {
            var scalingConfig = G<ScalingConfig>(throwIfNotFound: false) ?? new ScalingConfig(referenceDPI: _referenceDPI, fallbackDPI: _fallbackDPI, multiplier: _multiplier);
            ScreenSizeSignal = G<ScreenSizeSignal>(throwIfNotFound: false) ?? new ScreenSizeSignal(referenceDPI: scalingConfig.ReferenceDPI, fallbackDPI: scalingConfig.FallbackDPI);
            var context = new ScalingContext(screenSizeSignal: ScreenSizeSignal, config: scalingConfig);

            return F.ContextProvider(
                value: context,
                children: Children
            );
        }
    }
}