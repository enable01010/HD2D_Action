using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAngleFollower : MonoBehaviour
{
    private void Awake()
    {
        EventBus.Register<ObjectAngleData>(SetData);
    }

    private void OnDestroy()
    {
        EventBus.UnRegister<ObjectAngleData>(SetData);
    }

    /// <summary>
    /// �p�x����K�p���鏈��
    /// </summary>
    /// <param name="data">�I�u�W�F�N�g�p�f�[�^</param>
    public void SetData(ObjectAngleData data)
    {
        transform.eulerAngles = data.angle;
    }
}

/// <summary>
/// �I�u�W�F�N�g�p�̃f�[�^�W��N���X
/// </summary>
public class ObjectAngleData:IEvent
{
    public Vector3 angle;
}