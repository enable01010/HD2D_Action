using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaluculationPos : A_RePosObj
{
    [SerializeField] Transform Zpos;
    [SerializeField] Transform YPos_up;
    [SerializeField] Transform Ypos_down;

    void Awake()
    {
        RePos();
    }

    public override void RePos()
    {
        if (Zpos == null) return;

        Vector3 pos = Zpos.position;
        pos.y += (YPos_up.position.y - Ypos_down.position.y);
        transform.position = pos;
    }
}
