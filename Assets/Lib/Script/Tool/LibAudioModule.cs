using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Linq;
using System;

internal class LibAudioModule : MonoBehaviour
{
    /// <summary>
    /// BGM�p�̃N���X
    /// </summary>
    private class Bgm
    {
        public AudioSource audioSource;     // BGM�p��AudioSource
        public bool isPlaying;              // �����Ă��邩
        public float startTime;             // �����̊J�n����
    }

    #region �ϐ�

    private AudioMixer audioMixer;                      // AudioMixer���t�H���_����ǂݍ���

    private Bgm[] bgms;                                 // BGM�̊Ǘ��p

    private AudioClip[] audioClipsSE;                   // ���f�ނ��t�H���_����ǂݍ���(PlayOneShot�̂��߂ɕێ�)

    private AudioSource[] audioSourcesPlay;             // SE�p��AudioSource(�����d�Ȃ炸�A�㏑�������)
    private AudioSource[] audioSourcesPlayOneShot;      // SE�p��AudioSource(�����d�Ȃ�)

    private int playOneShotIndex = 0;                   // ���ɍĐ�����z��ԍ�

    [Header("PlayOneShot�𐶐����鐔(�����d�Ȃ�)")]
    [SerializeField] private int PLAY_ONE_SHOT_LENGTH = 10;
    
    [Header("AudioMixer��Volum�̍ő�l(���̃v���O������0")]
    [SerializeField] private float AUDIO_MIXER_MAX_VOLUME = 15;

    #endregion

    #region Init

    public void Init()
    {
        // �I�u�W�F�N�g����
        Transform parentSound = new GameObject("Sound").transform;
        Transform parentBGM = new GameObject("BGM").transform;
        Transform parentSE = new GameObject("SE").transform;
        Transform parentPlay = new GameObject("Play").transform;
        Transform parentPlayOneShot = new GameObject("PlayOneShot").transform;

        // �e�q�֌W�̐ݒ�
        parentSound.SetParent(transform);
        parentBGM.SetParent(parentSound);
        parentSE.SetParent(parentSound);
        parentPlay.SetParent(parentSE);
        parentPlayOneShot.SetParent(parentSE);

        // Resources�t�H���_����擾
        AudioClip[] audioClipBgm = Resources.LoadAll<AudioClip>("Audio/BGM");
        audioClipsSE = Resources.LoadAll<AudioClip>("Audio/SE");

        // enumn�̐���audioClip�̐����r
        int bgmLength = LibFuncUtility.EnumGetLength<AudioBgm>();
        int soundLength = LibFuncUtility.EnumGetLength<AudioSE>();

#if UNITY_EDITOR
        // ��v���Ȃ��Ȃ�G���[���o��
        if (bgmLength != audioClipBgm.Length)
        {
            Debug.LogError("Enum�̐���BGM�̐�����v���܂���");
        }

        if (soundLength != audioClipsSE.Length)
        {
            Debug.LogError("Enum�̐���SE�̐�����v���܂���");
        }
#endif

        // Linq���g����BGM��enum�̏��ԂɃ\�[�g����
        audioClipBgm = audioClipBgm
            .Where(bgm => Enum.TryParse<AudioBgm>(bgm.name, out _))
            .OrderBy(bgm => Enum.Parse<AudioBgm>(bgm.name))
            .ToArray();

        // Linq���g����SE��enum�̏��ԂɃ\�[�g����
        audioClipsSE = audioClipsSE
            .Where(se => Enum.TryParse<AudioSE>(se.name, out _))
            .OrderBy(se => Enum.Parse<AudioSE>(se.name))
            .ToArray();

        // audioObject�̐���
        GameObject audioObject = LibResourceLoader._emptyPref;
        audioObject.name = "AudioSource";
        audioObject.AddComponent<AudioSource>();

        // AudioMixerGroup�̎擾
        audioMixer = Resources.Load("Audio/AudioMixer/AudioMixer") as AudioMixer;
        AudioMixerGroup audioMixerGroupBgm = audioMixer.FindMatchingGroups("Master")[1];
        AudioMixerGroup audioMixerGroupSE = audioMixer.FindMatchingGroups("Master")[2];

        // BGM�pAudioSource�̐���
        bgms = new Bgm[bgmLength];
        for (int i = 0; i < bgmLength; i++)
        {
            // ����
            GameObject obj = Instantiate(audioObject, parentBGM);
            obj.name = ((AudioBgm)i).ToString();

            // AudioSource�̐ݒ�
            var audioSource = obj.GetComponent<AudioSource>();
            audioSource.clip = audioClipBgm[i];
            audioSource.outputAudioMixerGroup = audioMixerGroupBgm;

            // �z�񐶐�
            bgms[i] = new Bgm
            {
                audioSource = audioSource,
                isPlaying = false,
                startTime = 0f,
            };
        }

        // SE��Play�pAudioSource�̐���(�����d�Ȃ炸�A�㏑�������)
        audioSourcesPlay = new AudioSource[soundLength];
        for (int i = 0; i < soundLength; i++)
        {
            // ����
            GameObject obj = Instantiate(audioObject, parentPlay);
            obj.name = ((AudioSE)i).ToString();

            // �z��Ɋi�[
            audioSourcesPlay[i] = obj.GetComponent<AudioSource>();
            audioSourcesPlay[i].outputAudioMixerGroup = audioMixerGroupSE;
            audioSourcesPlay[i].clip = audioClipsSE[i];
        }

        // SE��PlayOneShot�pAudioSource�̐���(�����d�Ȃ�)
        audioSourcesPlayOneShot = new AudioSource[PLAY_ONE_SHOT_LENGTH];
        for (int i = 0; i < PLAY_ONE_SHOT_LENGTH; i++)
        {
            // ����
            GameObject obj = Instantiate(audioObject, parentPlayOneShot);
            obj.name = "PlayOneShot_" + i;

            // �z��Ɋi�[
            audioSourcesPlayOneShot[i] = obj.GetComponent<AudioSource>();
            audioSourcesPlayOneShot[i].outputAudioMixerGroup = audioMixerGroupSE;
        }
    }

