using App.Game.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityFx.Async.Promises;

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

    [SerializeField]
    AudioClip menuSound;

    const string levelSelectionScene = "LevelSelection";

    //SoundController soundController;

    private void Awake()
    {
        //soundController = SoundController.Instance;
        //bgmSlider.value = soundController.BgmVolume;
        //sfxSlider.value = soundController.Sfxvolume;
        playButton.onClick.AddListener(() => SceneManager.LoadScene(levelSelectionScene));
        quitButton.onClick.AddListener(() => Application.Quit());
        settingsButton.onClick.AddListener(() => settingsPopupHolder.SetActive(true));
        creditsButton.onClick.AddListener(() => credtisPopupHolder.SetActive(true));
        // sfxSlider.onValueChanged.AddListener((volume) => soundController.UpdateSFX(volume));
        // bgmSlider.onValueChanged.AddListener((volume) => soundController.UpdateBGM(volume));
        settingsCloseButton.onClick.AddListener(() => settingsPopupHolder.SetActive(false));
        credtisCloseButton.onClick.AddListener(() => credtisPopupHolder.SetActive(false));


        Services.Instance.GetService<ISoundService>().Then((soundService) => soundService.PlayBGM(menuSound, new SoundDetails(true, false, 0, 0))).Catch((e) => Debug.LogException(e));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {

            Services.Instance.GetService<SoundService>().Then((soundService) => soundService.PauseBGM(true));
        }
        if (Input.GetKeyDown(KeyCode.S))
        {

            Services.Instance.GetService<SoundService>().Then((soundService) => soundService.PauseBGM(false));
        }
        if (Input.GetKeyDown(KeyCode.D))
        {

            Services.Instance.GetService<SoundService>().Then((soundService) => soundService.StopBGM());
        }
    }

}
