using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MidiReader))]
public class MidiReaderEditor : Editor
{
    MidiReader midiReader;
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Load Midi File..."))
        {
            SelectNewFile();
        }
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            DrawDefaultInspector();
        }
    }
    public void SelectNewFile()
    {
        string path = EditorUtility.OpenFilePanel("Load new ", "Assets/", "mid");
        if (string.IsNullOrEmpty(path)) return;
        midiReader.file = path;
    }
    private void OnEnable()
    {
        midiReader = (MidiReader)target;
    }
}
