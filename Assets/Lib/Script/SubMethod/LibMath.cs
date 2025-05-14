using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// よく使う数学チックな処理用のクラス
/// </summary>
public class LibMath 
{

    /// <summary>
    /// 上限下限に値を矯正する処理
    /// </summary>
    /// <param name="value">実際の値</param>
    /// <param name="min">下限</param>
    /// <param name="max">上限</param>
    /// <returns>矯正した値</returns>
    static public float CastLimit(float value, float min, float max)
    {
        float answer = 0;

        answer = value;
        answer = (answer < min) ? min : answer;
        answer = (answer > max) ? max : answer;

        return answer;
    }

    /// <summary>
    /// 上限下限に対する実数から比率を計算する処理
    /// </summary>
    /// <param name="value">実際の値</param>
    /// <param name="min">下限</param>
    /// <param name="max">上限</param>
    /// <param name="isCast">0~1に矯正するかどうか</param>
    /// <returns>比率</returns>
    static public float GetRangeToValue(float value,float min,float max,bool isCast = true)
    {
        float answer = 0;

        answer = (value - min) / (max - min);
        answer = (isCast) ? CastLimit(answer, 0, 1) : answer;

        return answer;
    }

    /// <summary>
    /// 上限下限に対する比率から実数を計算する処理
    /// </summary>
    /// <param name="raito">比率</param>
    /// <param name="min">下限</param>
    /// <param name="max">上限</param>
    /// <param name="isCast">上限下限に矯正するかどうか</param>
    /// <returns>実数</returns>
    static public float GetValueToRange(float raito,float min,float max,bool isCast = true)
    {
        float answer = 0;

        answer = min + (max - min) * raito;
        answer = (isCast) ? CastLimit(answer, min, max) : answer;

        return answer;
    }

    /// <summary>
    /// 比率を反転する処理
    /// </summary>
    /// <param name="raito">0～1</param>
    /// <param name="isErrorLog">エラーを通知するか（デフォルトは通知する）</param>
    /// <returns>反転した数</returns>
    static public float GetRaito_anti(float raito,bool isErrorLog = true)
    {
        if (isErrorLog == true &&　(raito < 0 || raito > 1))
            Debug.LogWarning(
                "GetRaito_anti関数の引数が不正の可能性があります\n" + 
                "エラーを通知しない場合は第二引数をfalseにしてください"
                );

        float answer = 0;

        answer = 1 - answer;

        return answer;
    }

    /// <summary>
    /// 上限下限に対する比率の反対を計算する処理
    /// </summary>
    /// <param name="value">実数</param>
    /// <param name="min">下限</param>
    /// <param name="max">上限</param>
    /// <returns>比率を反転した数</returns>
    static public float GetRaito_anti(float value,float min,float max)
    {
        float answer = 0;

        answer = GetRangeToValue(value, min, max);
        answer = GetRaito_anti(answer);

        return answer;
    }

    /// <summary>
    /// 逆数を計算する処理
    /// </summary>
    /// <param name="value">実数</param>
    /// <returns>逆数</returns>
    static public float GetReverse(float value)
    {
        float answer = 0;

        answer = 1 / value;

        return answer;
    }

    /// <summary>
    /// 1以下の少数を反転する処理
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    static public float OneMinus(float value, bool isErrorLog = true)
    {
        if (isErrorLog == true && value > 1)
            Debug.LogWarning(
                "OneMinus関数の引数が不正の可能性があります\n" +
                "エラーを通知しない場合は第二引数をfalseにしてください"
                );

        float answer = 0;

        answer = 1 - value;

        return answer;
    }

    /// <summary>
    /// 0～1までの数値を周期的に返す関数
    /// </summary>
    /// <param name="pitch">周期</param>
    /// <param name="offset">周期のずらし</param>
    /// <returns></returns>
    static public float GetMetoronomeZeroToOne(float pitch,float offset = 0)
    {
        float answer = 0;

        float time = offset + Time.realtimeSinceStartup;//ゲームが開始してからの経過時間
        float theta = time * 2 * Mathf.PI * pitch;
        float y = Mathf.Sin(theta);
        answer = GetRangeToValue(y, -1, 1);

        return answer;
    }

    /// <summary>
    /// 角度を0～360に修正する処理
    /// </summary>
    /// <param name="lfAngle"></param>
    /// <returns></returns>
    public static float ClampAngle(float lfAngle)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return lfAngle;
    }

    /// <summary>
    /// 角度を-180～180に修正する処理
    /// </summary>
    /// <param name="angle">修正する前の角度</param>
    /// <returns>修正後の角度</returns>
    public static float ClampAngle180(float angle)
    {
        if (angle < -180) angle += 360f;
        if (angle > 180) angle -= 360f;
        return angle;
    }


    public static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    /// <summary>
    /// valueを最高速度までの変化量で目標値に近づける
    /// </summary>
    /// <param name="value"></param>
    /// <param name="goal"></param>
    /// <param name="maxSpeed"></param>
    /// <returns></returns>
    public static float MoveForcusSpeed(float value,float goal,float maxSpeed)
    {
        if (Mathf.Abs(value - goal) < maxSpeed) return goal;

        float direction = (value > goal) ? -1 : 1;
        return value + maxSpeed * direction;
    }

    public static float ZeroOrOne(float value)
    {
        float answer = 0;
        if(value > 0)
        {
            answer = 1;
        }
        else if(value < 0)
        {
            answer = -1;
        }

        return answer;
    }
}
