using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Linq;
using System;

internal class LibAudioModule : MonoBehaviour
{
    /// <summary>
    /// BGM用のクラス
    /// </summary>
    private class Bgm
    {
        public AudioSource audioSource;     // BGM用のAudioSource
        public bool isPlaying;              // 流しているか
        public float startTime;             // 音源の開始時間
    }

    #region 変数

    private AudioMixer audioMixer;                      // AudioMixerをフォルダから読み込む

    private Bgm[] bgms;                                 // BGMの管理用

    private AudioClip[] audioClipsSE;                   // 音素材をフォルダから読み込む(PlayOneShotのために保持)

    private AudioSource[] audioSourcesPlay;             // SE用のAudioSource(音が重ならず、上書きされる)
    private AudioSource[] audioSourcesPlayOneShot;      // SE用のAudioSource(音が重なる)

    private int playOneShotIndex = 0;                   // 次に再生する配列番号

    [Header("PlayOneShotを生成する数(音が重なる)")]
    [SerializeField] private int PLAY_ONE_SHOT_LENGTH = 10;
    
    [Header("AudioMixerのVolumの最大値(元のプログラムは0")]
    [SerializeField] private float AUDIO_MIXER_MAX_VOLUME = 15;

    #endregion

    #region Init

    public void Init()
    {
        // オブジェクト生成
        Transform parentSound = new GameObject("Sound").transform;
        Transform parentBGM = new GameObject("BGM").transform;
        Transform parentSE = new GameObject("SE").transform;
        Transform parentPlay = new GameObject("Play").transform;
        Transform parentPlayOneShot = new GameObject("PlayOneShot").transform;

        // 親子関係の設定
        parentSound.SetParent(transform);
        parentBGM.SetParent(parentSound);
        parentSE.SetParent(parentSound);
        parentPlay.SetParent(parentSE);
        parentPlayOneShot.SetParent(parentSE);

        // Resourcesフォルダから取得
        AudioClip[] audioClipBgm = Resources.LoadAll<AudioClip>("Audio/BGM");
        audioClipsSE = Resources.LoadAll<AudioClip>("Audio/SE");

        // enumnの数とaudioClipの数を比較
        int bgmLength = LibFuncUtility.EnumGetLength<AudioBgm>();
        int soundLength = LibFuncUtility.EnumGetLength<AudioSE>();

#if UNITY_EDITOR
        // 一致しないならエラーを出す
        if (bgmLength != audioClipBgm.Length)
        {
            Debug.LogError("Enumの数とBGMの数が一致しません");
        }

        if (soundLength != audioClipsSE.Length)
        {
            Debug.LogError("Enumの数とSEの数が一致しません");
        }
#endif

        // Linqを使ってBGMをenumの順番にソートする
        audioClipBgm = audioClipBgm
            .Where(bgm => Enum.TryParse<AudioBgm>(bgm.name, out _))
            .OrderBy(bgm => Enum.Parse<AudioBgm>(bgm.name))
            .ToArray();

        // Linqを使ってSEをenumの順番にソートする
        audioClipsSE = audioClipsSE
            .Where(se => Enum.TryParse<AudioSE>(se.name, out _))
            .OrderBy(se => Enum.Parse<AudioSE>(se.name))
            .ToArray();

        // audioObjectの生成
        GameObject audioObject = LibResourceLoader._emptyPref;
        audioObject.name = "AudioSource";
        audioObject.AddComponent<AudioSource>();

        // AudioMixerGroupの取得
        audioMixer = Resources.Load("Audio/AudioMixer/AudioMixer") as AudioMixer;
        AudioMixerGroup audioMixerGroupBgm = audioMixer.FindMatchingGroups("Master")[1];
        AudioMixerGroup audioMixerGroupSE = audioMixer.FindMatchingGroups("Master")[2];

        // BGM用AudioSourceの生成
        bgms = new Bgm[bgmLength];
        for (int i = 0; i < bgmLength; i++)
        {
            // 生成
            GameObject obj = Instantiate(audioObject, parentBGM);
            obj.name = ((AudioBgm)i).ToString();

            // AudioSourceの設定
            var audioSource = obj.GetComponent<AudioSource>();
            audioSource.clip = audioClipBgm[i];
            audioSource.outputAudioMixerGroup = audioMixerGroupBgm;

            // 配列生成
            bgms[i] = new Bgm
            {
                audioSource = audioSource,
                isPlaying = false,
                startTime = 0f,
            };
        }

        // SEのPlay用AudioSourceの生成(音が重ならず、上書きされる)
        audioSourcesPlay = new AudioSource[soundLength];
        for (int i = 0; i < soundLength; i++)
        {
            // 生成
            GameObject obj = Instantiate(audioObject, parentPlay);
            obj.name = ((AudioSE)i).ToString();

            // 配列に格納
            audioSourcesPlay[i] = obj.GetComponent<AudioSource>();
            audioSourcesPlay[i].outputAudioMixerGroup = audioMixerGroupSE;
            audioSourcesPlay[i].clip = audioClipsSE[i];
        }

        // SEのPlayOneShot用AudioSourceの生成(音が重なる)
        audioSourcesPlayOneShot = new AudioSource[PLAY_ONE_SHOT_LENGTH];
        for (int i = 0; i < PLAY_ONE_SHOT_LENGTH; i++)
        {
            // 生成
            GameObject obj = Instantiate(audioObject, parentPlayOneShot);
            obj.name = "PlayOneShot_" + i;

            // 配列に格納
            audioSourcesPlayOneShot[i] = obj.GetComponent<AudioSource>();
            audioSourcesPlayOneShot[i].outputAudioMixerGroup = audioMixerGroupSE;
        }
    }

