using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class SceneAnimation : MonoBehaviour
{
    Animator _anim;
    [SerializeField] float FADEIN_TIME = 0.5f;
    [SerializeField] float ANIMATION_TIME;
    [SerializeField] float FADEOUT_TIME = 0.5f;
    [SerializeField] Slider _slider;
    private AsyncOperation async;
    float nowTime = 0;

    public bool isLoading { get; private set; } = false;

    public  void Init()
    {
        _anim = GetComponent<Animator>();
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(int sceneNumbaer)
    {
        StartCoroutine(LoadStart(true, sceneNumbaer,null));
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadStart(false, 0, sceneName));
    }

    IEnumerator LoadStart(bool isNumber,int number,string name)
    {
        nowTime = 0;
        isLoading = true;

        _anim.SetBool("FadeOut", false);
        _anim.SetBool("FadeIn",true);

        //LibSound.PlayOneShotSE(SoundFxName.SceneLoad);

        yield return new WaitForSeconds(FADEIN_TIME);

        if (isNumber == true)
        {
            async = SceneManager.LoadSceneAsync(number);
        }
        else
        {
            async = SceneManager.LoadSceneAsync(name);
        }

        //　読み込みが終わるまで進捗状況をスライダーの値に反映させる
        while (!async.isDone || nowTime < ANIMATION_TIME)
        {
            nowTime += Time.deltaTime;
            float timeRate = nowTime / ANIMATION_TIME;
            float progressVal = Mathf.Clamp01(async.progress / 0.9f);
            float lessValue = (timeRate > progressVal) ? progressVal : timeRate;
            if (_slider != null)
                _slider.value = lessValue;
            yield return null;
        }
        _anim.SetBool("FadeIn", false);
        _anim.SetBool("FadeOut", true);
        yield return new WaitForSeconds(FADEOUT_TIME);
        isLoading = false;
    }
}

public class LibSceneManager
{
    private static LibSceneManager instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new LibSceneManager();
            }
            return m_Instance;
        }
    }

    private static LibSceneManager m_Instance;
    SceneAnimation obj;

    private LibSceneManager()
    {
        GameObject pref = (GameObject)Resources.Load("Prefabs/SceneManagerModule");
        obj = GameObject.Instantiate(pref).GetComponent<SceneAnimation>();
        GameObject.DontDestroyOnLoad(pref);
        obj.Init();
    }

    public static void LoadScene(int sceneNumbaer)
    {
        instance.obj.LoadScene(sceneNumbaer);
    }

    public static void LoadScene(string sceneName)
    {
        instance.obj.LoadScene(sceneName);
    }

    public static bool GetIsLoad()
    {
        return instance.obj.isLoading;
    }
}
