using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[��ǂ�������J����
/// </summary>
public class PlayerFollowedCamera : MonoBehaviour
{
    public Transform target;
    public float lerpSpeed = 1.0f;

    private Vector3 offset;

    private void Awake()
    {
        EventBus.Register<CameraAngleData>(SetData);
    }

    private void Update()
    {
        Vector3 targetPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
    }

    private void OnDestroy()
    {
        EventBus.UnRegister<CameraAngleData>(SetData);
    }

    /// <summary>
    /// �p�x����K�p���鏈��
    /// </summary>
    /// <param name="data">�J�����p�f�[�^</param>
    public void SetData(CameraAngleData data)
    {
        offset = data.offset;
        transform.eulerAngles = data.angle;
    }
}

/// <summary>
/// �J�����֌W�̃f�[�^�W��N���X
/// </summary>
[System.Serializable]
public class CameraAngleData : IEvent
{
    public Vector3 offset;
    public Vector3 angle;
}