using System.Reflection;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class InspectorLock : MonoBehaviour
{
    [MenuItem("Tools/Inspector Lock %l")]
    private static void LockInspector()
    {
        ActiveEditorTracker tracker = ActiveEditorTracker.sharedTracker;
        tracker.isLocked = !tracker.isLocked;
        tracker.ForceRebuild();
    }
}
#endif