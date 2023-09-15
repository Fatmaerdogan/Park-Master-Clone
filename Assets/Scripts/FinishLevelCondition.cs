using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLevelCondition : MonoBehaviour, IEventsDependence
{
	private int shipsAmount;
	private int shipsOnFinishPointAmount;


	private void Start()
	{
		SubscribeToGlobalEvents();
		CountShipsAmount();
	}

	private void CountShipsAmount()
	{
		Ship[] ships = FindObjectsOfType<Ship>();
		shipsAmount = ships.Length;
	}

	private void ResetShipsOnFinishPointAmount()
	{
		shipsOnFinishPointAmount = 0;
	}

	private void OnShipReachFinishPoint()
	{

		AddShipsOnFinishPointAmount();
		CheckFinishLevelCondition();
	}

	private void AddShipsOnFinishPointAmount()
	{
		shipsOnFinishPointAmount ++;
	}

	private void CheckFinishLevelCondition()
	{
		if (shipsOnFinishPointAmount == shipsAmount)
		{
			Events.finishLevelEvent?.Invoke();
		}
	}

	public void SubscribeToGlobalEvents()
	{

        Events.startPathDrawingEvent += ResetShipsOnFinishPointAmount;
		Events.resetEvent += ResetShipsOnFinishPointAmount;
		Events.shipReachFinishPointEvent += OnShipReachFinishPoint;
	}

	public void UnsubscribeFromGlobalEvents()
	{
		Events.startPathDrawingEvent -= ResetShipsOnFinishPointAmount;
		Events.resetEvent -= ResetShipsOnFinishPointAmount;
		Events.shipReachFinishPointEvent -= OnShipReachFinishPoint;
	}
}
