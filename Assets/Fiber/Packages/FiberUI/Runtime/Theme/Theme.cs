using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fiber.UIElements;
using Fiber.DesignTokens;
using Signals;

namespace Fiber.UI
{
    public class Theme : BaseSignal<Theme>
    {
        public string FallbackRole;
        public ColorTokenCollection Color;
        public TypographyTokensCollection Typography;
        private readonly SpacingTokens _spacing;

        public Theme(
            string fallbackRole,
            ColorTokenCollection color,
            TypographyTokensCollection typography,
            int spacingBaseline = 4
        )
        {
            FallbackRole = fallbackRole;
            Color = color;
            color.RegisterParent(this);
            Typography = typography;
            _spacing = new SpacingTokens(spacingBaseline);
        }

        public override Theme Get() => this;
        public override bool IsDirty(byte otherDirtyBit)
        {
            return DirtyBit != otherDirtyBit;
        }

        public int Spacing(int multiplier)
        {
            return _spacing.Get(multiplier);
        }
    }
}