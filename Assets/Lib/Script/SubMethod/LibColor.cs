using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// よく使う色関係のクラス
/// </summary>
public class LibColor
{
    /// <summary>
    /// 色の１値のみを変更する処理
    /// </summary>
    /// <param name="col">元の色</param>
    /// <param name="value">変更後の値(0~1の場合はそのまま）(1～255の場合は比率に変換）</param>
    /// <param name="mode">変更する色（初期値は透明度）</param>
    /// <returns></returns>
    static public Color ChengeOneDir(Color color,float value,ColorT_Mode mode = ColorT_Mode.a)
    {
        Color answer = new Color();
        
        value = LibMath.CastLimit(value, 0, float.PositiveInfinity);
        value = (value <= 1.0f) ? value : LibMath.GetRangeToValue(value, 0, 255);
        answer = color;
        switch (mode)
        {
            case ColorT_Mode.r:
                answer.r = value;
                break;
            case ColorT_Mode.g:
                answer.g = value;
                break;
            case ColorT_Mode.b:
                answer.b = value;
                break;
            case ColorT_Mode.a:
                answer.a = value;
                break;
        }

        return answer;
    }

    /// <summary>
    /// 点滅させる処理
    /// </summary>
    /// <param name="col">元の色</param>
    /// <param name="pitch">点滅の周期</param>
    /// <returns></returns>
    static public Color FlashColor(Color col,float pitch)
    {
        Color answer = new Color();

        float alpha = LibMath.GetMetoronomeZeroToOne(pitch);
        answer = ChengeOneDir(col, alpha);

        return answer;
    }


    /// <summary>
    /// Mathf.LerpのColor版
    /// </summary>
    /// <param name="start"> 始めの色 </param>
    /// <param name="end"> 目標の色 </param>
    /// <param name="ratio"> 0 ～ 1 で割合入力 </param>
    /// <returns></returns>
    static public Color Lerp(Color start, Color end, float ratio)
    {
        Color answer;

        float r = Mathf.Lerp(start.r, end.r, ratio);
        float g = Mathf.Lerp(start.g, end.g, ratio);
        float b = Mathf.Lerp(start.b, end.b, ratio);
        float a = Mathf.Lerp(start.a, end.a, ratio);

        answer = new Color(r, g, b, a);

        return answer;
    }
}

public enum ColorT_Mode
{
    r,
    g,
    b,
    a
}
