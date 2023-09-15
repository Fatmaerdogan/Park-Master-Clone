using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Layers
{
	public const string SEA_FOR_PATH = "Sea";
	public const string START_POINT = "StartPoint";
	public const string FINISH_POINT = "FinishPoint";
	public const string SHIP = "Ship";
	public const string OBSTACLE = "Obstacle";
    public const string COLLECTABLE = "Collectable";

    public static LayerMask GetLayerByName(string layerName)
	{
		int layer = 0;
		layer |= (1 << LayerMask.NameToLayer(layerName));
		layer = ~layer;
		return layer;
	}
}