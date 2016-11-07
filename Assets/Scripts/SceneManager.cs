using UnityEngine;
using System.Collections;

/// <summary>
/// Different types of scenes
/// </summary>
public enum ScenesType
{
    DETECTION,
    QUIT,
    MAIN_MENU,
    FORMULAIRE,
    EVALUATION
}

/// <summary>
/// Manager of the scenes
/// </summary>
public class SceneManager : MonoBehaviour
{
    ScenesType actualScene = ScenesType.MAIN_MENU;

    // Use this for initialization
    void Start()
    {
        if(FindObjectsOfType<SceneManager>().Length > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            EventManager.addActionToEvent<ScenesType>(MyEventTypes.CHANGE_SCENE, changeScene);
        }
    }

    private IEnumerator waitThenQuitApplication()
    {
        yield return new WaitForSeconds(5);
        Application.Quit();
    }

    /// <summary>
    /// Switch between the scenes
    /// </summary>
    /// <param name="newScene"></param>
    public void changeScene(ScenesType newScene)
    {
        actualScene = newScene;

        switch (newScene)
        {
            case ScenesType.MAIN_MENU:
                goToMainMenu();
                break;
            case ScenesType.DETECTION:
                goToDetection();
                break;
            case ScenesType.FORMULAIRE:
                goToFormulaire();
                break;
            case ScenesType.QUIT:
                quitGame();
                break;
            default:
                break;
        }
    }
   
    public void quitGame()
    {
        Application.Quit();
    }

    public void goToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuPrincipal");
    }

    public void goToEvaluation()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("EvaluationScene");
    }

    public void goToFormulaire()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Formulaire");
    }

    public void goToDetection()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("DetectionScene");
    }

    void OnDestroy()
    {
        EventManager.removeActionFromEvent<ScenesType>(MyEventTypes.CHANGE_SCENE, changeScene);
    }
}
