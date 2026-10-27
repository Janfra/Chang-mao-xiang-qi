using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class KingSingleton : MonoBehaviour
{
    [SerializeField] private SO_TurnNotifier SO_turn;
    public static KingSingleton Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public Vector2 GetKingPos()
    {
        return transform.position;
    }

    public void Endturn()
    {
        Debug.Log("Player Turn Ended");
        SO_turn.TurnManager.RequestEndPlayerTurn();
    }
}
