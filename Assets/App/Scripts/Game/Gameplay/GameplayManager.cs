using UnityEngine;
using App.System.Utils;
using App.Game.Data;
using App.Game.WorldBuild;
using App.Game.Gameplay;
using System;

public class GameplayManager : MonoSingleton<GameplayManager>
{
    [SerializeField]
    private GameplayDatasheet gameplayDatasheet;

    [SerializeField]
    PlayerCamera playerCameraPrefab;

    WorldGrid grid;
    PlayerCamera playerCamera;
    CellSelector cellSelector;
    TurnController turnController;


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

    protected override void Init()
    {
        GameObject gridRoot = new GameObject("GridRoot");

        gridRoot.transform.position = Vector2.zero;
        gridRoot.transform.parent = transform;

        turnController = new TurnController();
        grid = new WorldGrid(gameplayDatasheet.GridSize, gameplayDatasheet.TileMapData, gridRoot.transform, Vector2.left * 3);
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
                if (cellSelector.CurrentSelectedCell != null)
                {
                    Cell cell = cellSelector.CurrentSelectedCell;
                    if (cell.IsEditable && cell.Build == null)
                    {
                        BuildData buildData = gameplayDatasheet.BuildDatas[0];
                        cell.CreateBuild(buildData, cellSelector.HoverCells, (entity) =>
                        {
                            turnController.AddBuild((IBuild)entity);

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

            if (key == KeyCode.Space) { 
            
                turnController.DoTurn();
            }
        };

        playerCamera = Instantiate(playerCameraPrefab, transform);
        playerCamera.transform.position = new Vector3(0, 0, -10);
        playerCamera.Init(cellSelector, gameplayDatasheet.PlayerCameraSpd, gameplayDatasheet);
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
