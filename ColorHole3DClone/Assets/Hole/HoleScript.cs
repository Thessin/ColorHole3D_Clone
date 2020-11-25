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
    }

    private void OnDisable()
    {
        LevelSystem.Instance.OnMovingToNextStageStart.RemoveListener(On_NextStart);
        LevelSystem.Instance.OnMovingToNextStageEnd.RemoveListener(On_NextEnd);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDraggable)
        {
            transform.position = currentStageBounds.ClosestPoint(CameraSystem.Instance.GetMouseWorldLocation(transform.position, true));
        }
    }

    private void SetStage(Bounds bounds)
    {
        currentStageBounds = bounds;
        nextStageX = nextStageTrans.position.x;

        Collider coll = GetComponent<Collider>();

        Vector3 min = new Vector3(currentStageBounds.min.x + coll.bounds.size.x / 2, currentStageBounds.min.y, currentStageBounds.min.z + coll.bounds.size.z / 2);
        Vector3 max = new Vector3(currentStageBounds.max.x - coll.bounds.size.x / 2, currentStageBounds.max.y, currentStageBounds.max.z - coll.bounds.size.z / 2);

        currentStageBounds.SetMinMax(min, max);    // To prevent the hole going half past through the border.
    }

    public Tween MoveToMiddle(float duration)
    {
        Tween tw = transform.DOMoveZ(0.0f, duration);

        return tw;
    }

    public Tween MoveToNext(float duration)
    {
        Tween tw = null;

        if (nextStageTrans != null)
            tw = transform.DOMoveX(nextStageX, duration);

        return tw;
    }

    private void On_NextStart()
    {
        isDraggable = false;

        SetStage(stage2PlaneColl.bounds);
    }

    private void On_NextEnd()
    {
        isDraggable = true;
    }
}
