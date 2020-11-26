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
    private int maxLevel = 3;

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

    /// <summary>
    /// To move to next stage.
    /// </summary>
    public void MoveToNextStage()
    {
        currentStage++;

        if (currentStage % stageCountPerLevel == 0)
        {
            GameManager.Instance.LevelWon();
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

    /// <summary>
    /// To move to next level.
    /// </summary>
    public void MoveToNextLevel()
    {
        currentLevel++;
        currentLevel = currentLevel % maxLevel; // So that current level never goes over bounds.
        currentStage = 0;

        PointSystem.Instance.ResetPoints();

        LoadCurrentLevel();
    }

    /// <summary>
    /// Things to do when a new scene is loaded.
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
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
            Debug.LogWarning("Error occured while trying to find HoleScript type with message = " + e.Message);
        }
    }

    /// <summary>
    /// Load the current level.
    /// </summary>
    private void LoadCurrentLevel()
    {
        try
        {
            SceneManager.LoadScene(scenePrefix + currentLevel.ToString());
        }
        catch (Exception e)
        {
            Debug.LogWarning("Error caught while changing level with message = " + e.Message);
        }
    }

    /// <summary>
    /// Returns current level number.
    /// </summary>
    /// <returns></returns>
    public int GetCurrentLevelNumber()
    {
        return currentLevel;
    }

    /// <summary>
    /// Returns current stage number.
    /// </summary>
    /// <returns></returns>
    public int GetCurrentStageNumber()
    {
        return currentStage;
    }

    /// <summary>
    /// Restart the level.
    /// </summary>
    public void RestartLevel()
    {
        currentStage = 0;

        LoadCurrentLevel();
    }
}
