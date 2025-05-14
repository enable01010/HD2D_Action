using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.PackageManager.UI;
using Unity.VisualScripting;

public class LibDebug:Singleton<LibDebug>
{
     private DebugEditor _window = null;
     private DebugEditor window
     { 
        get
        {
            if (_window == null)
                _window = (DebugEditor)EditorWindow.GetWindow(typeof(DebugEditor), false, null, false);
            _window.SheetSetting();//êÊÇ…åƒÇ—èoÇ≥Ç»Ç¢Ç∆ÉoÉOÇ™î≠ê∂Ç∑ÇÈÇΩÇﬂ
            return _window;
        }
     }

    static public void ButtonLog(object obj, Action action, DebugUser user,bool isStopAndView = false)
    {
#if UNITY_EDITOR
        //window.AddButtonLog(obj, action);
        instance.window.Button(obj,action,user);
        if (isStopAndView == true) StopAndView();
#endif
    }

    static public void LogIf(object obj, DebugUser user, Func<bool> whenUse, bool isStopAndView = false)
    {
#if UNITY_EDITOR
        if (whenUse.Invoke() == false) return;

        instance.window.Log(obj, user);

        if (isStopAndView == true) StopAndView();
#endif
    }

    static public void LogIf(object obj, DebugUser user, bool whenUse, bool isStopAndView = false)
    {
#if UNITY_EDITOR
        if (whenUse == false) return;

        instance.window.Log(obj, user);

        if (isStopAndView == true) StopAndView();
#endif
    }

    static public void Log(object obj, DebugUser user,bool isStopAndView = false)
    {
#if UNITY_EDITOR
        instance.window.Log(obj, user);

        if (isStopAndView == true) StopAndView();
#endif
    }

    static private void StopAndView()
    {
#if UNITY_EDITOR
        EditorWindow.GetWindow(typeof(DebugEditor));
        UnityEditor.EditorApplication.isPaused = true;
#endif
    }

}

public enum DebugUser
{
    All,
    Taniyama,
    Matuoka,
}
