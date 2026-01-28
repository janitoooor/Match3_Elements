using System.Globalization;
using UnityEditor;
using UnityEngine;

namespace Base
{
	[CustomPropertyDrawer(typeof(ArrayElementTitleAttribute))]
	public sealed class ArrayElementTitleDrawer : PropertyDrawer
	{
		private SerializedProperty titleNameProp;
     
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) 
			=> EditorGUI.GetPropertyHeight(property, label, true);
        
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (attribute is ArrayElementTitleAttribute title)
				EditTittle(position, property, label, title);
		}

		private void EditTittle(Rect position, SerializedProperty property, GUIContent label, 
			ArrayElementTitleAttribute title)
		{
			var fullPathName = property.propertyPath + "." + title.varName;
			titleNameProp = property.serializedObject.FindProperty(fullPathName);
			
			if (titleNameProp == null)
			{
				fullPathName = property.propertyPath + ".<" + title.varName + ">k__BackingField";
				titleNameProp = property.serializedObject.FindProperty(fullPathName);
			}
                
			var newLabel = GetTitle();
			
			if (string.IsNullOrEmpty(newLabel))
				newLabel = label.text;
				
			EditorGUI.PropertyField(position, property, new GUIContent(newLabel, label.tooltip), true);
		}

		private string GetTitle()
		{
			if (titleNameProp == null)
				return null;
            
			switch (titleNameProp.propertyType)
			{
				case SerializedPropertyType.Integer:
					return titleNameProp.intValue.ToString();
				case SerializedPropertyType.Boolean:
					return titleNameProp.boolValue.ToString();
				case SerializedPropertyType.Float:
					return titleNameProp.floatValue.ToString(CultureInfo.InvariantCulture);
				case SerializedPropertyType.String:
					return titleNameProp.stringValue;
				case SerializedPropertyType.Color:
					return titleNameProp.colorValue.ToString();
				case SerializedPropertyType.ObjectReference:
					return titleNameProp.objectReferenceValue.ToString();
				case SerializedPropertyType.Enum:
					return titleNameProp.enumNames[titleNameProp.enumValueIndex];
				case SerializedPropertyType.Vector2:
					return titleNameProp.vector2Value.ToString();
				case SerializedPropertyType.Vector3:
					return titleNameProp.vector3Value.ToString();
				case SerializedPropertyType.Vector4:
					return titleNameProp.vector4Value.ToString();
			}
			
			return string.Empty;
		}
	}
}