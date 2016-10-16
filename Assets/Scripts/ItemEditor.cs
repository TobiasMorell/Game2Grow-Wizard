using UnityEngine;
using UnityEditor;
using ItemClasses;

/*[CustomPropertyDrawer(typeof(ItemClasses.Item))]
public class ItemEditor : PropertyDrawer {

	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty (position, label, property);

		position = EditorGUI.PrefixLabel (position, GUIUtility.GetControlID (FocusType.Passive), label);

		EditorGUI.

		AttributeType at = (AttributeType) property.FindPropertyRelative ("Attribute").enumValueIndex;
		switch( at ) {
		case AttributeType.Attributes:
			EditorGUI.PropertyField(position, property.FindPropertyRelative("Attributes"));
			break;
		case AttributeType.Schools:
			EditorGUI.PropertyField (position, property.FindPropertyRelative ("Magic"));
			break;
		}
	}
}*/
