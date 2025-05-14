using UnityEngine;

public class LibResourceLoader
{
    /// <summary>
    /// 使う際に加工する可能性が高いため毎回リソースローダーから引っ張る
    /// </summary>
    public static GameObject _emptyPref { get { return Resources.Load(@"Prefabs\Empty") as GameObject; } }

    public static GameObject _bombFxPref;
    public static GameObject _debugToolPref;
    public static GameObject _uiEventSystem;

    /// <summary>
    /// ゲーム起動時に呼び出しかかる
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize()
    {
        _bombFxPref = Resources.Load("BombFxPref") as GameObject;
        _debugToolPref = Resources.Load("DebugToolCanvas") as GameObject;
        _uiEventSystem = Resources.Load("UI_EventSystem") as GameObject;
    }
}