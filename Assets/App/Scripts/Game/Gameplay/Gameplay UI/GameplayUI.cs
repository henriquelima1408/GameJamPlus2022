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
    [SerializeReference] Image characterPortraitImg;
    [SerializeReference] Button DoTurnButton;

    int currentTurnCount = 1;
    LevelData levelData;
    GameplayManager gameplayManager;

    public void Init(GameplayManager gameplayManager, LevelData levelData)
    {
        this.gameplayManager = gameplayManager;
        this.levelData = levelData;
        turnCountText.text = $"{currentTurnCount} / {levelData.MaxTurnCount}";
        DoTurnButton.onClick.AddListener(() => gameplayManager.DoTurn());

        for (int i = 0; i < levelData.BuildDataInfo.Length; i++)
        {
            var b = Instantiate<BuildButton>(buildButtonPrefab, buildButtonContentTransform);
            b.GetComponentInChildren<Text>().text = levelData.BuildDataInfo[i].BuildData.Id;
            b.Init(i, (index) =>
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
