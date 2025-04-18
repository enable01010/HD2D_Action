using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallerlPlane : A_RePosObj
{
    [SerializeField] SpriteRenderer _spriteRenderer;

    void Awake()
    {
        RePos();
    }

    public override void RePos()
    {
        Matrix4x4 _m = _spriteRenderer.localToWorldMatrix;
        Sprite _sprite = _spriteRenderer.sprite;
        float _halfX = _sprite.bounds.extents.x;
        float _halfY = _sprite.bounds.extents.y;
        Vector3 _vec = _vec = new Vector3(-_halfX, _halfY*2, 0f);
        
        Vector3 _pos = _m.MultiplyPoint3x4(_vec);
        _pos.x = transform.position.x;
        transform.position = _pos;
    }
}
