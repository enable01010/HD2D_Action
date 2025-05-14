using UnityEngine;
using TMPro;

/// <summary>
/// ��������\���p��class
/// TextMeshPro��Component����Ă���Object�ɕt����
/// </summary>
public class TextWaitPrint : MonoBehaviour
{
    #region �ϐ�

    // �\������TextMeshPro
    [SerializeField, AutoGetComponent] TMP_Text tmpText;

    // ���Ԃ̌v��
    float countTime = 0;

    // �\���Ԋu
    float intervalTime = 0;

    // �\���\��̕���
    string text = "";

    // ���ݕ\�����Ă��镶��
    string currentText = "";

    // �\�������ǂ���
    bool isPrinting = false;

    #endregion


    #region Unity�֐�

    private void Update()
    {
        PrintUpdate();
    }

    #endregion


    #region ����֐�

    /// <summary>
    /// ��������\�����J�n����
    /// </summary>
    /// <param name="text">�\������������</param>
    /// <param name="intervalTime">�\���Ԋu(�b)</param>
    public void PrintStart(string text, float intervalTime)
    {
        // �l�̕ێ�
        this.intervalTime = intervalTime;
        this.text = text;

        // �\�����̃e�L�X�g��j��
        currentText = "";
        tmpText.SetText(currentText);

        // text��0�����Ȃ�J�n���Ȃ�
        if (text.Length == 0) return;

        // �t���O�ύX
        isPrinting = true;
    }

    /// <summary>
    /// ��������\��������
    /// </summary>
    public void PrintUpdate()
    {
        // �\�����̂ݎ��s
        if (isPrinting == false) return;

        // ���Ԃ̊Ǘ�
        countTime += Time.deltaTime;
        if (countTime < intervalTime) return;
        countTime -= intervalTime;

        // �\�����Ă��镶����������
        currentText += text[currentText.Length];
        tmpText.SetText(currentText);

        // �\�����I��������m�F
        if (currentText.Equals(text)) PrintFinish();
    }

    /// <summary>
    /// �\���̏I��
    /// </summary>
    private void PrintFinish()
    {
        // �t���O�ύX
        isPrinting = false;

        // ���Ԃ̃��Z�b�g
        countTime = 0;
    }

    /// <summary>
    /// �\�����I�������
    /// </summary>
    /// <returns>�I�������true</returns>
    public bool FinishCheck()
    {
        return isPrinting;
    }

    /// <summary>
    /// �����I�ɑS�ĕ\��
    /// </summary>
    public void AllPrint()
    {
        // �\�����̂ݎ��s
        if (isPrinting == false) return;

        // �S�ĕ\��
        currentText = text;
        tmpText.SetText(currentText);

        PrintFinish();
    }

    #endregion
}

