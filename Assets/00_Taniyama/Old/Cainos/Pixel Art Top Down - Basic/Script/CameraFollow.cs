using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    //let camera follow target
    public class CameraFollow : MonoBehaviour
    {
        public Transform target;
        public float lerpSpeed = 1.0f;

        private Vector3 offset;

        private Vector3 targetPos;

        private void Start()
        {
            if (target == null) return;

            offset = transform.position - target.position;
        }

        private void Update()
        {
            if (target == null) return;

            targetPos = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
        }

        public void PosUp()
        {
            offset.y += 0.2f;
        }

        public void PosDown()
        {
            offset.y -= 0.2f;
        }

        public void RotUp()
        {
            Vector3 rot = transform.eulerAngles;
            rot.x -= 1.0f;
            transform.eulerAngles = rot;
        }

        public void RotDown()
        {
            Vector3 rot = transform.eulerAngles;
            rot.x += 1.0f;
            transform.eulerAngles = rot;
        }

        public void SpeedUp()
        {
            lerpSpeed += 0.1f;
            if(lerpSpeed < 0)
            {
                lerpSpeed = 0;
            }
        }

        public void SpeedDown()
        {
            lerpSpeed -= 0.1f;
        }
    }
}