    #endregion

    #region スライダーからAudioMixerの音量を変える

    /// <summary>
    /// スライダーからAudioMixerのMasterの音量を変える
    /// </summary>
    /// <param name="value">スライダーの値</param>
    public void SetAudioMixerMaster(float value)
    {
        // dbに変換
        float volume = Mathf.Clamp(Mathf.Log10(value) * 20f, -80f, 0f) + AUDIO_MIXER_MAX_VOLUME;

        audioMixer.SetFloat("Master", volume);
    }

    /// <summary>
    /// スライダーからAudioMixerのBGMの音量を変える
    /// </summary>
    /// <param name="value">スライダーの値</param>
    public void SetAudioMixerBGM(float value)
    {
        // dbに変換
        float volume = Mathf.Clamp(Mathf.Log10(value) * 20f, -80f, 0f) + AUDIO_MIXER_MAX_VOLUME;

        audioMixer.SetFloat("BGM", volume);
    }

    /// <summary>
    /// スライダーからAudioMixerのSEの音量を変える
    /// </summary>
    /// <param name="value">スライダーの値</param>
    public void SetAudioMixerSE(float value)
    {
        // dbに変換
        float volume = Mathf.Clamp(Mathf.Log10(value) * 20f, -80f, 0f) + AUDIO_MIXER_MAX_VOLUME;

        audioMixer.SetFloat("SE", volume);
    }

    #endregion

    #region 音を流す

    /// <summary>
    /// BGMを流す
    /// </summary>
    /// <param name="number">BgmNameの番号</param>
    /// <param name="position">位置</param>
    /// <param name="volume">音量(0-1)</param>
    /// <param name="startTime">音源の開始時間</param>
    /// <param name="is3D">3Dか</param>
    public void PlayBgm(int number, Vector3 position, float volume, float startTime, bool is3D)
    {
        // 無ければエラー
        if (bgms[number] == null || bgms[number].audioSource == null || bgms[number].audioSource.clip == null)
        {
#if UNITY_EDITOR
            Debug.LogError(((AudioBgm)number).ToString() + "は流せません");
#endif
            return;
        }

        // BGMを流す
        AudioSource audio = bgms[number].audioSource;
        audio.spatialBlend = is3D ? 1.0f : 0.0f;
        audio.transform.position = position;
        audio.volume = Mathf.Clamp01(volume);
        audio.Play();
        audio.time = startTime;// Playの後に設定

        bgms[number].isPlaying = true;
        bgms[number].startTime = startTime;
    }

    /// <summary>
    /// SEを流す(音が重ならず、上書きされる)
    /// </summary>
    /// <param name="number">SoundFxNameの番号</param>
    /// <param name="position">位置</param>
    /// <param name="volume">音量(0-1)</param>
    /// <param name="startTime">音源の開始時間</param>
    /// <param name="is3D">3Dか</param>
    public void PlaySE(int number, Vector3 position, float volume, float startTime, bool is3D)
    {
        // 無ければエラー
        if (audioSourcesPlay[number] == null || audioSourcesPlay[number].clip == null)
        {
#if UNITY_EDITOR
            Debug.Log(((AudioSE)number).ToString() + "は流せません");
#endif
            return;
        }

        // SEを流す
        AudioSource audio = audioSourcesPlay[number];
        audio.spatialBlend = is3D ? 1.0f : 0.0f;
        audio.transform.position = position;
        audio.volume = Mathf.Clamp01(volume);
        audio.Play();
        audio.time = startTime;// Playの後に設定
    }

