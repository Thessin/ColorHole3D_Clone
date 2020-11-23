using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class CameraSystem : Singleton<CameraSystem>
{
    private Camera currentCam;
    private Vector3 secondStagePos;
    private float cameraMoveAnimDuration = 3.0f;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += GetMainCamera;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= GetMainCamera;
    }

    private void GetMainCamera(Scene scene, LoadSceneMode loadMode)
    {
        currentCam = Camera.main;
        secondStagePos = currentCam.transform.GetChild(0).GetComponent<Transform>().position;

        Debug.LogWarning("CAMERA FOUND!!");
    }

    public void MoveCameraToNextStage()
    {
        // TODO: Move camera with DOTween after it is added to the project. And use its callbacks.

        Debug.LogError(secondStagePos);
        if (currentCam && secondStagePos != null)
        {
            currentCam.transform.DOMove(secondStagePos, cameraMoveAnimDuration);
        }
    }
}
