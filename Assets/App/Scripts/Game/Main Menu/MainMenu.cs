using App.System.Sound;
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

    [SerializeField]
    Button settingsButton;

    [SerializeField]
    GameObject settingsPopupHolder;
    [SerializeField]
    GameObject credtisPopupHolder;

    [SerializeField]
    Button creditsButton;


    [Header("Settings")]
    [SerializeField]
    Slider bgmSlider;

    [SerializeField]
    Slider sfxSlider;

    [SerializeField]
    Button settingsCloseButton;

    [Header("Credtis")]
    [SerializeField]
    Button credtisCloseButton;

    const string levelSelectionScene = "LevelSelection";

    SoundController soundController;

    private void Awake()
    {
        soundController = SoundController.Instance;
        bgmSlider.value = soundController.BgmVolume;
        sfxSlider.value = soundController.Sfxvolume;
        playButton.onClick.AddListener(() => SceneManager.LoadScene(levelSelectionScene));
        quitButton.onClick.AddListener(() => Application.Quit());
        settingsButton.onClick.AddListener(() => settingsPopupHolder.SetActive(true));
        creditsButton.onClick.AddListener(() => credtisPopupHolder.SetActive(true));
        sfxSlider.onValueChanged.AddListener((volume) => soundController.UpdateSFX(volume));
        bgmSlider.onValueChanged.AddListener((volume) => soundController.UpdateBGM(volume));
        settingsCloseButton.onClick.AddListener(() => settingsPopupHolder.SetActive(false));
        credtisCloseButton.onClick.AddListener(() => credtisPopupHolder.SetActive(false));

    }

}
