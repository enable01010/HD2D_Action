using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ScriptOpener : AssetPostprocessor
{
    static void OnPostprocessAllAssets(
         string[] importedAssets,
         string[] deletedAssets,
         string[] movedAssets,
         string[] movedFromAssetPaths)
    {
        if (importedAssets.Length == 1 && importedAssets[0].EndsWith(".cs"))
        {
            OpenScriptInVisualStudio(importedAssets[0]);
        }
    }

    // Visual Studioでスクリプトを開く処理
    private static void OpenScriptInVisualStudio(string scriptPath)
    {
        //初回起動のみ実施
        if (EditorPrefs.GetBool(scriptPath, false)) return;
        EditorPrefs.SetBool(scriptPath, true);

        // Unityのプロジェクトフォルダからソリューションファイルのパスを生成
        string projectPath = Application.dataPath;
        string projectDirectoryName = new DirectoryInfo(projectPath).Parent.Name;
        string solutionPath = projectDirectoryName + ".sln";


        // Unityの設定からVisual Studioのパスを取得
        string externalScriptEditor = EditorPrefs.GetString("kScriptsDefaultApp");

        if (!string.IsNullOrEmpty(externalScriptEditor) && File.Exists(solutionPath))
        {
            // 引数としてスクリプトファイルのみを指定
            string arguments = IsVisualStudioRunning() ? $"/Edit \"{scriptPath}\"" : $"\"{solutionPath}\" \"{scriptPath}\"";

            // ProcessStartInfoを使ってVisual Studioを起動
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = externalScriptEditor,
                Arguments = arguments,
                UseShellExecute = true,
                CreateNoWindow = false
            };

            // Visual Studioを起動
            Process.Start(startInfo);
        }
    }

    private static bool IsVisualStudioRunning()
    {
        // Visual Studioのプロセス名を取得
        string[] visualStudioProcessNames = { "devenv", "vs", "sln", "VisualStudio" };

        // 現在のプロセスを取得
        Process[] processes = Process.GetProcesses();

        // 各プロセスを確認
        int i = 0;
        while (i < processes.Length)
        {
            try
            {
                Process process = processes[i];
                foreach (string processName in visualStudioProcessNames)
                {
                    if (process.ProcessName.Equals(processName, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            catch (Exception e)
            {

            }
            i++;
        }

        return false;
    }
}
