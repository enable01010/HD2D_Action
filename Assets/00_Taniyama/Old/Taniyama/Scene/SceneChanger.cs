using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void OnClick(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }
}
