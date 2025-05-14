using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_GameManager<T> : Singleton<T> where T : A_GameManager<T>
{
    protected A_GameManagerState state;
    public List<I_GameManagerEventListener> eventListeners { get; protected set; }
        = new List<I_GameManagerEventListener>();

    public virtual void AddListener(I_GameManagerEventListener listener)
    {
        eventListeners.Add(listener);
    }

    protected virtual void Update()
    {
        state.OnUpdate();

        CreateEvents();
    }

    protected virtual void LateUpdate()
    {
        state.OnLateUpdate();
    }

    protected virtual void FixedUpdate()
    {
        state.OnFixedUpdate();
    }

    public void ChangeState(A_GameManagerState next)
    {
        state?.OnExit();
        state = next;
        state.OnEnter();
    }

    /// <summary>
    /// プレイヤーの入力をイベントに変化させる処理
    /// </summary>
    private void CreateEvents()
    {
        int count = eventListeners.Count;

        //左クリック系
        if (Input.GetMouseButtonDown(0))
        {
            state.GetMouseButtonDownLeft();
        }
        else if (Input.GetMouseButton(0))
        {
            state.GetMouseButtonLeft();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            state.GetMouseButtonUpLeft();
        }

        //右クリック系
        if (Input.GetMouseButtonDown(1))
        {
            state.GetMouseButtonDownRight();
        }
        else if (Input.GetMouseButton(1))
        {
            state.GetMouseButtonRight();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            state.GetMouseButtonUpRight();
        }
    }

    /// <summary>
    /// ステートに必要な要素の宣言用クラス
    /// </summary>
    [System.Serializable]
    public abstract class A_GameManagerState
    {
        protected T manager;
        public virtual void OnInit(T owner)
        {
            this.manager = owner;
        }

        //ステートパターン
        public virtual void OnEnter() { }
        public virtual void OnUpdate() { }
        public virtual void OnLateUpdate() { }
        public virtual void OnFixedUpdate() { }
        public virtual void OnExit() { }

        //ストラテジーパターン
        public virtual void GetMouseButtonDownLeft() { }
        public virtual void GetMouseButtonLeft() { }
        public virtual void GetMouseButtonUpLeft() { }
        public virtual void GetMouseButtonDownRight() { }
        public virtual void GetMouseButtonRight() { }
        public virtual void GetMouseButtonUpRight() { }
    }

    /// <summary>
    /// すべての通知をそのまま送信するクラス
    /// </summary>
    [System.Serializable]
    public class DefaultState : A_GameManagerState, I_DefaultHnadler
    {
        T I_DefaultHnadler.owner => this.manager;

        //ストラテジーパターン
        public override void GetMouseButtonDownLeft() { ((I_DefaultHnadler)this).MouseButtonDownLeft(); }
        public override void GetMouseButtonLeft() { ((I_DefaultHnadler)this).MouseButtonLeft(); }
        public override void GetMouseButtonUpLeft() { ((I_DefaultHnadler)this).MouseButtonUpLeft(); }
        public override void GetMouseButtonDownRight() { ((I_DefaultHnadler)this).MouseButtonDownRight(); }
        public override void GetMouseButtonRight() { ((I_DefaultHnadler)this).MouseButtonRight(); }
        public override void GetMouseButtonUpRight() { ((I_DefaultHnadler)this).MouseButtonUpRight(); }
    }

    /// <summary>
    /// 受け取ったイベントをそのまますべてに通知するインターフェース
    /// </summary>
    public interface I_DefaultHnadler
    {
        public T owner { get; }
        public virtual void MouseButtonDownLeft() {
            for(int i = 0; i < owner.eventListeners.Count; i++)
            {
                owner.eventListeners[i].GetMouseButtonDownLeft();
            }
        }
        public virtual void MouseButtonLeft()
        {
            for (int i = 0; i < owner.eventListeners.Count; i++)
            {
                owner.eventListeners[i].GetMouseButtonLeft();
            }
        }
        public virtual void MouseButtonUpLeft()
        {
            for (int i = 0; i < owner.eventListeners.Count; i++)
            {
                owner.eventListeners[i].GetMouseButtonUpLeft();
            }
        }
        public virtual void MouseButtonDownRight()
        {
            for (int i = 0; i < owner.eventListeners.Count; i++)
            {
                owner.eventListeners[i].GetMouseButtonDownRight();
            }
        }
        public virtual void MouseButtonRight()
        {
            for (int i = 0; i < owner.eventListeners.Count; i++)
            {
                owner.eventListeners[i].GetMouseButtonRight();
            }
        }
        public virtual void MouseButtonUpRight()
        {
            for (int i = 0; i < owner.eventListeners.Count; i++)
            {
                owner.eventListeners[i].GetMouseButtonUpRight();
            }
        }
    }

}

/// <summary>
/// イベントを受け取る側のインターフェース
/// </summary>
/// <typeparam name="T"></typeparam>
public interface I_GameManagerEventListener
{
    public void GetMouseButtonDownLeft();
    public void GetMouseButtonLeft();
    public void GetMouseButtonUpLeft();
    public void GetMouseButtonDownRight();
    public void GetMouseButtonRight();
    public void GetMouseButtonUpRight();
}

/// <summary>
/// イベントを受け取るMonoBehaviourクラス
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class A_GameManagerEventListenerMono<T>:MonoBehaviour, I_GameManagerEventListener where T : A_GameManager<T>
{
    protected virtual void Start()
    {
        A_GameManager<T>.instance.AddListener(this);
    }

    public virtual void GetMouseButtonDownLeft() { }
    public virtual void GetMouseButtonLeft() { }
    public virtual void GetMouseButtonUpLeft() { }
    public virtual void GetMouseButtonDownRight() { }
    public virtual void GetMouseButtonRight() { }
    public virtual void GetMouseButtonUpRight() { }
}

/// <summary>
/// イベントを受け取るSingletonクラス
/// </summary>
/// <typeparam name="S"></typeparam>
/// <typeparam name="G"></typeparam>
public abstract class A_GameManagerEventListenerSingleton<S,G>:Singleton<S>, I_GameManagerEventListener
    where S : Singleton<S>
    where G : A_GameManager<G>
{
    public override void OnInitialize()
    {
        base.OnInitialize();

        A_GameManager<G>.instance.AddListener(this);
    }

    public virtual void GetMouseButtonDownLeft() { }
    public virtual void GetMouseButtonLeft() { }
    public virtual void GetMouseButtonUpLeft() { }
    public virtual void GetMouseButtonDownRight() { }
    public virtual void GetMouseButtonRight() { }
    public virtual void GetMouseButtonUpRight() { }
}
