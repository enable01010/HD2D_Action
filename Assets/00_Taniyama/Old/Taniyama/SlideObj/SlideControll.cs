using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideControll : MonoBehaviour
{
    [SerializeField] private Transform[] slideObj;
    [SerializeField] private ParallerlPlane[] parallerlObj;
    [SerializeField] private MeshCornerPos[] reposObjs;
    [SerializeField] private CaluculationPos[] calucObj;
    [SerializeField] private MeshCreater[] meshObj;

    private float slide = -15.0f;
    private bool isChange = true;

    [SerializeField, Range(0, 1)] float billboard = 0;

    public void Update()
    {
        if(isChange == true)
        {
            if (slideObj != null)
            {
                for (int i = 0; i < slideObj.Length; i++)
                {
                    Vector3 rot = slideObj[i].eulerAngles;
                    rot.x = slide;
                    slideObj[i].eulerAngles = rot;
                    isChange = false;
                }
            }

            if (parallerlObj != null)
            {
                for (int i = 0; i < parallerlObj.Length; i++)
                {
                    parallerlObj[i].RePos();
                }
            }

            if (reposObjs != null)
            {
                for (int i = 0; i < reposObjs.Length; i++)
                {
                    reposObjs[i].RePos();
                }
            }

            if (calucObj != null)
            {
                for (int i = 0; i < calucObj.Length; i++)
                {
                    calucObj[i].RePos();
                }
            }

            if (meshObj != null)
            {
                for (int i = 0; i < meshObj.Length; i++)
                {
                    meshObj[i].ResetMesh();
                }
            }
        }

        if (billboard != 0)
        {

            if (slideObj != null)
            {
                for (int i = 0; i < slideObj.Length; i++)
                {
                    Vector3 vec = new Vector3(0, 0, -1);
                    vec = Quaternion.Euler(slide, 0, 0) * vec;
                    Vector3 camVec = Camera.main.transform.position - slideObj[i].position;
                    camVec.x = 0;
                    float baseDir = Vector3.Angle(new Vector3(0, 0, -1), camVec);
                    float angle = Vector3.Angle(vec, camVec);
                    angle *= billboard * ((Mathf.Abs(slide) < baseDir && slideObj[i].position.y > Camera.main.transform.position.y) ? (-1) : (1));

                    slideObj[i].eulerAngles = new Vector3(slide + angle, 0, 0);
                }

                if (parallerlObj != null)
                {
                    for (int i = 0; i < parallerlObj.Length; i++)
                    {
                        parallerlObj[i].RePos();
                    }
                }

                if (reposObjs != null)
                {
                    for (int i = 0; i < reposObjs.Length; i++)
                    {
                        reposObjs[i].RePos();
                    }
                }

                if (calucObj != null)
                {
                    for (int i = 0; i < calucObj.Length; i++)
                    {
                        calucObj[i].RePos();
                    }
                }

                if (meshObj != null)
                {
                    for (int i = 0; i < meshObj.Length; i++)
                    {
                        meshObj[i].ResetMesh();
                    }
                }
            }
        }
    }

    public void SlideUp()
    {
        isChange = true;
        slide -= 1.0f;
    }

    public void SlideDown()
    {
        isChange = true;
        slide += 1.0f;
    }
}
