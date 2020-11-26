using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreCanvasScript : MonoBehaviour
{
    [SerializeField] private Slider firstStageSlider;
    [SerializeField] private Slider secondStageSlider;

    private Slider currentSlider;

    [SerializeField] private TextMeshProUGUI currentLevelText;
    [SerializeField] private TextMeshProUGUI nextLevelText;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SetInitial;
        PointSystem.Instance.OnPointsUpdate.AddListener(SetSliderValue);
        LevelSystem.Instance.OnMovingToNextStageStart.AddListener(ChangeCurrentSlider);
        LevelSystem.Instance.OnMovingToNextStageEnd.AddListener(SetCurrentSliderMaxValue);
    }

    private void Start()
    {
        currentSlider = firstStageSlider;
        SetCurrentSliderMaxValue();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SetInitial;
        PointSystem.Instance.OnPointsUpdate.RemoveListener(SetSliderValue);
        LevelSystem.Instance.OnMovingToNextStageStart.RemoveListener(ChangeCurrentSlider);
        LevelSystem.Instance.OnMovingToNextStageEnd.RemoveListener(SetCurrentSliderMaxValue);
    }

    /// <summary>
    /// Set the things to be done when the level is initialised.
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void SetInitial(Scene scene, LoadSceneMode mode)
    {
        int currentLevel = 0;

        currentLevel = LevelSystem.Instance.GetCurrentLevelNumber();

        currentLevelText.text = currentLevel.ToString();
        nextLevelText.text = (currentLevel + 1).ToString();
    }

    /// <summary>
    /// Change the currentSlider variable when moving from a stage ends.
    /// </summary>
    private void ChangeCurrentSlider()
    {
        currentSlider = secondStageSlider;
    }

    /// <summary>
    /// Set the current slider's max value.
    /// </summary>
    private void SetCurrentSliderMaxValue()
    {
        currentSlider.maxValue = PointSystem.Instance.GetNeededPointsForStage();
    }

    /// <summary>
    /// Set the currentSlider's value.
    /// </summary>
    /// <param name="currentPoints"></param>
    private void SetSliderValue(int currentPoints)
    {
        currentSlider.value = currentPoints;
    }
}
