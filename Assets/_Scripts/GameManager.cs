using UnityEngine;
using FiveBabbittGames;
using UnityEngine.SceneManagement;

/// <summary>
/// GameManager
/// </summary>
public class GameManager : Singleton<GameManager>
{
    
    
    protected override void Awake()
    {
        base.Awake();

    }

    public void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadLevel(int sceneId) 
    {
        SceneManager.LoadScene(sceneId);
    }

    public void NextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex >= SceneManager.sceneCountInBuildSettings)
        {
            LoadLevel("Main Menu");
            return;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