    #endregion

    #region �X���C�_�[����AudioMixer�̉��ʂ�ς���

    /// <summary>
    /// �X���C�_�[����AudioMixer��Master�̉��ʂ�ς���
    /// </summary>
    /// <param name="value">�X���C�_�[�̒l</param>
    public void SetAudioMixerMaster(float value)
    {
        // db�ɕϊ�
        float volume = Mathf.Clamp(Mathf.Log10(value) * 20f, -80f, 0f) + AUDIO_MIXER_MAX_VOLUME;

        audioMixer.SetFloat("Master", volume);
    }

    /// <summary>
    /// �X���C�_�[����AudioMixer��BGM�̉��ʂ�ς���
    /// </summary>
    /// <param name="value">�X���C�_�[�̒l</param>
    public void SetAudioMixerBGM(float value)
    {
        // db�ɕϊ�
        float volume = Mathf.Clamp(Mathf.Log10(value) * 20f, -80f, 0f) + AUDIO_MIXER_MAX_VOLUME;

        audioMixer.SetFloat("BGM", volume);
    }

    /// <summary>
    /// �X���C�_�[����AudioMixer��SE�̉��ʂ�ς���
    /// </summary>
    /// <param name="value">�X���C�_�[�̒l</param>
    public void SetAudioMixerSE(float value)
    {
        // db�ɕϊ�
        float volume = Mathf.Clamp(Mathf.Log10(value) * 20f, -80f, 0f) + AUDIO_MIXER_MAX_VOLUME;

        audioMixer.SetFloat("SE", volume);
    }

    #endregion

    #region ���𗬂�

