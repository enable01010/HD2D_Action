using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;

public class PickUpFolder : EditorWindow
{
    private List<DefaultAsset> folder;
    private Vector2 scrollPos = Vector2.zero;
    private DefaultAsset defaultAsset;

    [MenuItem("カスタムエディター/PickUpFolder",false,4)]
    public static void FolderSwitcherOpen()
    {
        GetWindow<PickUpFolder>("PickUpFolder");
    }

    private void OnEnable()
    {
        if (folder == null)
        {
            folder = new List<DefaultAsset>();
            Load();
        }
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();

        var folderAsset = EditorGUILayout.ObjectField(null, typeof(DefaultAsset), false) as DefaultAsset;
        if (folderAsset != null && folder.IndexOf(folderAsset) < 0)
        {
            folder.Add(folderAsset);
            Save();
        }

        EditorGUILayout.EndHorizontal();

        GuiLine();

        this.scrollPos = EditorGUILayout.BeginScrollView(this.scrollPos);
        for (var i = 0; i < folder.Count; ++i)
        {
            var scene = folder[i];
            EditorGUILayout.BeginHorizontal();
            var path = AssetDatabase.GetAssetPath(scene);

            //削除のボタン
            if (GUILayout.Button("Delete", GUILayout.Width(50)))
            {
                folder.Remove(scene);
                Save();
                --i;
            }
            else
            {

                if (GUILayout.Button(Path.GetFileNameWithoutExtension(path), GUILayout.Width(150)))
                {
                    //そのシーンの位置をProject上で表示させる
                    EditorGUIUtility.PingObject(scene);
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();
    }

    private void Save()
    {
        var guids = new List<string>();
        foreach (var folderAsset in folder)
        {
            string guid;
            long localId;
            if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(folderAsset, out guid, out localId))
            {
                guids.Add(guid);
            }
        }

        var content = string.Join("\n", guids.ToArray());
        File.WriteAllText(FilePath, content);
    }

    private static string FilePath => $"{Application.persistentDataPath}/_PichUpFolder.sav";

    private void Load()
    {
        folder.Clear();
        if (File.Exists(FilePath))
        {
            string content = File.ReadAllText(FilePath);
            foreach (var guid in content.Split(new char[] { '\n' }))
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var scene = AssetDatabase.LoadAssetAtPath<DefaultAsset>(path);
                if (scene != null)
                    folder.Add(scene);
            }
        }
    }

    private void GuiLine(int height = 1)
    {
        GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(height) });
    }
}

#endif