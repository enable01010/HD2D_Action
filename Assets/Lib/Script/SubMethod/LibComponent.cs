using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

static public class LibComponent
{
    static public void SetActive(this Component cmp, bool active)
    {
        cmp.gameObject.SetActive(active);
    }


}

