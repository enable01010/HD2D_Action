using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.Callbacks;
using System.ComponentModel;
using System.IO;
using System.Diagnostics;
using System;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class SelectStartScene : IPreprocessBuildWithReport
{
	public int callbackOrder { get { return 0; } }
	public void OnPreprocessBuild(BuildReport report)
	{
		if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
		{
			EditorBuildSettingsScene[] scene = EditorBuildSettings.scenes;
			EditorSceneManager.OpenScene(scene[0].path);
		}
	}
}
#endif