    /// <summary>
    /// BGM�𗬂�
    /// </summary>
    /// <param name="number">BgmName�̔ԍ�</param>
    /// <param name="position">�ʒu</param>
    /// <param name="volume">����(0-1)</param>
    /// <param name="startTime">�����̊J�n����</param>
    /// <param name="is3D">3D��</param>
    public void PlayBgm(int number, Vector3 position, float volume, float startTime, bool is3D)
    {
        // ������΃G���[
        if (bgms[number] == null || bgms[number].audioSource == null || bgms[number].audioSource.clip == null)
        {
#if UNITY_EDITOR
            Debug.LogError(((AudioBgm)number).ToString() + "�͗����܂���");
#endif
            return;
        }

        // BGM�𗬂�
        AudioSource audio = bgms[number].audioSource;
        audio.spatialBlend = is3D ? 1.0f : 0.0f;
        audio.transform.position = position;
        audio.volume = Mathf.Clamp01(volume);
        audio.Play();
        audio.time = startTime;// Play�̌�ɐݒ�

        bgms[number].isPlaying = true;
        bgms[number].startTime = startTime;
    }

    /// <summary>
    /// SE�𗬂�(�����d�Ȃ炸�A�㏑�������)
    /// </summary>
    /// <param name="number">SoundFxName�̔ԍ�</param>
    /// <param name="position">�ʒu</param>
    /// <param name="volume">����(0-1)</param>
    /// <param name="startTime">�����̊J�n����</param>
    /// <param name="is3D">3D��</param>
    public void PlaySE(int number, Vector3 position, float volume, float startTime, bool is3D)
    {
        // ������΃G���[
        if (audioSourcesPlay[number] == null || audioSourcesPlay[number].clip == null)
        {
#if UNITY_EDITOR
            Debug.Log(((AudioSE)number).ToString() + "�͗����܂���");
#endif
            return;
        }

        // SE�𗬂�
        AudioSource audio = audioSourcesPlay[number];
        audio.spatialBlend = is3D ? 1.0f : 0.0f;
        audio.transform.position = position;
        audio.volume = Mathf.Clamp01(volume);
        audio.Play();
        audio.time = startTime;// Play�̌�ɐݒ�
    }

    /// <summary>
    /// SE�𗬂�(�����d�Ȃ�)
    /// </summary>
    /// <param name="number">SoundFxName�̔ԍ�</param>
    /// <param name="position">�ʒu</param>
    /// <param name="volume">����(0-1)</param>
    /// <param name="startTime">�����̊J�n����</param>
    /// <param name="is3D">3D��</param>
    public void PlayOneShotSE(int number, Vector3 position, float volume, float startTime, bool is3D)
    {
        // ������΃G���[
        if (audioClipsSE[number] == null)
        {
#if UNITY_EDITOR
            Debug.Log(((AudioSE)number).ToString() + "�͗����܂���");
#endif
            return;
        }

        // SE�𗬂�
        AudioSource audio = audioSourcesPlayOneShot[playOneShotIndex];
        audio.clip = audioClipsSE[number];
        audio.spatialBlend = is3D ? 1.0f : 0.0f;
        audio.transform.position = position;
        audio.volume = Mathf.Clamp01(volume);
        audio.Play();
        audio.time = startTime;// Play�̌�ɐݒ�

        audio.gameObject.name = ((AudioSE)number).ToString();

        // audioSourcePlayOneShot�����ԂɎg���Ă���
        playOneShotIndex = (playOneShotIndex + 1) % audioSourcesPlayOneShot.Length;
    }

    #endregion

    #region �����~�߂�

    /// <summary>
    /// BGM���~�߂�
    /// </summary>
    /// <param name="number">BgmName�̔ԍ�</param>
    public void StopBgm(int number)
    {
        bgms[number].audioSource.Stop();
        bgms[number].isPlaying = false;
    }

    /// <summary>
    /// �S�Ă�BGM���~�߂�
    /// </summary>
    public void StopAllBgm()
    {
        for (int i = 0; i < bgms.Length; i++)
        {
            StopBgm(i);
        }
    }