    /// <summary>
    /// SEを流す(音が重なる)
    /// </summary>
    /// <param name="number">SoundFxNameの番号</param>
    /// <param name="position">位置</param>
    /// <param name="volume">音量(0-1)</param>
    /// <param name="startTime">音源の開始時間</param>
    /// <param name="is3D">3Dか</param>
    public void PlayOneShotSE(int number, Vector3 position, float volume, float startTime, bool is3D)
    {
        // 無ければエラー
        if (audioClipsSE[number] == null)
        {
#if UNITY_EDITOR
            Debug.Log(((AudioSE)number).ToString() + "は流せません");
#endif
            return;
        }

        // SEを流す
        AudioSource audio = audioSourcesPlayOneShot[playOneShotIndex];
        audio.clip = audioClipsSE[number];
        audio.spatialBlend = is3D ? 1.0f : 0.0f;
        audio.transform.position = position;
        audio.volume = Mathf.Clamp01(volume);
        audio.Play();
        audio.time = startTime;// Playの後に設定

        audio.gameObject.name = ((AudioSE)number).ToString();

        // audioSourcePlayOneShotを順番に使っていく
        playOneShotIndex = (playOneShotIndex + 1) % audioSourcesPlayOneShot.Length;
    }

    #endregion

    #region 音を止める

    /// <summary>
    /// BGMを止める
    /// </summary>
    /// <param name="number">BgmNameの番号</param>
    public void StopBgm(int number)
    {
        bgms[number].audioSource.Stop();
        bgms[number].isPlaying = false;
    }

    /// <summary>
    /// 全てのBGMを止める
    /// </summary>
    public void StopAllBgm()
    {
        for (int i = 0; i < bgms.Length; i++)
        {
            StopBgm(i);
        }
    }

    /// <summary>
    /// 全ての音を止める
    /// </summary>
    public void StopAllAudio()
    {
        StopAllBgm();

        foreach (var audioSource in audioSourcesPlay)
        {
            audioSource.Stop();
        }

        foreach (var audioSource in audioSourcesPlayOneShot)
        {
            audioSource.Stop();
        }
    }

    #endregion

    #region AudioMixerの値をスライダーに反映

    /// <summary>
    /// AudioMixerの値をスライダーに反映させる
    /// </summary>
    /// <param name="sliderMaster">Master用のスライダー</param>
    /// <param name="sliderBgm">BGM用のスライダー</param>
    /// <param name="sliderSE">SE用のスライダー</param>
    public void SetSliderValue(Slider sliderMaster, Slider sliderBgm, Slider sliderSE)
    {
        // AudioMixerの音量取得
        audioMixer.GetFloat("Master", out float masterVolume);
        audioMixer.GetFloat("BGM", out float bgmVolume);
        audioMixer.GetFloat("SE", out float seVolume);


        // 初期音量設定
        sliderMaster.value = Mathf.Pow(10f, (masterVolume - AUDIO_MIXER_MAX_VOLUME) / 20f);
        sliderBgm.value = Mathf.Pow(10f, (bgmVolume - AUDIO_MIXER_MAX_VOLUME) / 20f);
        sliderSE.value = Mathf.Pow(10f, (seVolume - AUDIO_MIXER_MAX_VOLUME) / 20f);
    }

    #endregion

    #region BGMのループ再生

    /// <summary>
    /// BGMをループ再生する(loopをfalseにしてUpdateで回す
    /// </summary>
    private void LoopBgm()
    {
        foreach (Bgm bgm in bgms)
        {
            if (bgm.isPlaying == false) continue;
            if (bgm.audioSource.isPlaying == true) continue;

            // 流れているBGMが終わったら、再度流し始める
            bgm.audioSource.Play();
            bgm.audioSource.time = bgm.startTime;// Playの後に設定
        }
    }

    #endregion

    #region Update

    private void Update()
    {
        LoopBgm();
    }

    #endregion
}

public class LibAudio
{
    #region Singleton関係など

