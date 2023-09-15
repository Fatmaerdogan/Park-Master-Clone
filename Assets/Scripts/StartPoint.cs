using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartPoint : MonoBehaviour
{
	[SerializeField] private int teamId;

    public int TeamId
    {
        get { return teamId; }
        set { teamId = value; }
    }


    private void OnMouseDown()
	{
		Events.touchStartPointEvent?.Invoke(this);
	}
}