    /// <summary>
    /// �S�Ẳ����~�߂�
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

    #region AudioMixer�̒l���X���C�_�[�ɔ��f

    /// <summary>
    /// AudioMixer�̒l���X���C�_�[�ɔ��f������
    /// </summary>
    /// <param name="sliderMaster">Master�p�̃X���C�_�[</param>
    /// <param name="sliderBgm">BGM�p�̃X���C�_�[</param>
    /// <param name="sliderSE">SE�p�̃X���C�_�[</param>
    public void SetSliderValue(Slider sliderMaster, Slider sliderBgm, Slider sliderSE)
    {
        // AudioMixer�̉��ʎ擾
        audioMixer.GetFloat("Master", out float masterVolume);
        audioMixer.GetFloat("BGM", out float bgmVolume);
        audioMixer.GetFloat("SE", out float seVolume);


        // �������ʐݒ�
        sliderMaster.value = Mathf.Pow(10f, (masterVolume - AUDIO_MIXER_MAX_VOLUME) / 20f);
        sliderBgm.value = Mathf.Pow(10f, (bgmVolume - AUDIO_MIXER_MAX_VOLUME) / 20f);
        sliderSE.value = Mathf.Pow(10f, (seVolume - AUDIO_MIXER_MAX_VOLUME) / 20f);
    }

    #endregion

    #region BGM�̃��[�v�Đ�

