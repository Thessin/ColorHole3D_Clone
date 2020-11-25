using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private bool gameStateDetermined = false;

    /// <summary>
    /// Things to do when the current level is won.
    /// </summary>
    public void LevelWon()
    {
        // TODO: Move to next level, reward player, show won UI etc.

        if (!gameStateDetermined)
        {
            gameStateDetermined = true;


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

            // TODO: Add Level Lost UI
        }
    }

    /// <summary>
    /// Things to do when the game should restart.
    /// </summary>
    public void RestartGame()
    {
        gameStateDetermined = false;

        LevelSystem.Instance.ResetStage();
        PointSystem.Instance.ResetPoints();
    }
}
