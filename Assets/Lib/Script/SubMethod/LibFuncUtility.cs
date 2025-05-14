using System;
using System.Text;
using UnityEngine;

public static class LibFuncUtility
{
    /// <summary>
    /// ������false�̎��̂ݎ��{����֐�
    /// </summary>
    /// <param name="isDone">���{�������Ƃ����邩</param>
    /// <param name="action">���{����֐�</param>
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
    /// Enum�̗v�f�����擾����֐�
    /// </summary>
    /// <typeparam name="T">�擾����Enum</typeparam>
    /// <returns></returns>
    public static int EnumGetLength<T>() where T : Enum
    {
        return Enum.GetValues(typeof(T)).Length;
    }
}