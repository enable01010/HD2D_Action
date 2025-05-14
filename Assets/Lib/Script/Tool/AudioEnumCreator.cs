#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

/// <summary>
/// Audio�p��EnumScipt���쐬���邽�߂̃N���X
/// [Tools] �� [Create Audio Enum Scipt] �ō쐬
/// �����ꏊ�̃p�X...FILE_PATH = "Assets/Lib/CreateScipts/Audio/AudioEnum.cs";
/// AudioClip�������Ă���t�H���_�̃p�X...Resources/Audio/BGM, Resources/Audio/SE
/// </summary>
public static class AudioEnumCreator
{
    private enum Audio
    {
        Bgm = 0,
        SE,
        Test,
    }

    // �쐬�ꏊ�̃p�X
    private const string FILE_PATH = "Assets/Lib/CreateScipts/Audio/AudioEnum.cs";

    // AudioClip�������Ă���t�H���_�̃p�X
    private const string AUDIO_PATH = "Audio/{Enum}";

    // �R�[�h�̒��g
    private const string CODE_FORMAT =
@"public enum Audio{Enum}
{{contents}
}";

    private const string REPLACE_ENUM = "{Enum}";
    private const string REPLACE_CONTENTS = "{contents}";

    /// <summary>
    /// Script���쐬����(Audio�p��EnumScipt)
    /// </summary>
    [MenuItem("Tools/Create Audio Enum Scipt")]
    private static void CreateAudioEnumScipt()
    {
        string code = "";

        for (int i = 0; i < LibFuncUtility.EnumGetLength<Audio>(); i++)
        {
            code += CreateCode((Audio)i) + "\n\n";
        }

        // script���쐬����
        File.WriteAllText(FILE_PATH, code);

        // �ǉ����ꂽ�A�Z�b�g�̃C���|�[�g
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// Enum���Ƃ�Code�𐶐�����
    /// </summary>
    /// <param name="audio">Enum�̖��O</param>
    private static string CreateCode(Audio audio)
    {
        // AudioClip��ǂݍ���
        AudioClip[] audios = Resources.LoadAll<AudioClip>(AUDIO_PATH.Replace(REPLACE_ENUM, audio.ToString()));

        // AudioClip�̃t�@�C������z��Ɋi�[
        string[] audioNames = audios.Select(audio => audio.name).ToArray();

        // enum�̒��g�̗p��
        string contents = "";
        foreach (string audioName in audioNames)
        {
            contents += "\n\t" + audioName + ",";
        }

        // CODE_FORMAT��u�����Ċ���������
        return CODE_FORMAT.Replace(REPLACE_ENUM, audio.ToString()).Replace(REPLACE_CONTENTS, contents);
    }
}
#endif
