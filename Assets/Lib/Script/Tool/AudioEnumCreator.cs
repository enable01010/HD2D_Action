#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

/// <summary>
/// Audio用のEnumSciptを作成するためのクラス
/// [Tools] の [Create Audio Enum Scipt] で作成
/// 生成場所のパス...FILE_PATH = "Assets/Lib/CreateScipts/Audio/AudioEnum.cs";
/// AudioClipが入っているフォルダのパス...Resources/Audio/BGM, Resources/Audio/SE
/// </summary>
public static class AudioEnumCreator
{
    private enum Audio
    {
        Bgm = 0,
        SE,
        Test,
    }

    // 作成場所のパス
    private const string FILE_PATH = "Assets/Lib/CreateScipts/Audio/AudioEnum.cs";

    // AudioClipが入っているフォルダのパス
    private const string AUDIO_PATH = "Audio/{Enum}";

    // コードの中身
    private const string CODE_FORMAT =
@"public enum Audio{Enum}
{{contents}
}";

    private const string REPLACE_ENUM = "{Enum}";
    private const string REPLACE_CONTENTS = "{contents}";

    /// <summary>
    /// Scriptを作成する(Audio用のEnumScipt)
    /// </summary>
    [MenuItem("Tools/Create Audio Enum Scipt")]
    private static void CreateAudioEnumScipt()
    {
        string code = "";

        for (int i = 0; i < LibFuncUtility.EnumGetLength<Audio>(); i++)
        {
            code += CreateCode((Audio)i) + "\n\n";
        }

        // scriptを作成する
        File.WriteAllText(FILE_PATH, code);

        // 追加されたアセットのインポート
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// EnumごとのCodeを生成する
    /// </summary>
    /// <param name="audio">Enumの名前</param>
    private static string CreateCode(Audio audio)
    {
        // AudioClipを読み込む
        AudioClip[] audios = Resources.LoadAll<AudioClip>(AUDIO_PATH.Replace(REPLACE_ENUM, audio.ToString()));

        // AudioClipのファイル名を配列に格納
        string[] audioNames = audios.Select(audio => audio.name).ToArray();

        // enumの中身の用意
        string contents = "";
        foreach (string audioName in audioNames)
        {
            contents += "\n\t" + audioName + ",";
        }

        // CODE_FORMATを置換して完成させる
        return CODE_FORMAT.Replace(REPLACE_ENUM, audio.ToString()).Replace(REPLACE_CONTENTS, contents);
    }
}
#endif
