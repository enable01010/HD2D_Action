using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/ButtonUIData")]
public class ButtonUIData : ScriptableObject
{
    public ButtonType type;
    public ButtonSpliteData[] sprites;
    public float size;
}

