using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// イベントインターフェース
/// </summary>
public interface IEvent { }

/// <summary>
/// イベントの発火、受信を行うためのイベントバス
///
/// 作成日：2020/07/31 K.Suzuki
/// </summary>
public class EventBus
{
    private const float _GUI_UPDATE_RATE = 10;
    private static Dictionary<Type, List<IEventHandler>> g_eventList = new Dictionary<Type, List<IEventHandler>>();
    private static int g_displayEventIndex;
    private static GUIContent[] g_labels = new GUIContent[0];

    public interface IEventHandler
    {
        int Order { get; }
    }

    /// <summary>
    /// イベントハンドラ
    /// </summary>
    public class EventHandler : IEventHandler
    {
        public object Callback { get; set; }
        public int Order { get; private set; }
        public Action OnChangeOrder { private get; set; }
        public Type ReflectedType { get; set; }
    }

    static EventBus()
    {

    }

    /// <summary>
    /// イベントを登録する
    /// </summary>
    /// <typeparam name="TEvent">イベントの種類</typeparam>
    /// <param name="callback">コールバック</param>
    /// <returns>EventHandler</returns>
    public static EventHandler Register<TEvent>(Action<TEvent> callback) where TEvent : IEvent
    {
        Type type = typeof(TEvent);
        if (_Contains<Action<TEvent>>(type, callback))
        {
            return null;
        }

        var handler = MakeHandler(type);
        handler.Callback = callback;
        handler.ReflectedType = callback.Method.ReflectedType;
        return handler;
    }

    /// <summary>
    /// イベントを登録する
    /// </summary>
    /// <param name="type">イベントの種類</param>
    /// <param name="callback">コールバック</param>
    /// <returns>EventHandler</returns>
    public static EventHandler Register(Type type, Action callback)
    {
        if (_Contains<Action>(type, callback))
        {
            return null;
        }

        var handler = MakeHandler(type);
        handler.Callback = callback;
        handler.ReflectedType = callback.Method.ReflectedType;
        return handler;
    }

    /// <summary>
    /// イベントハンドラを作成する
    /// </summary>
    /// <param name="type">型</param>
    /// <returns>EventHandler</returns>
    private static EventHandler MakeHandler(Type type)
    {
        List<IEventHandler> list = new List<IEventHandler>();

        var handler = new EventHandler();
        handler.OnChangeOrder = () => list.Sort((a, b) => a.Order - b.Order);

        list.Add(handler);
        g_eventList[type] = list;
        return handler;
    }

    /// <summary>
    /// イベントを解除する
    ///
    /// 登録したら必ず解除処理を書く事！
    /// </summary>
    /// <typeparam name="TEvent">イベントの種類</typeparam>
    /// <param name="handler">イベントハンドラ</param>
    public static void UnRegister<TEvent>(Action<TEvent> handler) where TEvent : IEvent
    {
        if (g_eventList.TryGetValue(typeof(TEvent), out List<IEventHandler> list))
        {
            list.RemoveAll(e =>
            {
                if (e is EventHandler eh && eh.Callback is Action<TEvent> callback)
                {
                    return callback == handler;
                }
                return false;
            });
        }
    }

    /// <summary>
    /// イベントを解除する
    ///
    /// 登録したら必ず解除処理を書く事！
    /// </summary>
    /// <param name="type">イベント型</param>
    /// <param name="handler">イベントハンドラ</param>
    public static void UnRegister(Type type, Action handler)
    {
        if (g_eventList.TryGetValue(type, out List<IEventHandler> list))
        {
            list.RemoveAll(e =>
            {
                if (e is EventHandler eh && eh.Callback is Action callback)
                {
                    return callback == handler;
                }
                return false;
            });
        }
    }

    /// <summary>
    /// イベントを発生させる
    /// </summary>
    /// <typeparam name="TEvent">イベントの種類</typeparam>
    /// <param name="eventData">イベントデータ</param>
    public static void Raise<TEvent>(TEvent eventData = default) where TEvent : IEvent
    {
        Type type = typeof(TEvent);
        if (g_eventList.TryGetValue(type, out List<IEventHandler> receivers))
        {
            for (int i = 0, len = receivers.Count; i < len; i++)
            {
                IEventHandler reciever = receivers[i];
                if (reciever == null)
                {
                    continue;
                }

                if (reciever is EventHandler genericHandler)
                {
                    if (genericHandler.Callback is Action<TEvent> callback)
                    {
                        callback(eventData);
                    }
                    else if (genericHandler.Callback is Action callbackNoArg)
                    {
                        callbackNoArg();
                    }
                }
            }
        }
    }

    /// <summary>
    /// 全イベントを破棄
    /// </summary>
    public static void Clear()
    {
        g_eventList.Clear();
    }

    /// <summary>
    /// 既に登録済みか
    /// </summary>
    /// <typeparam name="TCallback">コールバックの型</typeparam>
    /// <param name="type">イベント型</param>
    /// <param name="callbackObj">コールバック</param>
    /// <returns>TRUE:登録済み</returns>
    private static bool _Contains<TCallback>(Type type, object callbackObj) where TCallback : Delegate
    {
        TCallback callback = callbackObj as TCallback;
        if (g_eventList.TryGetValue(type, out List<IEventHandler> list))
        {
            foreach (IEventHandler h in list)
            {
                if (h is EventHandler handler)
                {
                    if (handler.Callback is TCallback handlerCallback && handlerCallback == callback)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}

