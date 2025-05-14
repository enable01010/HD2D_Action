using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibCoroutineRunner
{
    static LibCoroutineRunner _instnace;
    LibCoroutineRunnerModule obj;

    public static LibCoroutineRunner instance
    {
        get
        {
            _instnace = _instnace ?? new LibCoroutineRunner();
            return _instnace;
        }
    }

    private LibCoroutineRunner()
    {
        obj = GameObject.Instantiate((GameObject)Resources.Load("Prefabs/CoroutineRunner")).GetComponent<LibCoroutineRunnerModule>();
        GameObject.DontDestroyOnLoad(obj.gameObject);
    }

    /// <summary>
    /// コルーチンを開始する
    /// </summary>
    /// <param name="routine">コルーチンを</param>
    /// <param name="callback">コルーチン終了コールバック</param>
    public static void StartCoroutine(IEnumerator routine, Action callback = null)
    {
        instance.obj.StartCoroutine(routine, callback);
    }

    /// <summary>
    /// 稼働中のコルーチンを全て破棄する
    /// </summary>
    public static void Dispose()
    {
        instance.obj.Dispose();
    }

    /// <summary>
    /// 特定のコルーチン停止
    /// </summary>
    /// <param name="routine">コルーチン</param>
    public static void StopCoroutine(IEnumerator routine)
    {
        instance.obj.StopCoroutine(routine);
    }

    /// <summary>
    /// コルーチンが1件以上実行中か
    /// </summary>
    /// <returns>true: 実行中</returns>
    public static bool IsRunning()
    {
        return instance.obj.IsRunning();
    }

    /// <summary>
    /// 一時停止
    /// </summary>
    public static void Pause()
    {
        instance.obj.Pause();
    }

    /// <summary>
    /// 再開
    /// </summary>
    public static void Resume()
    {
        instance.obj.Resume();
    }
}

public class LibCoroutineRunnerModule : MonoBehaviour
{
    private Dictionary<IEnumerator, Action> m_callbacks = new Dictionary<IEnumerator, Action>();
    private List<IEnumerator> m_routines = new List<IEnumerator>();
    private bool m_isPaused = false;

    /// <summary>
    /// コルーチンを開始する
    /// </summary>
    /// <param name="routine">コルーチンを</param>
    /// <param name="callback">コルーチン終了コールバック</param>
    public void StartCoroutine(IEnumerator routine, Action callback = null)
    {
        if (callback != null)
        {
            m_callbacks.Add(routine, callback);
        }
        m_routines.Add(routine);
        routine.MoveNext(); // 1フレーム遅延しないように即実行
    }

    /// <summary>
    /// 稼働中のコルーチンを全て破棄する
    /// </summary>
    public void Dispose()
    {
        m_routines.Clear();
    }

    /// <summary>
    /// 特定のコルーチン停止
    /// </summary>
    /// <param name="routine">コルーチン</param>
    public void StopCoroutine(IEnumerator routine)
    {
        m_routines.Remove(routine);
    }

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        if (m_isPaused)
        {
            return;
        }

        for (int i = 0, len = m_routines.Count; i < len; i++)
        {
            IEnumerator routine = m_routines[i];
            object current = routine.Current;

            if (routine.MoveNext() == false)
            {
                if (m_callbacks.TryGetValue(routine, out Action callback))
                {
                    callback();
                }

                m_callbacks.Remove(routine);
                routine = null;
                m_routines.RemoveAt(i);
                i--;
                len--;
            }
        }
    }

    /// <summary>
    /// コルーチンが1件以上実行中か
    /// </summary>
    /// <returns>true: 実行中</returns>
    public bool IsRunning()
    {
        return m_routines.Count > 0;
    }

    /// <summary>
    /// 一時停止
    /// </summary>
    public void Pause()
    {
        m_isPaused = true;
    }

    /// <summary>
    /// 再開
    /// </summary>
    public void Resume()
    {
        m_isPaused = false;
    }
}
