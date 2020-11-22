using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passability : MonoBehaviour
{
    [SerializeField]
    private bool badPiece = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name + "  entered with " + gameObject.name);

        if (LayerMask.LayerToName(other.gameObject.layer) == LayerNames.Hole)
            gameObject.layer = LayerMask.NameToLayer(LayerNames.Passable);

    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.gameObject.name + "  exit from " + gameObject.name);

        if (LayerMask.LayerToName(other.gameObject.layer) == LayerNames.Hole)
            gameObject.layer = LayerMask.NameToLayer(LayerNames.Unpassable);
    }
}
