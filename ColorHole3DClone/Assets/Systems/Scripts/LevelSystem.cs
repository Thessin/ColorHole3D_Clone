using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LevelSystem : Singleton<LevelSystem>
{
    private readonly string preloadString = "_PRELOAD_SCENE";

    private string scenePrefix = "Level_";
    private int currentLevel = 0;
    private int currentStage = 0;

    private int stageCountPerLevel = 2;

    private float stageChangeDuration = 7.0f;
    private float holeToMiddleDuration = 1.5f;

    public UnityEvent OnMovingToNextStageStart = new UnityEvent();
    public UnityEvent OnMovingToNextStageEnd = new UnityEvent();

    private HoleScript hole;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += On_SceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= On_SceneLoaded;
    }

    public void MoveToNextStage()
    {
        currentStage++;

        if (currentStage % stageCountPerLevel == 0)
        {
            MoveToNextLevel();
        }
        else
        {
            Sequence seq = DOTween.Sequence();

            seq.Append(hole.MoveToMiddle(holeToMiddleDuration))
                .Append(CameraSystem.Instance.MoveCameraToNextStage(stageChangeDuration))
                .Insert(holeToMiddleDuration, hole.MoveToNext(stageChangeDuration))
                .OnStart(() => OnMovingToNextStageStart?.Invoke())
                .OnComplete(() => OnMovingToNextStageEnd?.Invoke());

            seq.Play();
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

    private void On_SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == preloadString)    // If preload scene is loaded
            SceneManager.LoadScene(1);      // Load first level

        try
        {
            hole = FindObjectOfType<HoleScript>();
        }
        catch(Exception e)
        {
            Debug.LogError("Error occured while trying to find HoleScript type with message = " + e.Message);
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

    public void ResetStage()
    {
        currentStage = 0;
    }
}
