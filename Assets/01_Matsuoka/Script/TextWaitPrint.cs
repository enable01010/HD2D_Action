using UnityEngine;
using TMPro;

/// <summary>
/// 文字送り表示用のclass
/// TextMeshProがComponentされているObjectに付ける
/// </summary>
public class TextWaitPrint : MonoBehaviour
{
    #region 変数

    // 表示するTextMeshPro
    [SerializeField, AutoGetComponent] TMP_Text tmpText;

    // 時間の計測
    float countTime = 0;

    // 表示間隔
    float intervalTime = 0;

    // 表示予定の文字
    string text = "";

    // 現在表示している文字
    string currentText = "";

    // 表示中かどうか
    bool isPrinting = false;

    #endregion


    #region Unity関数

    private void Update()
    {
        PrintUpdate();
    }

    #endregion


    #region 自作関数

    /// <summary>
    /// 文字送り表示を開始する
    /// </summary>
    /// <param name="text">表示したい文字</param>
    /// <param name="intervalTime">表示間隔(秒)</param>
    public void PrintStart(string text, float intervalTime)
    {
        // 値の保持
        this.intervalTime = intervalTime;
        this.text = text;

        // 表示中のテキストを破棄
        currentText = "";
        tmpText.SetText(currentText);

        // textが0文字なら開始しない
        if (text.Length == 0) return;

        // フラグ変更
        isPrinting = true;
    }

    /// <summary>
    /// 文字送り表示をする
    /// </summary>
    public void PrintUpdate()
    {
        // 表示中のみ実行
        if (isPrinting == false) return;

        // 時間の管理
        countTime += Time.deltaTime;
        if (countTime < intervalTime) return;
        countTime -= intervalTime;

        // 表示している文字をかえる
        currentText += text[currentText.Length];
        tmpText.SetText(currentText);

        // 表示が終わったか確認
        if (currentText.Equals(text)) PrintFinish();
    }

    /// <summary>
    /// 表示の終了
    /// </summary>
    private void PrintFinish()
    {
        // フラグ変更
        isPrinting = false;

        // 時間のリセット
        countTime = 0;
    }

    /// <summary>
    /// 表示が終わったか
    /// </summary>
    /// <returns>終わったらtrue</returns>
    public bool FinishCheck()
    {
        return isPrinting;
    }

    /// <summary>
    /// 強制的に全て表示
    /// </summary>
    public void AllPrint()
    {
        // 表示中のみ実行
        if (isPrinting == false) return;

        // 全て表示
        currentText = text;
        tmpText.SetText(currentText);

        PrintFinish();
    }

    #endregion
}

