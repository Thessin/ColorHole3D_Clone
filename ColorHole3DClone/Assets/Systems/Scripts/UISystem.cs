using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UITypes
{
    GameOverUI,
    GameWonUI
}

public class UISystem : Singleton<UISystem>
{
    [SerializeField] private GameObject gameOverGO;
    [SerializeField] private GameObject gameWonGO;

    /// <summary>
    /// Show the wanted UI.
    /// </summary>
    /// <param name="uiType"></param>
    public void ShowUI(UITypes uiType)
    {
        switch (uiType)
        {
            case UITypes.GameOverUI:
                Instantiate(gameOverGO);
                break;
            case UITypes.GameWonUI:
                Instantiate(gameWonGO);
                break;
            default:
                break;
        }
    }
}
