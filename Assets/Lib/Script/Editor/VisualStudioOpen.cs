using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;


[InitializeOnLoad]
public class VisualStudioOpen
{
    private const string startupKey = "StartedUpKey";

    static VisualStudioOpen()
    {


        EditorApplication.delayCall += Open;
    }
    
    private static void Open()
    {
        //èââÒãNìÆÇÃÇ›é¿é{
        if (SessionState.GetBool(startupKey, false)) return;
        SessionState.SetBool(startupKey, true);

        string projectPath = Application.dataPath;
        string projectDirectoryName = new DirectoryInfo(projectPath).Parent.Name;
        Process.Start(projectDirectoryName + ".sln");
        SessionState.SetBool(startupKey, true);

        EditorApplication.delayCall -= Open;
    }
}
