using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "SkeletonSO", menuName = "ScriptableObjects/SkeletonData", order = 1)]
public class SkeletonSO : ScriptableObject {
    public List<Limb> limbs = new List<Limb>();
}

public class Limb {
    Texture2D texture;
    string name;
    Vector4 coordinates;
    Vector2 pivot;

    public Texture2D GetTexture() { return texture; }
    public void SetTexture(Texture2D _Texture) { texture = _Texture; }
    public string GetName() { return name; }
    public void SetName(string Name) { name = Name; }
    /* 0: X; Where the sprite box starts;
     * 1: Y; Where the sprite box ends;
     * 2: Width; Size of the texture;
     * 3: Height; Size of the texture; */
    public Vector4 GetCoordinates() { return coordinates; }
    public void SetCoordinates(Vector4 Coordinates) { coordinates = Coordinates; }
    public Vector2 GetPivot() { return pivot; }
    public void SetPivot(Vector2 Pivot) { pivot = Pivot; }
}

[CustomEditor(typeof(SkeletonSO))] // Replace with the name of your ScriptableObject class
public class SkeletonSOEditor : Editor{
    public override void OnInspectorGUI(){
        SkeletonSO scriptableObject = (SkeletonSO) target;
        DrawDefaultInspector();

        // Display a label for a property
        EditorGUILayout.LabelField("Skeleton Scriptable Object Data", EditorStyles.boldLabel);
        EditorGUILayout.LabelField(scriptableObject.limbs.Count + " Limbs found");
        EditorGUILayout.HelpBox("Textures must be in the 'Resources' folder at the project's root for the texture compiling script to function properly.", MessageType.Warning);
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Texture to search: ", EditorStyles.boldLabel);
        string textureToSearch = EditorGUILayout.TextField("SkeletonData");
        EditorGUILayout.EndHorizontal();

        //Confirm search and load button;
        if (GUILayout.Button("Load Limb Data From Texture")){
            CrAPTextureManagement.LoadSkeletonData(textureToSearch, scriptableObject);
        }

        EditorGUILayout.Space();

        int boxHeight = 20;

        try {
            foreach(Limb limb in scriptableObject.limbs) {
                string name = limb.GetName();

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.LabelField(name, EditorStyles.boldLabel);

                //Coordinates;
                Vector4 coordinates = limb.GetCoordinates();
                //Sprite boxes;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Sprite Box Start", GUILayout.Width(100));
                EditorGUILayout.SelectableLabel(coordinates.x.ToString(), EditorStyles.textField, GUILayout.Height(boxHeight));

                EditorGUILayout.LabelField("Sprite Box End", GUILayout.Width(100));
                EditorGUILayout.SelectableLabel(coordinates.y.ToString(), EditorStyles.textField, GUILayout.Height(boxHeight));
                EditorGUILayout.EndHorizontal();

                //Width / Height;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Width", GUILayout.Width(100));
                EditorGUILayout.SelectableLabel(coordinates.z.ToString(), EditorStyles.textField, GUILayout.Height(boxHeight));

                EditorGUILayout.LabelField("Height", GUILayout.Width(100));
                EditorGUILayout.SelectableLabel(coordinates.w.ToString(), EditorStyles.textField, GUILayout.Height(boxHeight));
                EditorGUILayout.EndHorizontal();

                //Coordinates;
                Vector2 pivot = limb.GetPivot();
                //Pivot;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Pivot X", GUILayout.Width(100));
                EditorGUILayout.SelectableLabel(pivot.x.ToString(), EditorStyles.textField, GUILayout.Height(boxHeight));

                EditorGUILayout.LabelField("Pivot Y", GUILayout.Width(100));
                EditorGUILayout.SelectableLabel(pivot.y.ToString(), EditorStyles.textField, GUILayout.Height(boxHeight));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();

                // Display the label and read-only texture field
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Texture");
                Rect objectFieldRect = GUILayoutUtility.GetRect(EditorGUIUtility.fieldWidth * 2, EditorGUIUtility.fieldWidth * 2);

                EditorGUI.BeginDisabledGroup(true);
                EditorGUI.ObjectField(objectFieldRect, GUIContent.none, limb.GetTexture(), typeof(Texture2D), false);
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }
        } catch { }
    }
}