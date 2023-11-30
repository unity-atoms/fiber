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

        public static int GetParentCount(this Transform transform)
        {
            var count = 0;
            var current = transform.parent;
            while (current != null)
            {
                count++;
                current = current.parent;
            }
            return count;
        }
    }
}