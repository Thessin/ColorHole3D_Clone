using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passability : MonoBehaviour
{
    [SerializeField] private bool badPiece = false;

    [SerializeField] private int pointToGive = 1;    // The points this object can give when caught in the catcher.

    [SerializeField] private Color badColor;
    [SerializeField] private Color goodColor;

    private bool movingStages = false;

    private void Start()
    {
        if (badPiece)
            GetComponent<MeshRenderer>().material.color = badColor;
        else
            GetComponent<MeshRenderer>().material.color = goodColor;

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
        Debug.Log(other.gameObject.name + "  entered with " + gameObject.name);

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

                Debug.LogWarning("ADD POINTS, OBJECT " + gameObject.name + " IS CAUGHT!");
            }

            Destroy(gameObject);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.gameObject.name + "  exit from " + gameObject.name);

        if (LayerMask.LayerToName(other.gameObject.layer) == LayerNames.Hole)
            gameObject.layer = LayerMask.NameToLayer(LayerNames.Unpassable);
    }

    private void MovingStageStart()
    {
        movingStages = true;
    }
    private void MovingStageEnd()
    {
        movingStages = false;
    }
}
