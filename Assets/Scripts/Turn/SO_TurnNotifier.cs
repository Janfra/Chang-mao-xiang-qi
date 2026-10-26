using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Turn Notifier", menuName = "ScriptableObjects/Notifier/Turn Notifier")]
public class SO_TurnNotifier : ScriptableObject
{
    public ITurnManager TurnManager { get; private set; }
    public event UnityAction OnManagerSet;

    private void OnDisable()
    {
        OnManagerSet = null;
    }

    private void OnEnable()
    {
        OnManagerSet = null;
    }

    public void AssignManager(ITurnManager turnManager)
    {
        TurnManager = turnManager;
        if (TurnManager == null)
        {
            return;
        }

        OnManagerSet?.Invoke();
        OnManagerSet = null;
    }
}
