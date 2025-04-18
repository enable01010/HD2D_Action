using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_RePos
{
    public void RePos();
}

public class A_RePosObj : MonoBehaviour, I_RePos
{
    public virtual void RePos() { }
}

public class MeshCornerPos :  A_RePosObj
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] RefPosType refPos;

    void Awake()
    {
        RePos();
    }

    public override void RePos()
    {
        if (_spriteRenderer == null) return;
        Matrix4x4 _m = _spriteRenderer.localToWorldMatrix;
        Sprite _sprite = _spriteRenderer.sprite;
        float _halfX = _sprite.bounds.extents.x;
        float _halfY = _sprite.bounds.extents.y;
        Vector3 _vec = Vector3.zero;
        switch (refPos)
        {
            case RefPosType.left_up:
                _vec = new Vector3(-_halfX, _halfY*2, 0f);
                break;
            case RefPosType.left_down:
                _vec = new Vector3(-_halfX, 0, 0f);
                break;
            case RefPosType.right_up:
                _vec = new Vector3(_halfX, _halfY*2, 0f);
                break;
            case RefPosType.right_down:
                _vec = new Vector3(_halfX, 0, 0f);
                break;
        }
        Vector3 _pos = _m.MultiplyPoint3x4(_vec);
        transform.position = _pos;
    }

}

public enum RefPosType
{
    left_up,
    left_down,
    right_up,
    right_down
}
