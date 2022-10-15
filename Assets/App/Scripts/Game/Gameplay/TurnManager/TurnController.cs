using App.Game.Gameplay;
using App.Game.Gameplay.AI;
using System;
using System.Collections;
using UnityEngine;


public class TurnController
{
    public Action OnTurnStarted;
    public Action OnTurnFinished;

    readonly BuildUpdater buildUpdater;
    readonly AIUpdater aiUpdater;

    public TurnController()
    {
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
    }

    public void Dispose()
    {
        buildUpdater.Dispose();
        aiUpdater.Dispose();
    }
}
