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
    }

    private void Start()
    {
        firstStageSlider.maxValue = PointSystem.Instance.GetNeededPointsForStage();
        Debug.LogError("SETTING MAX VAL => " + firstStageSlider.maxValue);
        currentSlider = firstStageSlider;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SetInitial;
        PointSystem.Instance.OnPointsUpdate.RemoveListener(SetSliderValue);
        LevelSystem.Instance.OnMovingToNextStageStart.RemoveListener(ChangeCurrentSlider);
    }

    private void SetInitial(Scene scene, LoadSceneMode mode)
    {
        int currentLevel = 0;

        currentLevel = LevelSystem.Instance.GetCurrentLevelNumber();

        currentLevelText.text = currentLevel.ToString();
        nextLevelText.text = (currentLevel + 1).ToString();
    }

    private void ChangeCurrentSlider()
    {
        secondStageSlider.maxValue = PointSystem.Instance.GetNeededPointsForStage();
        currentSlider = secondStageSlider;
    }

    private void SetSliderValue(int currentPoints)
    {
        currentSlider.value = currentPoints;
    }
}
