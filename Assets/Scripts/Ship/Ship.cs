using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Ship : MonoBehaviour, IEventsDependence
{
	[SerializeField] private int teamId;
    public int TeamId
    {
        get { return teamId; }
        set { teamId = value; }
    }


    // Main Components
    private ShipAppearance appearance;
	private ShipAnimations animations;
	private ShipMover mover;
	private ShipPhysics physics;

    public ShipAppearance ShipAppearance
	{
		get {
            return appearance; 
		}
	}
    private void Start()
    {
		animations = GetComponent<ShipAnimations>();
		mover = GetComponent<ShipMover>();
		physics = GetComponent<ShipPhysics>();

        SubscribeToGlobalEvents();

		physics.SetCallbackForCollisionWithFinishPoint(OnColideWithFinishPoint);
		physics.SetCallbackForCollisionWithShip(OnColideWithAnotherShip);
		physics.SetCallbackForCollisionWithObstacle(OnColideWithObstacle);
	}

	public void MaterialCreat(Color color)
	{
        appearance = GetComponent<ShipAppearance>();
        ShipAppearance.MaterialCreat(color);
    }
    private void OnFinishPathDrawing(Path path)
	{
		if (teamId == path.TeamId)
		{
			mover.SetPath(path.KeyPoints);
		}

		if (mover.HasPath())
		{
			Action reachPathEndCallback = animations.PlayIdleAnimation;
			mover.StartMoveAlongPath(reachPathEndCallback);
			mover.ToggleRotationalPathFollowing(true);
			animations.PlayMoveAnimation();
		}

	}

	private void OnReachStartPositionOnReset()
	{
		animations.PlayIdleAnimation();
		physics.ToggleCollisionsCallbacks(true);
		physics.ToggleRigidbodyPhysics(false);
		mover.ResetRotation();
	}

	private void OnColideWithFinishPoint(FinishPoint finishPoint)
	{
        if (finishPoint.TeamId != teamId)
			return;

		mover.StartMoveTo(finishPoint.transform.position);
		animations.PlayJumpAnimation();

        Events.shipReachFinishPointEvent?.Invoke();
	}

	private void OnColideWithAnotherShip()
	{
		Die();
		Events.shipsCrashEvent?.Invoke();
	}

	private void OnColideWithObstacle()
	{
		Die();
		Events.shipsCrashEvent?.Invoke();
	}

    private void Die()
	{
		mover.InterruptAnyMovement();
		mover.ToggleRotationalPathFollowing(false);
		physics.ToggleCollisionsCallbacks(false);
		physics.ToggleRigidbodyPhysics(true);
		physics.ThrowShipUp();
		animations.PlayDeadAnimation();
		appearance.SetDeadAppearance();
	}

	private void ResetToStartPosition()
	{
		appearance.SetAliveAppearance();
		physics.ToggleCollisionsCallbacks(false);
		physics.ToggleRigidbodyPhysics(false);
		Action callbackOnReachStartPos = OnReachStartPositionOnReset;
		mover.StartMoveToStartPoint(OnReachStartPositionOnReset);
		animations.PlayIdleAnimation();
	}

	public void SubscribeToGlobalEvents()
	{
		Events.startPathDrawingEvent += ResetToStartPosition;
		Events.finishPathDrawingEvent += OnFinishPathDrawing;
		Events.resetEvent += ResetToStartPosition;
	}

	public void UnsubscribeFromGlobalEvents()
	{
		Events.startPathDrawingEvent -= ResetToStartPosition;
		Events.finishPathDrawingEvent -= OnFinishPathDrawing;
		Events.resetEvent -= ResetToStartPosition;
	}
}
