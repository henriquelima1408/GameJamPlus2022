using App.Game.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    [SerializeField]
    Button backButton;

    [SerializeField]
    LevelButton levelButtonPrefab;

    [SerializeField]
    Transform contentTransform;

    [SerializeField]
    int levelAmount;

    const string gameplaySceneName = "GameplayScene";

    // Start is called before the first frame update
    void Start()
    {
        backButton.onClick.AddListener(() => SceneManager.LoadScene("Main Menu"));

        for (int i = 0; i < levelAmount; i++)
        {
            var levelButton = Instantiate<LevelButton>(levelButtonPrefab, contentTransform);
            levelButton.Init(i, (index) =>
            {
                LevelController.Instance.SetNextLevel(index);
                SceneManager.LoadScene(gameplaySceneName);
            });
        }
    }

}
