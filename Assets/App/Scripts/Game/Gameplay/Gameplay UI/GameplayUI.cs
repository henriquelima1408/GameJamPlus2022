using App.System.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] Text turnCountText;
    [SerializeField] BuildButton buildButtonPrefab;
    [SerializeField] Transform buildButtonContentTransform;
    [SerializeField] Image characterPortraitImg;
    [SerializeField] Button DoTurnButton;
    [SerializeField] Button pauseButton;
    [SerializeField] GameObject pauseButtonHolder;
    [Header("PausePopUp")]

    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Button resumeLevelButton;
    [SerializeField] Button restartLevelButton;
    [SerializeField] Button quitButton;


    int currentTurnCount = 1;
    LevelData levelData;
    GameplayManager gameplayManager;
    //SoundController soundController;

    public void Init(GameplayManager gameplayManager, LevelData levelData)
    {
        this.gameplayManager = gameplayManager;
        //soundController = SoundController.Instance;
        this.levelData = levelData;
        turnCountText.text = $"{currentTurnCount} / {levelData.MaxTurnCount}";
        DoTurnButton.onClick.AddListener(() => gameplayManager.DoTurn());
        pauseButton.onClick.AddListener(() => pauseButtonHolder.SetActive(true));

        //bgmSlider.value = soundController.BgmVolume;
        //sfxSlider.value = soundController.Sfxvolume;

        //bgmSlider.onValueChanged.AddListener((volume) => soundController.UpdateBGM(volume));
        //sfxSlider.onValueChanged.AddListener((volume) => soundController.UpdateSFX(volume));

        resumeLevelButton.onClick.AddListener(() => pauseButtonHolder.SetActive(false));
        restartLevelButton.onClick.AddListener(() => gameplayManager.ResetLevel());
        quitButton.onClick.AddListener(() => gameplayManager.Quit());


        for (int i = 0; i < levelData.BuildDataInfo.Length; i++)
        {
            var b = Instantiate<BuildButton>(buildButtonPrefab, buildButtonContentTransform);
            b.Init(i, levelData.BuildDataInfo[i].BuildData.BuildSprite, (index) =>
            {
                if (gameplayManager.IsPossibleToSelectBuild(levelData.BuildDataInfo[index].BuildData.Id))
                {
                    gameplayManager.SetBuildData(levelData.BuildDataInfo[index].BuildData);
                }
            });
        }
    }

    public void SetCurrentTurnCount(int value)
    {

        currentTurnCount = value + 1;
    }

}
