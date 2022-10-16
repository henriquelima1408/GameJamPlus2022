using UnityEngine;
using App.System.Utils;
using App.Game.Data;
using App.Game.WorldBuild;
using App.Game.Gameplay;
using System;
using System.Collections.Generic;

public class GameplayManager : MonoSingleton<GameplayManager>
{
    [SerializeField]
    GameplayDatasheet gameplayDatasheet;

    [SerializeField]
    GameplayUI gameplayUI;

    [SerializeField]
    PlayerCamera playerCameraPrefab;

    WorldGrid grid;
    PlayerCamera playerCamera;
    CellSelector cellSelector;
    TurnController turnController;

    BuildData currentSelectedBuildData;


    Dictionary<string, int> buildDataAllowedUse = new Dictionary<string, int>();
    LevelData levelData;

    //TODO Remove it: Just for test
    Action<KeyCode> OnInput;

    public WorldGrid Grid
    {
        get => grid;
    }
    public PlayerCamera PlayerCamera
    {
        get => playerCamera;
    }
    public BuildData CurrentSelectedBuildData { get => currentSelectedBuildData; }
    public void SetBuildData(BuildData buildData)
    {
        currentSelectedBuildData = buildData;
    }

    void Awake()
    {
        Instance = this;
    }

    protected override void Dispose()
    {
        grid = null;
        playerCamera = null;
        gameplayDatasheet = null;
        cellSelector = null;

        turnController.Dispose();
        turnController = null;

    }


    public void DoTurn() {

        turnController.DoTurn();
    }
    protected override void Init()
    {
        levelData = LevelController.Instance.LevelData;

        GameObject gridRoot = new GameObject("GridRoot");

        gridRoot.transform.position = Vector2.zero;
        gridRoot.transform.parent = transform;

        turnController = new TurnController();
        grid = new WorldGrid(gameplayDatasheet.TileMapDatas, gridRoot.transform, Vector2.left * 3);
        cellSelector = new CellSelector(grid);

        //TODO: Remove it. Its here just to test
        cellSelector.OnSelectedCellChanged += (previous, current, hoverCells) =>
        {




        };

        //TODO: Remove it. Its here just to test
        OnInput += (key) =>
        {
            if (key == KeyCode.Mouse0)
            {
                if (UIUtils.IsPointerOverUIElement())
                {
                    cellSelector.DeselectCell();
                    return;
                }

                if (cellSelector.CurrentSelectedCell != null)
                {
                    Cell cell = cellSelector.CurrentSelectedCell;
                    if (cell.IsEditable && cell.Build == null)
                    {
                        BuildData buildData = currentSelectedBuildData;
                        cell.CreateBuild(buildData, cellSelector.HoverCells, (entity) =>
                        {
                            var build = (IBuild)entity;
                            turnController.AddBuild(build);
                            buildDataAllowedUse[build.BuildData.Id]--;
                            currentSelectedBuildData = null;

                        });

                        turnController.DoTurn();
                    }
                }
            }

            if (key == KeyCode.Mouse1)
            {
                if (cellSelector.CurrentSelectedCell != null)
                {
                    Cell cell = cellSelector.CurrentSelectedCell;
                    if (cell.Build != null)
                    {
                        cell.RemoveBuild();
                    }
                }
            }

            if (key == KeyCode.Space)
            {

                turnController.DoTurn();
            }
        };

        playerCamera = Instantiate(playerCameraPrefab, transform);

        foreach (var buildDataInfo in levelData.BuildDataInfo)
        {
            buildDataAllowedUse[buildDataInfo.BuildData.Id] = buildDataInfo.UseCount;
        }
        playerCamera.Init(this, cellSelector, gameplayDatasheet.PlayerCameraSpd);
        gameplayUI.Init(this, levelData);
    }

    public bool IsPossibleToSelectBuild(string buildID)
    {
        return buildDataAllowedUse[buildID] > 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            OnInput?.Invoke(KeyCode.Mouse0);
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            OnInput?.Invoke(KeyCode.Mouse1);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnInput?.Invoke(KeyCode.Space);
        }
    }

}
