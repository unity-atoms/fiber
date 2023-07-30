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
        public TokenCollection DesignTokens;
        // TODO: public ComponentTokens...

        public Theme(
            TokenCollection designTokens
        )
        {
            DesignTokens = designTokens;
            designTokens.RegisterParent(this);
        }

        public override Theme Get() => this;
        public override bool IsDirty(byte otherDirtyBit)
        {
            return DirtyBit != otherDirtyBit;
        }
    }
}