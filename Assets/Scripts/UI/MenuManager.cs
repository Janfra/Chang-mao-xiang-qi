using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private SO_TurnNotifier SO_turn;
    [SerializeField] private SO_menu _menu;
    [SerializeField] private GameObject Tutorial;
    [SerializeField] private GameObject Deathscreen;
    [SerializeField] private GameObject endturn;
    [SerializeField] private TextMeshProUGUI turnCounterText;

    private void Start()
    {
        SO_turn.TurnManager.OnPlayerTurn += UpdateTurnCounter;
        if (_menu.Tutorialised == false)
        {
            _menu.Tutorialised = true;
            endturn.SetActive(false);
            Deathscreen.SetActive(false);
        }
        else
        {
            Tutorial.SetActive(false);
            Deathscreen.SetActive(false);
        }

        var king = KingSingleton.Instance;
        if (king)
        {
            king.OnKingDied += DisplayDeath;
        }
    }

    private void OnDestroy()
    {
        var king = KingSingleton.Instance;
        if (king)
        {
            king.OnKingDied -= DisplayDeath;
        }
    }

    public void UpdateTurnCounter()
    {
        turnCounterText.text = "" + SO_turn.TurnManager.GetTurnNumb();
    }


    public void DisplayDeath()
    {
        endturn.SetActive(false);
        Deathscreen.SetActive(true);
    }

    public void Reset()
    {
        SceneManager.LoadScene("FirstScene");
    }

}
