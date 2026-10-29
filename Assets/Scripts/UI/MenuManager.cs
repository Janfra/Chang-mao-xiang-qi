using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private SO_menu _menu;
    [SerializeField] private GameObject Tutorial;
    [SerializeField] private GameObject Deathscreen;
    [SerializeField] private GameObject endturn;
    private void Start()
    {
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

    public void DisplayDeath()
    {
        endturn.SetActive(false);
        Deathscreen.SetActive(true);
    }

    public void Reset()
    {
        SceneManager.LoadScene("MainScene");
    }

}
