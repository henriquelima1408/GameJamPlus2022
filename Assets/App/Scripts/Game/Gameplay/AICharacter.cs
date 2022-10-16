using App.Game.Gameplay;
using App.Game.Gameplay.AI;
using App.Game.WorldBuild;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AICharacter : IAICharacter
{
    readonly string guid;
    readonly TurnController turnController;
    readonly WorldGrid worldGrid;
    GameObject self;

    Cell currentCell;

    Vector2Int dir = Vector2Int.up;

    public AICharacter(TurnController turnController, WorldGrid worldGrid, Cell currentCell, GameObject self)
    {
        guid = Guid.NewGuid().ToString();
        this.turnController = turnController;
        this.worldGrid = worldGrid;
        this.currentCell = currentCell;
        this.self = self;
    }

    public string GUID => guid;


    Action<IEntity> onUpdate;
    Action<IEntity> onDestroy;
    Action<IEntity> onBuild;
    Action<IEntity> onTakeDamage;
    public Action<IEntity> OnUpdate { get => onUpdate; set => onUpdate = value; }
    public Action<IEntity> OnDestroy { get => onDestroy; set => onDestroy = value; }
    public Action<IEntity> OnBuild { get => onBuild; set => onBuild = value; }
    public Action<IEntity> OnTakeDamage { get => onTakeDamage; set => onTakeDamage = value; }


    public void Destroy()
    {
        self.gameObject.SetActive(false);
        onDestroy?.Invoke(this);
    }

    public void Update()
    {

        Cell c = null;
        bool canContinueWithDir = true;
        var randomDir = dir;
        int maxIteration = 10;

        while (maxIteration > 0)
        {
            maxIteration--;
            if (!canContinueWithDir)
            {
                var r = UnityEngine.Random.Range(0, 4);

                if (r == 0)
                {
                    randomDir = Vector2Int.right;
                }
                else if (r == 1)
                {
                    randomDir = Vector2Int.left;
                }
                else if (r == 2)
                {
                    randomDir = Vector2Int.up;
                }
                else
                {
                    randomDir = Vector2Int.down;
                }
            }

            var tc = worldGrid.GetCell((currentCell.CellPos + randomDir));


            if (tc == null || tc.IsStatic || worldGrid.DestinationCells.Contains(tc))
            {
                canContinueWithDir = false;
                continue;
            }

            bool ExistSomeoneInDesiredCell = false;
            foreach (var aichracter in turnController.AiUpdater.AiCharacters.Values)
            {
                var character = (AICharacter)aichracter;

                if (character != null && character.currentCell != null && character.currentCell.CellPos == tc.CellPos)
                {

                    if (aichracter != this)
                    {
                        canContinueWithDir = false;
                        ExistSomeoneInDesiredCell = true;
                        break;
                    }
                }

            }

            if (ExistSomeoneInDesiredCell)
                continue;


            if (tc.Build != null)
            {

                var build = tc.Build;
                var cells = build.CellsInArea;

                foreach (var cell in cells)
                {
                    if (cell.IsEditable)
                    {
                        build.Destroy();
                        Destroy();
                        break;
                    }
                }

                continue;
            }

            c = tc;

        }


        dir = randomDir;
        currentCell = c ?? currentCell;
        self.transform.position = currentCell.Self.transform.position;

    }
}

