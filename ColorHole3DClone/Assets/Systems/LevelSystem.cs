using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelSystem : Singleton<LevelSystem>
{
    private string scenePrefix = "Level_";
    private int currentLevel = 0;
    private int currentStage = 0;

    private int stageCountPerLevel = 2;

    public UnityEvent OnMovingToNextStageStart = new UnityEvent();
    public UnityEvent OnMovingToNextStageEnd = new UnityEvent();

    public void MoveToNextStage()
    {
        currentStage++;

        if (currentStage % stageCountPerLevel == 0)
        {
            MoveToNextLevel();
        }
        else
        {
            // TODO: Move camera

            OnMovingToNextStageStart?.Invoke();
        }
    }

    private void MoveToNextLevel()
    {
        currentLevel++;
        currentStage = 0;

        try
        {
            SceneManager.LoadScene(scenePrefix + currentLevel.ToString());
        }
        catch (Exception e)
        {
            Debug.LogError("Error caught while changing level with message = " + e.Message);
        }
    }

    public int GetCurrentLevelNumber()
    {
        return currentLevel;
    }

    public int GetCurrentStageNumber()
    {
        return currentStage;
    }
}
