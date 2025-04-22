using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalentAnimationTestAll : MonoBehaviour
{
    [SerializeField] GameObject front;
    [SerializeField] GameObject back;

    private void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            front.SetActive(false);
            back.SetActive(true);
        }
        else if(Input.GetKey(KeyCode.S))
        {
            front.SetActive(true);
            back.SetActive(false);
        }
    }
}
