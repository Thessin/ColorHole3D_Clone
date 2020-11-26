using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUIScript : MonoBehaviour
{
    [SerializeField]
    private Button restartButton;

    private void OnEnable()
    {
        restartButton.onClick.AddListener(RestartGame);
    }

    private void OnDisable()
    {
        restartButton.onClick.RemoveListener(RestartGame);
    }

    /// <summary>
    /// Player interaction to restart the game.
    /// </summary>
    private void RestartGame()
    {
        GameManager.Instance.RestartGame();
    }
}
