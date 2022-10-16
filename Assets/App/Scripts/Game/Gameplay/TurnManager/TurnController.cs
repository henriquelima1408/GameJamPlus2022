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
    WorldGrid worldGrid;

    public BuildUpdater BuildUpdater => buildUpdater;

    public AIUpdater AiUpdater => aiUpdater;

    public TurnController()
    {

        buildUpdater = new BuildUpdater();
        aiUpdater = new AIUpdater();
    }

    public void Init(WorldGrid worldGrid)
    {
        this.worldGrid = worldGrid;
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
        aiUpdater.DoTurn();
        buildUpdater.DoTurn();        
        if (PathChecker.IsPathValid(worldGrid))
        {
            OnVictory?.Invoke();
            Debug.Log("---------------------------Vitória");
        }

        OnTurnFinished?.Invoke();
    }

    public void Dispose()
    {
        buildUpdater.Dispose();
        aiUpdater.Dispose();
    }
}
