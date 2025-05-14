using UnityEngine;
using UnityEditor;

using System;
using System.IO;
using System.Collections;

public class MyWindow : EditorWindow
{
    //初期設定用のフォルダーを生成する
    [MenuItem("カスタムエディター/CreateFolders",priority = 1)]
    public static void CreateFolder()
    {
        Debug.Log("CreateFoldersの処理を実行します");

        //作成するフォルダーのパスを確認
        Debug.Log(Application.dataPath);
        string folderPath = Application.dataPath;

        //作成するフォルダーのパスを生成
        folderPath = Path.Combine(folderPath, @"MyProgram");

        //フォルダーがすでに生成されているか確認
        bool IsCreated = Directory.Exists(folderPath);
        if(IsCreated == true)
        {
            Debug.Log(folderPath + "はすでに作成されています");
        }
        else
        {
            Debug.Log("初期設定のフォルダーを生成します");
            Directory.CreateDirectory(folderPath);
            Directory.CreateDirectory(Path.Combine(folderPath, @"Prefab"));
            Directory.CreateDirectory(Path.Combine(folderPath, @"Script"));
            Directory.CreateDirectory(Path.Combine(folderPath, @"Image"));
            Directory.CreateDirectory(Path.Combine(folderPath, @"Music"));
            Directory.CreateDirectory(Path.Combine(folderPath, @"Animation"));
            Directory.CreateDirectory(Path.Combine(folderPath, @"Material"));
            Directory.CreateDirectory(Path.Combine(folderPath, @"TileMap"));

            if(Directory.Exists(folderPath))
            {
                Debug.Log("フォルダーの生成に成功しました");
            }

            AssetDatabase.Refresh();
        }
    }

    private void OnGUI()
    {
        
    }

}
