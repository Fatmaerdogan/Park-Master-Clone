using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPhysics : MonoBehaviour
{
	[SerializeField] private float throwUpForce;
	[SerializeField] private float torqueForce;

	private Rigidbody rigidbody;
	private BoxCollider collider;
	private Action<FinishPoint> callbackOnCollideWithFinishPoint;
	private Action callbackOnCollideWithShip;
	private Action callbackOnCollideWithObstacle;
	private bool isIgnoringCollisionsLogic;


	private void Start()
	{
		collider = GetComponent<BoxCollider>();
		rigidbody = GetComponent<Rigidbody>();
	}

	public void SetCallbackForCollisionWithFinishPoint(Action<FinishPoint> callback)
	{
		callbackOnCollideWithFinishPoint = callback;
	}

	public void SetCallbackForCollisionWithShip(Action callback)
	{
		callbackOnCollideWithShip = callback;
	}

	public void SetCallbackForCollisionWithObstacle(Action callback)
	{
		callbackOnCollideWithObstacle = callback;
	}
    public void ToggleRigidbodyPhysics(bool isActive)
	{
		collider.isTrigger = !isActive;
		rigidbody.isKinematic = !isActive;
	}

	public void ToggleCollisionsCallbacks(bool isActive)
	{
		isIgnoringCollisionsLogic = !isActive;
	}

	public void ThrowShipUp()
	{
		rigidbody.AddForce(Vector3.up * throwUpForce, ForceMode.Impulse);
		rigidbody.AddTorque(UnityEngine.Random.onUnitSphere * torqueForce, ForceMode.Impulse);
	}


	private void OnTriggerEnter(Collider other)
	{
		ProcessColision(other);
	}

	private void ProcessColision(Collider other)
	{
		if (isIgnoringCollisionsLogic)
			return;

        switch (LayerMask.LayerToName(other.gameObject.layer))
        {
            case Layers.FINISH_POINT:
                callbackOnCollideWithFinishPoint?.Invoke(other.GetComponent<FinishPoint>());
                break;
            case Layers.SHIP:
                callbackOnCollideWithShip?.Invoke();
                break;
            case Layers.OBSTACLE:
                callbackOnCollideWithObstacle?.Invoke();
                break;
        }
    }
}
