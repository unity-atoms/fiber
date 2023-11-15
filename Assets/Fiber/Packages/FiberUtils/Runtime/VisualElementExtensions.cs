using UnityEngine.UIElements;

public static class VisualElementExtensions
{
    public static void MoveToIndex(this VisualElement element, int index)
    {
        if (index < 0 || index >= element.parent.childCount)
        {
            throw new System.ArgumentOutOfRangeException(nameof(index));
        }


        var currentIndex = element.parent.IndexOf(element);
        if (currentIndex > index)
        {
            var currentElementAtIndex = element.parent.ElementAt(index);
            element.PlaceBehind(currentElementAtIndex);
        }
        else if (currentIndex < index)
        {
            var currentElementAtIndex = element.parent.ElementAt(index);
            element.PlaceInFront(currentElementAtIndex);
        }
    }

    public static void MoveToBack(this VisualElement element)
    {
        element.MoveToIndex(element.parent.childCount - 1);
    }

    public static void MoveToFront(this VisualElement element)
    {
        element.MoveToIndex(0);
    }
}