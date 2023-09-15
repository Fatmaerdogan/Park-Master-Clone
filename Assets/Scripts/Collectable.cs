using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        switch (LayerMask.LayerToName(other.gameObject.layer))
        {
            case Layers.SHIP:
                Collection();
                break;
            
        }
    }
    public void Collection()
    {
        Events.collectionEvent?.Invoke();
        MoveUp();
    }
    void MoveUp()
    {
        transform.DOMove(transform.position + (Vector3.up*15), 1);
        transform.DOScale(Vector3.zero, 1f);
    }
}
