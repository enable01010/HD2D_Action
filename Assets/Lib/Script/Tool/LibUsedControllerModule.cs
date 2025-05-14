using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.Switch;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.XInput;

public class LibUsedControllerModule : MonoBehaviour
{
    private List<I_UsedControllerHnadler> handlerList = new List<I_UsedControllerHnadler>();
    public InputDevice lastUsedDevice { get; private set; }

    public void Init()
    {
        InputSystem.onAnyButtonPress.CallOnce(ctrl => {
            lastUsedDevice = ctrl.device;
        });
    }

    private void Update()
    {
        InputSystem.onAnyButtonPress.CallOnce(AnyButtonPress);   
    }

    private void AnyButtonPress(InputControl ctrl)
    {
        InputDevice usingDevice = ctrl.device;
        if (IsDeviceEnableDvice(ctrl.device) == false) return;
        if (ctrl.device == lastUsedDevice) return;

        lastUsedDevice = ctrl.device;
        foreach (I_UsedControllerHnadler handler in handlerList)
        {
            handler.UsedControllerChanged(lastUsedDevice);
        }
    }

    private bool IsDeviceEnableDvice(InputDevice usingDevice)
    {
        switch(usingDevice)
        {
            case Keyboard:
            case Mouse:
            case XInputController:
            case DualShockGamepad:
            case SwitchProControllerHID:
                return true;
            default:
                return false;
        }
    }

    public void AddHnadler(I_UsedControllerHnadler handler)
    {
        handlerList.Add(handler);
    }

    public void RemoveHandler(I_UsedControllerHnadler handler)
    {
        handlerList.Remove(handler);
    }
}

public class LibUsedControllerManager
{

    static private LibUsedControllerModule _instance;
    static public LibUsedControllerModule instance
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
    static public InputDevice lastUsedDevice { get { return instance.lastUsedDevice; } }

    static private void CreateInstance()
    {
        GameObject pref = (GameObject)Resources.Load("Prefabs/LibUsedControllerModule");
        GameObject ins = GameObject.Instantiate(pref);
        GameObject.DontDestroyOnLoad(ins);
        _instance = ins.GetComponent<LibUsedControllerModule>();
        _instance.Init();
    }

    static public void AddHnadler(I_UsedControllerHnadler handler)
    {
        instance.AddHnadler(handler);
    }

    static public void RemoveHandler(I_UsedControllerHnadler handler)
    {
        instance.RemoveHandler(handler);
    }

}

public interface I_UsedControllerHnadler
{
    public void UsedControllerChanged(InputDevice lastUsedDevice);
}








