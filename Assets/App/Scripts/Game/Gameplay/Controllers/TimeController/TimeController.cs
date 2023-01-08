using System;
using App.Game.Services;
using System.Collections;
using UnityEngine;

public class TimeController 
{
    public event Action OnTimeOver;
    public event Action<float> OnTimeRemoved;
    public event Action<float> OnTimeUpdated;

    readonly float timeUpdateFrequency;
    readonly ICoroutineService coroutineService;

    float totalTime;
    string updateCoroutineID;

    public TimeController(ICoroutineService coroutineService, float totalTime)
    {
        this.coroutineService = coroutineService;
        this.totalTime = totalTime;
        this.timeUpdateFrequency = Time.deltaTime;
    }

    public void DoStart()
    {
        updateCoroutineID = coroutineService.AddCoroutine(DoUpdate()).CoroutineID;
    }

    public void DoStop()
    {
        coroutineService.RemoveCoroutine(updateCoroutineID);
    }

    public void RemoveTime(float amount) {

        var delta = totalTime - Mathf.Clamp(totalTime - amount, 0, float.MaxValue);
        totalTime = Mathf.Clamp(totalTime - amount, 0, float.MaxValue);

        OnTimeRemoved?.Invoke(amount);
    }

    IEnumerator DoUpdate()
    {
        var startTime = Time.time;
        while (totalTime > 0)
        {
            yield return new WaitForSeconds(timeUpdateFrequency);
            var timeDiff = Time.time - startTime;

            var delta = totalTime - Mathf.Clamp(totalTime - timeDiff, 0, float.MaxValue);
            totalTime = Mathf.Clamp(totalTime - timeDiff, 0, float.MaxValue);

            OnTimeUpdated?.Invoke(totalTime);
        }

        OnTimeOver?.Invoke();
        yield return null;
    }

    public void Dispose()
    {
        OnTimeOver = null;
        OnTimeRemoved = null;
        OnTimeUpdated = null;
    }
}