    /// <summary>
    /// BGM�����[�v�Đ�����(loop��false�ɂ���Update�ŉ�
    /// </summary>
    private void LoopBgm()
    {
        foreach (Bgm bgm in bgms)
        {
            if (bgm.isPlaying == false) continue;
            if (bgm.audioSource.isPlaying == true) continue;

            // ����Ă���BGM���I�������A�ēx�����n�߂�
            bgm.audioSource.Play();
            bgm.audioSource.time = bgm.startTime;// Play�̌�ɐݒ�
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
    #region Singleton�֌W�Ȃ�

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

    #region �X���C�_�[����AudioMixer�̉��ʂ�ς���

    /// <summary>
    /// �X���C�_�[����AudioMixer��Master�̉��ʂ�ς���
    /// �X���C�_�[��onValueChanged�ɐݒ肷��
    /// </summary>
    /// <param name="value">�X���C�_�[�̒l</param>
    public static void SetAudioMixerMaster(float value)
    {
        instance.module.SetAudioMixerMaster(value);
    }

    /// <summary>
    /// �X���C�_�[����AudioMixer��BGM�̉��ʂ�ς���
    /// �X���C�_�[��onValueChanged�ɐݒ肷��
    /// </summary>
    /// <param name="value">�X���C�_�[�̒l</param>
    public static void SetAudioMixerBGM(float value)
    {
        instance.module.SetAudioMixerBGM(value);
    }

    /// <summary>
    /// �X���C�_�[����AudioMixer��SE�̉��ʂ�ς���
    /// �X���C�_�[��onValueChanged�ɐݒ肷��
    /// </summary>
    /// <param name="value">�X���C�_�[�̒l</param>
    public static void SetAudioMixerSE(float value)
    {
        instance.module.SetAudioMixerSE(value);
    }

    #endregion

    #region ���𗬂�

    /// <summary>
    /// BGM�𗬂�(3D)
    /// </summary>
    /// <param name="sound">enum��BgmName</param>
    /// <param name="position">�ʒu</param>
    /// <param name="volume">����(0-1)</param>
    /// <param name="startTime">�����̊J�n����</param>
    public static void PlayBgm3D(AudioBgm sound, Vector3 position, float volume = 0.5f, float startTime = 0)
    {
        instance.module.PlayBgm((int)sound, position, volume, startTime, true);
    }

    /// <summary>
    /// BGM�𗬂�(2D)
    /// </summary>
    /// <param name="sound">enum��BgmName</param>
    /// <param name="volume">����(0-1)</param>
    /// <param name="startTime">�����̊J�n����</param>
    public static void PlayBgm2D(AudioBgm sound, float volume = 0.5f, float startTime = 0)
    {
        instance.module.PlayBgm((int)sound, Vector3.zero, volume, startTime, false);
    }

    /// <summary>
    /// SE�𗬂�(3D)(�����d�Ȃ炸�A�㏑�������)
    /// </summary>
    /// <param name="sound">enum��SoundFxName</param>
    /// <param name="position">�ʒu</param>
    /// <param name="volume">����(0-1)</param>
    /// <param name="startTime">�����̊J�n����</param>
    public static void PlaySE3D(AudioSE sound, Vector3 position, float volume = 0.5f, float startTime = 0)
    {
        instance.module.PlaySE((int)sound, position, volume, startTime, true);
    }

    /// <summary>
    /// SE�𗬂�(2D)(�����d�Ȃ炸�A�㏑�������)
    /// </summary>
    /// <param name="sound">enum��SoundFxName</param>
    /// <param name="volume">����(0-1)</param>
    /// <param name="startTime">�����̊J�n����</param>
    public static void PlaySE2D(AudioSE sound, float volume = 0.5f, float startTime = 0)
    {
        instance.module.PlaySE((int)sound, Vector3.zero, volume, startTime, false);
    }

    /// <summary>
    /// SE�𗬂�(3D)(�����d�Ȃ�)
    /// </summary>
    /// <param name="sound">enum��SoundFxName</param>
    /// <param name="position">�ʒu</param>
    /// <param name="volume">����(0-1)</param>
    /// <param name="startTime">�����̊J�n����</param>
    public static void PlayOneShotSE3D(AudioSE sound, Vector3 position, float volume = 0.5f, float startTime = 0)
    {
        instance.module.PlayOneShotSE((int)sound, position, volume, startTime, true);
    }

    /// <summary>
    /// SE�𗬂�(2D)(�����d�Ȃ�)
    /// </summary>
    /// <param name="sound">enum��SoundFxName</param>
    /// <param name="volume">����(0-1)</param>
    /// <param name="startTime">�����̊J�n����</param>
    public static void PlayOneShotSE2D(AudioSE sound, float volume = 0.5f, float startTime = 0)
    {
        instance.module.PlayOneShotSE((int)sound, Vector3.zero, volume, startTime, false);
    }

    #endregion

    #region PlayBgm Builder�p�^�[��

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

    #region PlaySE Builder�p�^�[��

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

    #region PlayOneShotSE Builder�p�^�[��

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

    #region �����~�߂�

    /// <summary>
    /// BGM���~�߂�
    /// </summary>
    /// <param name="bgm">BgmName</param>
    public static void StopBgm(AudioBgm bgm)
    {
        instance.module.StopBgm((int)bgm);
    }

    /// <summary>
    /// �S�Ă�BGM���~�߂�
    /// </summary>
    public static void StopAllBgm()
    {
        instance.module.StopAllBgm();
    }

    /// <summary>
    /// �S�Ẳ����~�߂�
    /// </summary>
    public static void StopAllAudio()
    {
        instance.module.StopAllAudio();
    }

    #endregion

    #region AudioMixer�̒l���X���C�_�[�ɔ��f

    /// <summary>
    /// AudioMixer�̒l���X���C�_�[�ɔ��f������
    /// </summary>
    /// <param name="sliderMaster">Master�p�̃X���C�_�[</param>
    /// <param name="sliderBgm">BGM�p�̃X���C�_�[</param>
    /// <param name="sliderSE">SE�p�̃X���C�_�[</param>
    public static void SliderValueChange(Slider sliderMaster, Slider sliderBgm, Slider sliderSE)
    {
        instance.module.SetSliderValue(sliderMaster, sliderBgm, sliderSE);
    }

    #endregion
}