using UnityEditor;
using UnityEngine;

namespace Signals.Editor
{
    [CustomPropertyDrawer(typeof(Signal<bool>))]
    [CustomPropertyDrawer(typeof(Signal<int>))]
    [CustomPropertyDrawer(typeof(Signal<float>))]
    [CustomPropertyDrawer(typeof(Signal<string>))]
    public class SignalDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginChangeCheck();
            var valueProp = property.FindPropertyRelative("_value");
            EditorGUI.PropertyField(position, valueProp, label, true);
            if (EditorGUI.EndChangeCheck())
            {
                var dirtyBitProp = property.FindPropertyRelative("_dirtyBit");
                dirtyBitProp.intValue += 1;
                property.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
