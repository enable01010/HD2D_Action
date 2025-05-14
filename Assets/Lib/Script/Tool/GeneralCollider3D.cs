using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GeneralCollider3D : MonoBehaviour
{
    private Action<Collider, GeneralColliderAttribute> onEnter;
    private Action<Collider, GeneralColliderAttribute> onStay;
    private Action<Collider, GeneralColliderAttribute> onExit;
    private GeneralColliderAttribute attribute;

    private void Awake()
    {
        // 引数用変数の作成
        if(attribute == null)
        {
            SetAttribute(new GeneralColliderAttribute());
        }

        // 親オブジェクトから自動検索
        if (onEnter == null && onStay == null && onExit == null)
        {
            Init(transform.GetComponentInParent<I_GeneralColliderUser>());
        }

        // トリガーつけ忘れ用
        GetComponent<Collider>().isTrigger = true;
    }

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="owner">呼び出し元</param>
    public void Init(I_GeneralColliderUser owner)
    {
        if (owner == null) return;
        
        onEnter = owner.OnEnter_GeneralCollider;
        onStay = owner.OnStay_GeneralCollider;
        onExit = owner.OnExit_GeneralCollider;
    }

    public void SetAttribute(GeneralColliderAttribute attribute)
    {
        this.attribute = attribute;
        this.attribute.AddGeneralCollider3D(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(onEnter != null)
            onEnter(other, attribute);
    }

    private void OnTriggerStay(Collider other)
    {
        if (onStay != null)
            onStay(other, attribute);
    }

    private void OnTriggerExit(Collider other)
    {
        if (onExit != null)
            onExit(other, attribute);
    }
}

/// <summary>
/// ジェネラルコライダーを使うようのインターフェース
/// </summary>
public interface I_GeneralColliderUser
{
    /// <summary>
    /// コライダーに触れ始めた際に呼ばれる
    /// </summary>
    /// <param name="other">当たったオブジェクト</param>
    /// <param name="attribute">追加情報</param>
    public void OnEnter_GeneralCollider(Collider other, GeneralColliderAttribute attribute) { }

    /// <summary>
    /// コライダーに続けている際に呼ばれる
    /// </summary>
    /// <param name="other">当たったオブジェクト</param>
    /// <param name="attribute">追加情報</param>
    public void OnStay_GeneralCollider(Collider other, GeneralColliderAttribute attribute) { }

    /// <summary>
    /// コライダーから離れた際に呼ばれる
    /// </summary>
    /// <param name="other">当たったオブジェクト</param>
    /// <param name="attribute">追加情報</param>
    public void OnExit_GeneralCollider(Collider other, GeneralColliderAttribute attribute) { }
}


/// <summary>
/// GeneralCollider3D用の引数用クラス
/// </summary>
public class GeneralColliderAttribute
{
    public GeneralCollider3D gc3d;
    public Transform transform;
    public GameObject gameObject;

    public void AddGeneralCollider3D(GeneralCollider3D gc3d)
    {
        this.gc3d = gc3d;
        transform = gc3d.transform;
        gameObject = gc3d.gameObject;
    }
}