using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    private bool gameStateDetermined = false;

    private float levelLoadDelay = 2.0f;

    public UnityEvent OnPauseGame = new UnityEvent();
    public UnityEvent OnResumeGame = new UnityEvent();

    /// <summary>
    /// Things to do when the current level is won.
    /// </summary>
    public void LevelWon()
    {
        // TODO: Move to next level, reward player, show won UI etc.

        if (!gameStateDetermined)
        {
            gameStateDetermined = true;

            UISystem.Instance.ShowUI(UITypes.GameWonUI);

            OnPauseGame?.Invoke();

            Sequence seq = DOTween.Sequence();

            seq.InsertCallback(levelLoadDelay, () => {
                LevelSystem.Instance.MoveToNextLevel();
                gameStateDetermined = false;
                OnResumeGame?.Invoke();
            });

            seq.Play();
        }
    }

    /// <summary>
    /// Things to do when the current level is lost.
    /// </summary>
    public void LevelLose()
    {
        if (!gameStateDetermined)
        {
            gameStateDetermined = true;

            CameraSystem.Instance.ShakeCamera();

            UISystem.Instance.ShowUI(UITypes.GameOverUI);

            OnPauseGame?.Invoke();
        }
    }

    /// <summary>
    /// Things to do when the game should restart.
    /// </summary>
    public void RestartGame()
    {
        gameStateDetermined = false;
        OnResumeGame?.Invoke();

        LevelSystem.Instance.RestartLevel();
        PointSystem.Instance.ResetPoints();

        Debug.LogError("ON RESTART GAME");
    }
}
