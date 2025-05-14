using System;
using System.Text;
using UnityEngine;

public static class LibFuncUtility
{
    /// <summary>
    /// ˆø”‚ªfalse‚Ì‚Ì‚İÀ{‚·‚éŠÖ”
    /// </summary>
    /// <param name="isDone">À{‚µ‚½‚±‚Æ‚ª‚ ‚é‚©</param>
    /// <param name="action">À{‚·‚éŠÖ”</param>
    public static void WhenFlaseDoAndReverse(ref bool isDone, Action action)
    {
        if(isDone == false)
        {
            isDone = true;
            action();
        }
    }

    public static string TextFormatBuilder(string format, object arg0 = null, object arg1 = null, object arg2 = null)
    {
        var builder = new StringBuilder();
        builder.AppendFormat(format, arg0, arg1, arg2);
        return builder.ToString();
    }

    /// <summary>
    /// Enum‚Ì—v‘f”‚ğæ“¾‚·‚éŠÖ”
    /// </summary>
    /// <typeparam name="T">æ“¾‚·‚éEnum</typeparam>
    /// <returns></returns>
    public static int EnumGetLength<T>() where T : Enum
    {
        return Enum.GetValues(typeof(T)).Length;
    }
}