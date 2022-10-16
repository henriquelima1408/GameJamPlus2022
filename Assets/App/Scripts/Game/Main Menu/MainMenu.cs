using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    Button playButton;

    [SerializeField]
    Button quitButton;

    const string levelSelectionScene = "LevelSelection";

    private void Awake()
    {
        playButton.onClick.AddListener(() => SceneManager.LoadScene(levelSelectionScene));
        quitButton.onClick.AddListener(() => Application.Quit());
    }

}
