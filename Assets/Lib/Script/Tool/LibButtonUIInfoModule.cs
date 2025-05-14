using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.Switch;
using UnityEngine.InputSystem.XInput;
using UnityEngine.UI;

public class LibButtonUIInfoModule : MonoBehaviour, I_UsedControllerHnadler
{
    [SerializeField] ButtonUIData[] buttonUiDatas;
    [SerializeField] GameObject spriteBase;
    [SerializeField] float OBJ_VERTICAL_POS = 200.0f;
    [SerializeField] float OBJ_HOLIZONTAL_MOVE_TIME = 0.2f;
    private List<ButtonUIObj> objs = new List<ButtonUIObj>();
    private List<ButtonUIObj> usedObjs = new List<ButtonUIObj>();

    public void Init()
    {
        LibUsedControllerManager.AddHnadler(this);
    }

    public void OnDest()
    {
        LibUsedControllerManager.RemoveHandler(this);
    }

    private void Update()
    {
        foreach (ButtonUIObj obj in usedObjs)
        {
            obj.Move();
        }
    }

    public void UsedControllerChanged(InputDevice lastUsedDevice)
    {
        foreach (ButtonUIObj obj in objs)
        {
            obj.SpriteChange(lastUsedDevice);
        }
    }

    #region Pop&Remove

    public void PopIcon(ButtonType type)
    {
        ButtonUIData uiData;
        ButtonUIObj obj;
        if (CheckHasButtonType(type, out uiData) == false) return;

        if (IsAlreadyCreate(uiData, out obj) == false) obj = CreateObj(uiData);
        if (IsAlreadyUse(obj) == false) UseObj(obj);
    }

    public void RemoveIcon(ButtonType type)
    {
        ButtonUIData uiData;
        ButtonUIObj obj;
        if (CheckHasButtonType(type, out uiData) == false) return;
        if (IsAlreadyCreate(uiData, out obj) == false) return;
        if (IsAlreadyUse(obj) == false) return;

        UnUseObj(obj);
    }

    private  bool CheckHasButtonType(ButtonType type, out ButtonUIData uiData)
    {
        foreach (ButtonUIData data in buttonUiDatas)
        {
            if (data.type == type)
            {
                uiData = data;
                return true;
            }
        }
        uiData = null;
        return false;
    }

    private bool IsAlreadyCreate(ButtonUIData uiData, out ButtonUIObj buttonObj)
    {
        foreach (ButtonUIObj obj in objs)
        {
            if (obj.data == uiData)
            {
                buttonObj = obj;
                return true;
            }
        }

        buttonObj = null;
        return false;
    }

    private ButtonUIObj CreateObj(ButtonUIData uiData)
    {
        GameObject ins = GameObject.Instantiate(spriteBase, transform);
        Image image = ins.GetComponent<Image>();
        Animator animator = ins.GetComponent<Animator>();
        RectTransform rect = ins.GetComponent<RectTransform>();
        InputDevice device = LibUsedControllerManager.lastUsedDevice;
        ButtonUIObj obj = new ButtonUIObj(uiData, rect, image, animator, device, OBJ_VERTICAL_POS);
        objs.Add(obj);
        return obj;
    }

    private bool IsAlreadyUse(ButtonUIObj buttonObj)
    {
        int index = usedObjs.IndexOf(buttonObj);
        return index != -1;
    }

    private void UseObj(ButtonUIObj obj)
    {

        usedObjs.Add(obj);
        float length = CalAllLength();
        obj.Use(length);
        ResetGoalPos();
    }

    private void UnUseObj(ButtonUIObj obj)
    {
        usedObjs.Remove(obj);
        obj.UnUse();
        ResetGoalPos();
    }

    private void ResetGoalPos()
    {
        float holizontalPos = CalAllLength() / 2 * -1;
        foreach (ButtonUIObj obj in usedObjs)
        {
            obj.SetGoalPos(OBJ_HOLIZONTAL_MOVE_TIME, ref holizontalPos);
        }
    }

