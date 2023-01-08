using UnityEngine;
using UnityEditor;

//public class ReadOnlyAttribute : PropertyAttribute
//{ 
//실제 사용할 어트리뷰트는 에디터 폴더에 있으면 안됨. 
//다른 스크립트 처럼 따로 만들어야함.

//}

[CustomPropertyDrawer(typeof(ReadOnly))]
public class ReadOnlyDrawer : PropertyDrawer
{
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return base.GetPropertyHeight(property, label);
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		GUI.enabled = false;
		//base.OnGUI(position, property, label);
		EditorGUI.PropertyField(position, property, label, true);
		GUI.enabled = true;
	}


}
