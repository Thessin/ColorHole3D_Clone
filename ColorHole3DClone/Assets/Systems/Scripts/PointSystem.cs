using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PointsUpdate : UnityEvent<int> { }

public class PointSystem : Singleton<PointSystem>
{
    [SerializeField]
    private PointsPerLevelSO scriptableObject;

    private int Points = 0;
    public PointsUpdate OnPointsUpdate = new PointsUpdate();

    private int pointsNeededCache = 0;

    private void Start()
    {
        SetPointsNeededToPassStage();

        LevelSystem.Instance.OnMovingToNextStageStart.AddListener(ResetPoints);
    }

    private void OnDisable()
    {
        LevelSystem.Instance.OnMovingToNextStageStart.RemoveListener(ResetPoints);
    }

    /// <summary>
    /// Add points.
    /// </summary>
    /// <param name="pointToAdd"></param>
    public void AddPoints(int pointToAdd)
    {
        Points += pointToAdd;
        OnPointsUpdate?.Invoke(Points);

        if (Points >= pointsNeededCache)
        {
            LevelSystem.Instance.MoveToNextStage();

            Debug.Log("MOVE TO NEXT STAGE!");
        }
    }

    /// <summary>
    /// Set the points needed to pass the stage.
    /// </summary>
    private void SetPointsNeededToPassStage()
    {
        try
        {
            pointsNeededCache = scriptableObject.GetPointsNeeded(LevelSystem.Instance.GetCurrentLevelNumber(), LevelSystem.Instance.GetCurrentStageNumber());

            Debug.LogWarning("RESET POINTS ON SET POINTS NEEDED pointsNeededCache= " + pointsNeededCache);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Error caught while trying to appoint pointsNeededCache with scriptableObject with message = " + e.Message);
        }
    }

    /// <summary>
    /// Resets the current points.
    /// </summary>
    public void ResetPoints()
    {
        Points = 0;

        SetPointsNeededToPassStage();

        Debug.LogWarning("RESET POINTS");
    }

    public int GetNeededPointsForStage()
    {
        return pointsNeededCache;
    }
}