    private float CalAllLength()
    {
        float length = 0;
        foreach (ButtonUIObj obj in usedObjs)
        {
            length += obj.data.size;
        }
        return length;
    }
    #endregion
}


[System.Serializable]
public class ButtonSpliteData
{
    [SerializeField] public Sprite sprite;
    [SerializeField] public ControllerType device;
}

public enum ButtonType
{
    ZipLine,
    Climbing,
    Drone,
}

public enum ControllerType
{
    Keyboard,
    Mouse,
    XInputController,
    DualShockGamepad,
    SwitchProControllerHID,
}

public class ButtonUIObj
{
    public ButtonUIData data { get; private set; }
    public RectTransform transform { get; private set; }
    public Image image { get; private set; }
    public Animator animator { get; private set; }
    private Vector3 goalPos = Vector3.zero;
    private float moveTime;

    public ButtonUIObj(ButtonUIData data, RectTransform transform, Image image, Animator animator, InputDevice device, float yPos)
    {
        this.data = data;
        this.transform = transform;
        this.image = image;
        this.animator = animator;
        goalPos = LibVector.Set_Y(goalPos, yPos);

        if (CheckHasButtonType(device, out Sprite sprite) == false) return;

        image.sprite = sprite;

    }

    public void Use(float length)
    {
        goalPos = LibVector.Set_X(goalPos, (length - data.size) / 2);
        transform.anchoredPosition = goalPos;
        animator.SetTrigger("Start");

    }

    public void UnUse()
    {
        animator.SetTrigger("End");
    }

    public void SetGoalPos(float time, ref float holizontalPos)
    {
        moveTime = time;
        goalPos = LibVector.Set_X(goalPos,holizontalPos + data.size / 2);
        holizontalPos += data.size;
    }

    public void Move()
    {
        transform.MoveFocusTime(goalPos, ref moveTime);
    }

    #region SpriteChange
    public void SpriteChange(InputDevice device)
    {
        Sprite sprite;
        if (CheckHasButtonType(device, out sprite) == false) return;
        image.sprite = sprite;
    }
    #endregion

    private bool CheckHasButtonType(InputDevice type, out Sprite uiData)
    {
        int length = data.sprites.Length;
        for (int i = 0; i < length; i++)
        {
            bool isMatch = false;
            switch (type)
            {
                case Keyboard:
                    if (data.sprites[i].device == ControllerType.Keyboard) isMatch = true;
                    break;
                case Mouse:
                    if (data.sprites[i].device == ControllerType.Mouse) isMatch = true;
                    break;
                case XInputController:
                    if (data.sprites[i].device == ControllerType.XInputController) isMatch = true;
                    break;
                case DualShockGamepad:
                    if (data.sprites[i].device == ControllerType.DualShockGamepad) isMatch = true;
                    break;
                case SwitchProControllerHID:
                    if (data.sprites[i].device == ControllerType.SwitchProControllerHID) isMatch = true;
                    break;
            }

            if (isMatch == true)
            {
                uiData = data.sprites[i].sprite;
                return true;
            }
        }

        uiData = null;
        return false;
    }

}

public class LibButtonUIInfoManager
{

    static private LibButtonUIInfoModule _instance;
    static public LibButtonUIInfoModule instance
    {
        get
        {
            if (_instance == null)
            {
                CreateInstance();
            }
            return _instance;
        }

    }

    static private void CreateInstance()
    {
        GameObject pref = (GameObject)Resources.Load("Prefabs/LibButtonUIInfoModule");
        GameObject ins = GameObject.Instantiate(pref);
        GameObject.DontDestroyOnLoad(ins);
        _instance = ins.GetComponent<LibButtonUIInfoModule>();
        _instance.Init();
    }

    static public void PopIcon(ButtonType type)
    {
        instance.PopIcon(type);
    }

    static public void RemoveIcon(ButtonType type)
    {
        instance.RemoveIcon(type);
    }
}