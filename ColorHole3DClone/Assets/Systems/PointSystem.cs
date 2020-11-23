using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSystem : Singleton<PointSystem>
{
    [SerializeField]
    private PointsPerLevelSO scriptableObject;

    private int Points = 0;

    private int pointsNeededCache = 0;

    private void Start()
    {
        SetPointsNeededToPassLevel();
    }

    public void AddPoints(int pointToAdd)
    {
        Points += pointToAdd;

        Debug.Log("PointSystem updated, new points = " + Points);

        if (Points >= pointsNeededCache)
        {
            // TODO: Move to next stage!
            LevelSystem.Instance.MoveToNextStage();

            SetPointsNeededToPassLevel();

            Debug.Log("MOVE TO NEXT STAGE!");
        }
    }

    private void SetPointsNeededToPassLevel()
    {
        pointsNeededCache = scriptableObject.GetPointsNeeded(LevelSystem.Instance.GetCurrentLevelNumber(), LevelSystem.Instance.GetCurrentStageNumber());
    }
}
