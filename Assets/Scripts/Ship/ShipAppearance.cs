using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAppearance : MonoBehaviour
{
	[SerializeField] private Material deadStateMaterial;
	[SerializeField] private MeshRenderer renderer;
    public void MaterialCreat(Color color)
    {
        renderer.materials[1].color = color;
    }
    public void SetAliveAppearance()
	{
		//renderer.materials[1] = aliveStateMaterial;
	}

	public void SetDeadAppearance()
	{
		renderer.materials[1] = deadStateMaterial;
	}
}
