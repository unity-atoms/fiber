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
        public TypographyTokens Typography;
        public SpacingTokens Spacing;

        public Theme(
            string fallbackRole,
            ColorTokenCollection color,
            TypographyTokens typography,
            int spacingBaseline = 4
        )
        {
            FallbackRole = fallbackRole;
            Color = color;
            Color.RegisterDependentSignal(this);
            Typography = typography;
            Typography.RegisterDependentSignal(this);
            Spacing = new SpacingTokens(spacingBaseline);
            Spacing.RegisterDependentSignal(this);
        }

        public override Theme Get() => this;
        public override bool IsDirty(byte otherDirtyBit)
        {
            return DirtyBit != otherDirtyBit;
        }

        public ColorModifiers GetColorModifiers(string role, ElementType elementType, string variant = null)
        {
            role = Color.ContainsKey(role) ? role : FallbackRole;
            var element = Color[role].GetElement(elementType);
            var colorModifiers = variant != null && element.Variants.ContainsKey(variant) ? element.Variants[variant] : element;
            return colorModifiers;
        }
    }
}