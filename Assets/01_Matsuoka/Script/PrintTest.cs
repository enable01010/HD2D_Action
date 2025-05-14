using UnityEngine;

/// <summary>
/// TextWaitPrint‚ÌTest—pScript
/// </summary>
public class PrintTest : MonoBehaviour
{
    [SerializeField] TextWaitPrint textWaitPrint;
    [SerializeField] string text = "abcdefg";
    [SerializeField] float intervalTime = 0.5f;

    [ContextMenu("Print")]
    private void Print()
    {
        textWaitPrint.PrintStart(text, intervalTime);
    }

    [ContextMenu("AllPrint")]
    private void AllPrint()
    {
        textWaitPrint.AllPrint();
    }
}
