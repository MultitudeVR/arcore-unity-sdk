using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImmortalCanvasController : MonoBehaviour
{
    public Button quitButton;
    public static bool didLoadOnce = false;

    private void Start()
    {
        if (didLoadOnce)
            Destroy(gameObject);
        else
        {
            quitButton.onClick.AddListener(QuitButtonClick);
            DontDestroyOnLoad(gameObject);
            didLoadOnce = true;
        }
    }

    private void QuitButtonClick()
    {
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0)
        {
            Application.Quit();
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
    private void OnApplicationQuit()
    {
        didLoadOnce = false;
    }
}
