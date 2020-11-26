using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class CameraSystem : Singleton<CameraSystem>
{
    private Camera currentCam;
    private Vector3 secondStagePos;

    private float camShakeDuration = 1.0f;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += GetMainCamera;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= GetMainCamera;
    }

    /// <summary>
    /// Get the main camera for further use per level.
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="loadMode"></param>
    private void GetMainCamera(Scene scene, LoadSceneMode loadMode)
    {
        try
        {
            currentCam = Camera.main;

            if (currentCam)
                secondStagePos = currentCam.transform.GetChild(0).GetComponent<Transform>().position;

            Debug.LogWarning("CAMERA FOUND!!");
        }
        catch(System.Exception e)
        {
            Debug.LogWarning("Error occured while trying to get main camera with message = " + e.Message);
        }
    }

    /// <summary>
    /// Moves camera to the next stage and returns a Tween.
    /// </summary>
    /// <param name="cameraMoveAnimDuration"></param>
    /// <returns></returns>
    public Tween MoveCameraToNextStage(float cameraMoveAnimDuration)
    {
        // TODO: Move camera with DOTween after it is added to the project. And use its callbacks.

        Tween tw = null;

        if (currentCam && secondStagePos != null)
        {
            tw = currentCam.transform.DOMove(secondStagePos, cameraMoveAnimDuration);
        }

        return tw;
    }

    /// <summary>
    /// Returns the world location where the mouse is pointing.
    /// </summary>
    /// <param name="pointForDistance"></param>
    /// <param name="lockToGround"></param>
    /// <returns></returns>
    public Vector3 GetMouseWorldLocation(Vector3 pointForDistance, bool lockToGround)
    {
        Ray ray = currentCam.ScreenPointToRay(Input.mousePosition);

        Vector3 loc = ray.GetPoint((pointForDistance - ray.origin).magnitude);

        if (lockToGround)
            loc.y = 0;

        return loc;
    }

    /// <summary>
    /// To shake the camera.
    /// </summary>
    public void ShakeCamera()
    {
        currentCam.DOShakePosition(camShakeDuration);
    }
}
