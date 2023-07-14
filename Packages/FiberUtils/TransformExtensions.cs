using UnityEngine;

namespace FiberUtils
{
    public static class TransformExtensions
    {
        public static bool IsAncestor(this Transform transform, Transform other)
        {
            if (transform == other) return true;
            if (other == null) return false;

            return IsAncestor(transform, other.parent);
        }
    }
}