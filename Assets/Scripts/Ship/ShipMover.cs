using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMover : MonoBehaviour
{
	[SerializeField] private float moveSpeed;
	[SerializeField] private float rotationSpeed;

	private Vector3 startPosition;
	private List<Vector3> waypoints = new List<Vector3>();
	private Vector3 curentWaypoint = new Vector3();
	private bool isRotationalPathFollowing;

	private void Start()
	{
		SaveStartPosition();
	}

	private void Update()
	{
		if (isRotationalPathFollowing)
			ProcessRotationalPathFollowing();
	}


	public void ToggleRotationalPathFollowing(bool isActive)
	{
		isRotationalPathFollowing = isActive;
	}

	public void ResetRotation()
	{
		transform.rotation = Quaternion.identity;
	}

	public void SetPath(List<Vector3> waypoints)
	{
		this.waypoints = waypoints;
	}

	public bool HasPath()
	{
		return waypoints.Count > 0;
	}

	public void InterruptAnyMovement()
	{
		StopAllCoroutines();
	}

	public void StartMoveAlongPath(Action callbackOnFinish)
	{
		StopAllCoroutines();
		StartCoroutine(MoveAlongPath(callbackOnFinish));
	}

	public void StartMoveToStartPoint(Action callbackOnFinish)
	{
		StopAllCoroutines();
		StartCoroutine(MoveToPoint(startPosition, 5f, callbackOnFinish));
	}

	public void StartMoveTo(Vector3 target)
	{
		StopAllCoroutines();
		StartCoroutine(MoveToPoint(target));
	}


	private void SaveStartPosition()
	{
		startPosition = transform.position;
	}

	private void ProcessRotationalPathFollowing()
	{
		Vector3 direction = curentWaypoint - transform.position;
		if (direction == Vector3.zero)
			return;

		Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
	}

	private IEnumerator MoveAlongPath(Action callbackOnFinish)
	{
		for (int i = 0; i < waypoints.Count; i++)
		{
			curentWaypoint = waypoints[i];
			while (Vector3.Distance(transform.position, waypoints[i]) > 0.01f)
			{
				transform.position = Vector3.MoveTowards(transform.position, waypoints[i], moveSpeed * Time.deltaTime);
				yield return null;
			}
		}

		callbackOnFinish?.Invoke();
	}

	private IEnumerator MoveToPoint(Vector3 target, float speedModifier = 1f, Action callbackOnFinish = null)
	{
		curentWaypoint = target;

		while (Vector3.Distance(transform.position, target) > 0.01f)
		{
			transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * speedModifier * Time.deltaTime);
			yield return null;
		}

		transform.position = target;
		callbackOnFinish?.Invoke();
	}
}
