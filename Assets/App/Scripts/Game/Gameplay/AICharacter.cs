using UnityEngine;
using App.Game.Gameplay;

public class AICharacter : TimeEntity
{
    public AICharacter(float updateFrequency) : base(updateFrequency)
    {
        OnUpdate += Update;
    }

    void Update()
    {
        Debug.Log($"Entity {GUID} updated");
    }
}

