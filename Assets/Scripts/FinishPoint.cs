using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPoint : MonoBehaviour
{
	[SerializeField] private int teamId;
    [SerializeField] private MeshRenderer meshRenderer;

	public void MaterialCreat(Color color)
	{
		meshRenderer.materials[1].color = color;
    }
    public int TeamId
    {
        get { return teamId; }
        set { teamId = value; }
    }
}
