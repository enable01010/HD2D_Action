using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

[InitializeOnLoad]
public static class ActiveCheckbox
{
    private const int WIDTH = 16;
    private const int RECT_POS_X = 32;

    static ActiveCheckbox()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnGUI;
    }

    private static void OnGUI(int instanceID, Rect selectionRect)
    {
        GameObject gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

        if (gameObject == null) return;

        Rect rect = selectionRect;

        //rect.x = rect.xMax - WIDTH;
        //rect.width = WIDTH;
        rect.x = RECT_POS_X;
        rect.width = WIDTH;

        bool isActive = GUI.Toggle(rect, gameObject.activeSelf, string.Empty);

        if (isActive == gameObject.activeSelf) return;

        gameObject.SetActive(isActive);
    }
}
#endif 