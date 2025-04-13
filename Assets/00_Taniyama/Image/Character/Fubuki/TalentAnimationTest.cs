using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalentAnimationTest : MonoBehaviour
{
    [SerializeField] Animator _anim;
    [SerializeField] float angleSpeed = 1.0f;
    bool isLeft = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.A))
        {
            _anim.SetBool("Move", true);
            isLeft = true;
        }
        else if(Input.GetKey(KeyCode.D))
        {
            _anim.SetBool("Move", true);
            isLeft = false;
        }
        else
        {
            _anim.SetBool("Move", false);
        }

        float goalAngle = isLeft ? 0 : 180f;
        Vector3 nowAngle = transform.localEulerAngles;
        nowAngle.y += angleSpeed;
        nowAngle.y = (nowAngle.y > 360f) ? nowAngle.y - 360f : nowAngle.y;
        if(Mathf.Abs(nowAngle.y - goalAngle) < angleSpeed * 1.1f)
        {
            nowAngle.y = goalAngle;
        }
        transform.localEulerAngles = nowAngle;
    }
}
