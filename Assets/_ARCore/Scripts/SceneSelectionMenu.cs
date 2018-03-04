using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneSelectionMenu : MonoBehaviour
{
    public Button scene1Button;
    public Button scene2Button;

    private void Start()
    {
        scene1Button.onClick.AddListener(delegate { UnityEngine.SceneManagement.SceneManager.LoadScene(1); });
        scene2Button.onClick.AddListener(delegate { UnityEngine.SceneManagement.SceneManager.LoadScene(2); });
    }
}
