using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passability : MonoBehaviour
{
    [SerializeField] private bool badPiece = false;

    [SerializeField] private int pointToGive = 1;    // The points this object can give when caught in the catcher.

    private bool movingStages = false;

    private void OnEnable()
    {
        LevelSystem.Instance.OnMovingToNextStageStart.AddListener(MovingStageStart);
        LevelSystem.Instance.OnMovingToNextStageEnd.AddListener(MovingStageEnd);
    }

    private void OnDisable()
    {
        LevelSystem.Instance.OnMovingToNextStageStart.RemoveListener(MovingStageStart);
        LevelSystem.Instance.OnMovingToNextStageEnd.RemoveListener(MovingStageEnd);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == LayerNames.Hole)
        {
            if (!(badPiece && movingStages))
                gameObject.layer = LayerMask.NameToLayer(LayerNames.Passable);
        }

        if (LayerMask.LayerToName(other.gameObject.layer) == LayerNames.BlockCatcher)
        {
            if (badPiece && !movingStages)
            {
                GameManager.Instance.LevelLose();
            }
            else
            {
                PointSystem.Instance.AddPoints(pointToGive);
            }

            Destroy(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == LayerNames.Hole)
            gameObject.layer = LayerMask.NameToLayer(LayerNames.Unpassable);
    }

    /// <summary>
    /// Things to do when moving stage action starts.
    /// </summary>
    private void MovingStageStart()
    {
        movingStages = true;
    }

    /// <summary>
    /// Things to do when moving stage action ends.
    /// </summary>
    private void MovingStageEnd()
    {
        movingStages = false;
    }
}
