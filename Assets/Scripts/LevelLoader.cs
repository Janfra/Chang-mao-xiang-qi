using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public string LevelName;
    public bool IsLoading { get; private set; } 

    public void LoadLevel()
    {
        if (IsLoading)
        {
            return;
        }

        StartCoroutine(AsyncLoadScene());
    }

    private IEnumerator AsyncLoadScene()
    {
        IsLoading = true;
        var loadOperation = SceneManager.LoadSceneAsync(LevelName); // Can later add fancy loading stuff
        while (!loadOperation.isDone)
        {
            yield return null;
        }

        IsLoading = false;
    }
}
