using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

public class PickUpScene : EditorWindow
{
    private List<SceneAsset> scenes;
    private Vector2 scrollPos = Vector2.zero;

    [MenuItem("カスタムエディター/Pick Up Scene",false,2)]
    static void SceneSwitcherOpen()
    {
        //選択されたらウインドウを開く
        GetWindow<PickUpScene>("PickUpScene");
    }

    private void OnEnable()
    {
        if (scenes == null)
        {
            scenes = new List<SceneAsset>();
            Load();
        }
    }

    private static string FilePath => $"{Application.persistentDataPath}/_sceneLauncher.sav";

    private void OnGUI()
    {
        //一行開始

        EditorGUILayout.BeginHorizontal();

        //シーン用の検索窓の設置
        var sceneAsset = EditorGUILayout.ObjectField(null, typeof(SceneAsset), false) as SceneAsset;
        if (sceneAsset != null && scenes.IndexOf(sceneAsset) < 0)
        {
            scenes.Add(sceneAsset);
            Save();
        }

        //リストを増やす用のボタン
        if (GUILayout.Button("Add current scene"))
        {
            //現在のシーンの取得
            var scene = EditorSceneManager.GetActiveScene();

            //現在のシーンがすでにリストに入っていない場合の処理
            if (scene != null && scene.path != null &&
                scenes.Find(s => AssetDatabase.GetAssetPath(s) == scene.path) == null)
            {
                var asset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.path);
                if (asset != null && scenes.IndexOf(asset) < 0)
                {
                    //リストに表示する処理
                    scenes.Add(asset);
                    Save();
                }
            }
        }
        EditorGUILayout.EndHorizontal();
        //一行終了

        GuiLine();

        //リストの中身分表示
        this.scrollPos = EditorGUILayout.BeginScrollView(this.scrollPos);
        for (var i = 0; i < scenes.Count; ++i)
        {
            var scene = scenes[i];
            EditorGUILayout.BeginHorizontal();
            var path = AssetDatabase.GetAssetPath(scene);

            //削除のボタン
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                scenes.Remove(scene);
                Save();
                --i;
            }
            else
            {
                
                if (GUILayout.Button("O", GUILayout.Width(20)))
                {
                    //そのシーンの位置をProject上で表示させる
                    EditorGUIUtility.PingObject(scene);
                }

                if (GUILayout.Button(i > 0 ? "↑" : "　", GUILayout.Width(20)) && i > 0)
                {
                    //シーンの場所を入れ替える
                    scenes[i] = scenes[i - 1];
                    scenes[i - 1] = scene;
                    Save();
                }

                EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying);
                if (GUILayout.Button(Path.GetFileNameWithoutExtension(path)))
                {
                    //セーブされているか確認
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        //シーンの変更
                        EditorSceneManager.OpenScene(path);
                    }
                }
                EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();
    }

    private void Save()
    {
        var guids = new List<string>();
        foreach (var scene in scenes)
        {
            string guid;
            long localId;
            if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(scene, out guid, out localId))
            {
                guids.Add(guid);
            }
        }

        var content = string.Join("\n", guids.ToArray());
        File.WriteAllText(FilePath, content);
    }

    private void Load()
    {
        scenes.Clear();
        if (File.Exists(FilePath))
        {
            string content = File.ReadAllText(FilePath);
            foreach (var guid in content.Split(new char[] { '\n' }))
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
                if (scene != null)
                    scenes.Add(scene);
            }
        }
    }

    private void GuiLine(int height = 1)
    {
        GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(height) });
    }
}