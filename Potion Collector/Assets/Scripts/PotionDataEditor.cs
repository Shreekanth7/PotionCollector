// Scripts/Editor/PotionDataEditor.cs
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(PotionData))]
public class PotionDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PotionData potionData = (PotionData)target;

        EditorGUILayout.LabelField("Potion Settings", EditorStyles.boldLabel);

        potionData.potionName = EditorGUILayout.TextField("Name", potionData.potionName);
        potionData.potency = (int)EditorGUILayout.Slider("Potency", potionData.potency, 1f, 100f);
        potionData.potionTime = (int)EditorGUILayout.Slider("Potion Time", potionData.potionTime, 1f, 10f);
        EditorGUILayout.LabelField("Description");
        potionData.description = EditorGUILayout.TextArea(potionData.description, GUILayout.MinHeight(60));

        EditorGUILayout.Space();
        potionData.prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", potionData.prefab, typeof(GameObject), false);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(potionData);
        }
    }
}
#endif