using App.Game.Gameplay;
using App.Game.Gameplay.AI;
using App.Game.WorldBuild;
using System;
using System.Collections;
using UnityEngine;


public class TurnController
{
    public Action OnTurnStarted;
    public Action OnTurnFinished;
    public Action OnVictory;

    readonly BuildUpdater buildUpdater;
    readonly AIUpdater aiUpdater;
    readonly WorldGrid worldGrid;

    public TurnController(WorldGrid worldGrid)
    {
        this.worldGrid = worldGrid;
        buildUpdater = new BuildUpdater();
        aiUpdater = new AIUpdater();
    }

    public void AddBuild(IBuild build)
    {
        buildUpdater.AddBuild(build);
    }

    public void AddAI(IAICharacter aiCharacter)
    {
        aiUpdater.AddAICharacter(aiCharacter);
    }

    public void RemoveBuild(IBuild build)
    {
        buildUpdater.RemoveBuild(build);
    }

    public void RemoveAICharacter(IAICharacter aiCharacter)
    {
        aiUpdater.RemoveCharacter(aiCharacter);
    }

    public void DoTurn()
    {
        buildUpdater.DoTurn();
        aiUpdater.DoTurn();
        if (PathChecker.IsPathValid(worldGrid))
        {
            OnVictory?.Invoke();
            Debug.Log("---------------------------Vitória");
        }
    }

    public void Dispose()
    {
        buildUpdater.Dispose();
        aiUpdater.Dispose();
    }
}