    private static LibAudio instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new LibAudio();
            }
            return m_Instance; 
        }
    }

    private static LibAudio m_Instance;

    private LibAudioModule module;

    private LibAudio() 
    {
        GameObject prefab = Resources.Load("Prefabs/AudioModule") as GameObject;

        GameObject tempObject = GameObject.Instantiate(prefab);

        module = tempObject.GetComponent<LibAudioModule>();

        module.Init();

        GameObject.DontDestroyOnLoad(tempObject);
    }

    #endregion

    #region スライダーからAudioMixerの音量を変える

    /// <summary>
    /// スライダーからAudioMixerのMasterの音量を変える
    /// スライダーのonValueChangedに設定する
    /// </summary>
    /// <param name="value">スライダーの値</param>
    public static void SetAudioMixerMaster(float value)
    {
        instance.module.SetAudioMixerMaster(value);
    }

    /// <summary>
    /// スライダーからAudioMixerのBGMの音量を変える
    /// スライダーのonValueChangedに設定する
    /// </summary>
    /// <param name="value">スライダーの値</param>
    public static void SetAudioMixerBGM(float value)
    {
        instance.module.SetAudioMixerBGM(value);
    }

    /// <summary>
    /// スライダーからAudioMixerのSEの音量を変える
    /// スライダーのonValueChangedに設定する
    /// </summary>
    /// <param name="value">スライダーの値</param>
    public static void SetAudioMixerSE(float value)
    {
        instance.module.SetAudioMixerSE(value);
    }

    #endregion

    #region 音を流す

    /// <summary>
    /// BGMを流す(3D)
    /// </summary>
    /// <param name="sound">enumのBgmName</param>
    /// <param name="position">位置</param>
    /// <param name="volume">音量(0-1)</param>
    /// <param name="startTime">音源の開始時間</param>
    public static void PlayBgm3D(AudioBgm sound, Vector3 position, float volume = 0.5f, float startTime = 0)
    {
        instance.module.PlayBgm((int)sound, position, volume, startTime, true);
    }

    /// <summary>
    /// BGMを流す(2D)
    /// </summary>
    /// <param name="sound">enumのBgmName</param>
    /// <param name="volume">音量(0-1)</param>
    /// <param name="startTime">音源の開始時間</param>
    public static void PlayBgm2D(AudioBgm sound, float volume = 0.5f, float startTime = 0)
    {
        instance.module.PlayBgm((int)sound, Vector3.zero, volume, startTime, false);
    }

    /// <summary>
    /// SEを流す(3D)(音が重ならず、上書きされる)
    /// </summary>
    /// <param name="sound">enumのSoundFxName</param>
    /// <param name="position">位置</param>
    /// <param name="volume">音量(0-1)</param>
    /// <param name="startTime">音源の開始時間</param>
    public static void PlaySE3D(AudioSE sound, Vector3 position, float volume = 0.5f, float startTime = 0)
    {
        instance.module.PlaySE((int)sound, position, volume, startTime, true);
    }

    /// <summary>
    /// SEを流す(2D)(音が重ならず、上書きされる)
    /// </summary>
    /// <param name="sound">enumのSoundFxName</param>
    /// <param name="volume">音量(0-1)</param>
    /// <param name="startTime">音源の開始時間</param>
    public static void PlaySE2D(AudioSE sound, float volume = 0.5f, float startTime = 0)
    {
        instance.module.PlaySE((int)sound, Vector3.zero, volume, startTime, false);
    }

    /// <summary>
    /// SEを流す(3D)(音が重なる)
    /// </summary>
    /// <param name="sound">enumのSoundFxName</param>
    /// <param name="position">位置</param>
    /// <param name="volume">音量(0-1)</param>
    /// <param name="startTime">音源の開始時間</param>
    public static void PlayOneShotSE3D(AudioSE sound, Vector3 position, float volume = 0.5f, float startTime = 0)
    {
        instance.module.PlayOneShotSE((int)sound, position, volume, startTime, true);
    }

    /// <summary>
    /// SEを流す(2D)(音が重なる)
    /// </summary>
    /// <param name="sound">enumのSoundFxName</param>
    /// <param name="volume">音量(0-1)</param>
    /// <param name="startTime">音源の開始時間</param>
    public static void PlayOneShotSE2D(AudioSE sound, float volume = 0.5f, float startTime = 0)
    {
        instance.module.PlayOneShotSE((int)sound, Vector3.zero, volume, startTime, false);
    }

    #endregion

    #region PlayBgm Builderパターン

    static public PlayBgm_Builder PlayBgm_BuildStart()
    {
        return new PlayBgm_Builder();
    }

    public class PlayBgm_Builder
    {
        AudioBgm sound = 0;
        Vector3 position = Vector3.zero;
        float volume = 0.5f;
        float startTime = 0;
        bool is3D = false;

        public void PlayBgm()
        {
            instance.module.PlayBgm((int)sound, position, volume, startTime, is3D);
        }

        public PlayBgm_Builder SetSound(AudioBgm sound)
        {
            this.sound = sound;
            return this;
        }

        public PlayBgm_Builder SetPosition(Vector3 position)
        {
            this.position = position;
            return this;
        }

        public PlayBgm_Builder SetVolume(float volume)
        {
            this.volume = volume;
            return this;
        }

        public PlayBgm_Builder SetStartTime(float startTime)
        {
            this.startTime = startTime;
            return this;
        }

        public PlayBgm_Builder SetIs3D(bool is3D)
        {
            this.is3D = is3D;
            return this;
        }
    }

    #endregion

    #region PlaySE Builderパターン

    static public PlaySE_Builder PlaySE_BuildStart()
    {
        return new PlaySE_Builder();
    }

    public class PlaySE_Builder
    {
        AudioSE sound = 0;
        Vector3 position = Vector3.zero;
        float volume = 0.5f;
        float startTime = 0;
        bool is3D = false;

        public void PlaySE()
        {
            instance.module.PlaySE((int)sound, position, volume, startTime, is3D);
        }

        public PlaySE_Builder SetSound(AudioSE sound)
        {
            this.sound = sound;
            return this;
        }

        public PlaySE_Builder SetPosition(Vector3 position)
        {
            this.position = position;
            return this;
        }

        public PlaySE_Builder SetVolume(float volume)
        {
            this.volume = volume;
            return this;
        }

        public PlaySE_Builder SetStartTime(float startTime)
        {
            this.startTime = startTime;
            return this;
        }

        public PlaySE_Builder SetIs3D(bool is3D)
        {
            this.is3D = is3D;
            return this;
        }
    }

    #endregion

    #region PlayOneShotSE Builderパターン

    static public PlayOneShotSE_Builder PlayOneShotSE_BuildStart()
    {
        return new PlayOneShotSE_Builder();
    }

    public class PlayOneShotSE_Builder
    {
        AudioSE sound = 0;
        Vector3 position = Vector3.zero;
        float volume = 0.5f;
        float startTime = 0;
        bool is3D = false;

        public void PlayOneShotSE()
        {
            instance.module.PlayOneShotSE((int)sound, position, volume, startTime, is3D);
        }

        public PlayOneShotSE_Builder SetSound(AudioSE sound)
        {
            this.sound = sound;
            return this;
        }

        public PlayOneShotSE_Builder SetPosition(Vector3 position)
        {
            this.position = position;
            return this;
        }

        public PlayOneShotSE_Builder SetVolume(float volume)
        {
            this.volume = volume;
            return this;
        }

        public PlayOneShotSE_Builder SetStartTime(float startTime)
        {
            this.startTime = startTime;
            return this;
        }

        public PlayOneShotSE_Builder SetIs3D(bool is3D)
        {
            this.is3D = is3D;
            return this;
        }
    }

    #endregion

    #region 音を止める

    /// <summary>
    /// BGMを止める
    /// </summary>
    /// <param name="bgm">BgmName</param>
    public static void StopBgm(AudioBgm bgm)
    {
        instance.module.StopBgm((int)bgm);
    }

    /// <summary>
    /// 全てのBGMを止める
    /// </summary>
    public static void StopAllBgm()
    {
        instance.module.StopAllBgm();
    }

    /// <summary>
    /// 全ての音を止める
    /// </summary>
    public static void StopAllAudio()
    {
        instance.module.StopAllAudio();
    }

    #endregion

    #region AudioMixerの値をスライダーに反映

    /// <summary>
    /// AudioMixerの値をスライダーに反映させる
    /// </summary>
    /// <param name="sliderMaster">Master用のスライダー</param>
    /// <param name="sliderBgm">BGM用のスライダー</param>
    /// <param name="sliderSE">SE用のスライダー</param>
    public static void SliderValueChange(Slider sliderMaster, Slider sliderBgm, Slider sliderSE)
    {
        instance.module.SetSliderValue(sliderMaster, sliderBgm, sliderSE);
    }

    #endregion
}