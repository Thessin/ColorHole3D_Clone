using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class HoleScript : MonoBehaviour, IDragHandler
{
    private bool isDraggable = true;
    
    [SerializeField] private Collider stage1PlaneColl;
    [SerializeField] private Collider stage2PlaneColl;

    private Bounds currentStageBounds;

    [SerializeField] private Transform nextStageTrans;
    private float nextStageX = 0.0f;

    private void Start()
    {
        SetStage(stage1PlaneColl.bounds);
    }

    private void OnEnable()
    {
        LevelSystem.Instance.OnMovingToNextStageStart.AddListener(On_NextStart);
        LevelSystem.Instance.OnMovingToNextStageEnd.AddListener(On_NextEnd);

        GameManager.Instance.OnPauseGame.AddListener(On_Pause);
        GameManager.Instance.OnResumeGame.AddListener(On_Resume);
    }

    private void OnDisable()
    {
        LevelSystem.Instance.OnMovingToNextStageStart.RemoveListener(On_NextStart);
        LevelSystem.Instance.OnMovingToNextStageEnd.RemoveListener(On_NextEnd);

        GameManager.Instance.OnPauseGame.RemoveListener(On_Pause);
        GameManager.Instance.OnResumeGame.RemoveListener(On_Resume);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDraggable)
        {
            transform.position = currentStageBounds.ClosestPoint(CameraSystem.Instance.GetMouseWorldLocation(transform.position, true));
        }
    }

    /// <summary>
    /// Set the stage for stage's boundaries etc.
    /// </summary>
    /// <param name="bounds"></param>
    private void SetStage(Bounds bounds)
    {
        currentStageBounds = bounds;
        nextStageX = nextStageTrans.position.x;

        Collider coll = GetComponent<Collider>();

        Vector3 min = new Vector3(currentStageBounds.min.x + coll.bounds.size.x / 2, currentStageBounds.min.y, currentStageBounds.min.z + coll.bounds.size.z / 2);
        Vector3 max = new Vector3(currentStageBounds.max.x - coll.bounds.size.x / 2, currentStageBounds.max.y, currentStageBounds.max.z - coll.bounds.size.z / 2);

        currentStageBounds.SetMinMax(min, max);    // To prevent the hole going half past through the border.
    }

    /// <summary>
    /// Returns a tween while moving the hole to middle of the platform.
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    public Tween MoveToMiddle(float duration)
    {
        Tween tw = transform.DOMoveZ(0.0f, duration);

        return tw;
    }

    /// <summary>
    /// Returns a tween while moving the hole to next stage.
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    public Tween MoveToNext(float duration)
    {
        Tween tw = null;

        if (nextStageTrans != null)
            tw = transform.DOMoveX(nextStageX, duration);

        return tw;
    }

    /// <summary>
    /// Things to do when moving to next stage starts.
    /// </summary>
    private void On_NextStart()
    {
        SetDraggableState(false);

        SetStage(stage2PlaneColl.bounds);
    }

    /// <summary>
    /// Things to do when moving to next stage ends.
    /// </summary>
    private void On_NextEnd()
    {
        SetDraggableState(true);
    }

    /// <summary>
    /// Things to do when GameManager's pause event is invoked.
    /// </summary>
    private void On_Pause()
    {
        SetDraggableState(false);
    }

    /// <summary>
    /// Things to do when GameManager's resume event is invoked.
    /// </summary>
    private void On_Resume()
    {
        SetDraggableState(true);
    }
    
    /// <summary>
    /// Sets the draggable state of this hole.
    /// </summary>
    /// <param name="state"></param>
    public void SetDraggableState(bool state)
    {
        isDraggable = state;
    }
}
