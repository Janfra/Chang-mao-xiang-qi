using UnityEngine;

[CreateAssetMenu(fileName = "Selected Notifier", menuName = "ScriptableObjects/Notifier/Selected Notifier", order = 1)]
public class SO_SelectedNotifier : ScriptableObject
{
    public ISelector Selector { get; private set; }
    public GameObject SelectedObject => Selector?.CurrentSelected?.gameObject;

    public void AssignSelector(ISelector selector)
    {
        if (Selector != null)
        {
            Debug.LogWarning("Selector is already assigned. Ignoring reassignment.");
            return;
        }

        Selector = selector;
    }
}
