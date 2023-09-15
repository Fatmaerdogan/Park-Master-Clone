using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// On base of https://gist.github.com/ftvs/5822103

public class CameraShake : MonoBehaviour, IEventsDependence
{
	[SerializeField] private float maxShakeTime;
	[SerializeField] private float shakeAmount;
	[SerializeField] private float decreaseFactor;

	private float currentShakeTime;
	private Vector3 originalPos;


	private void Awake()
	{
		originalPos = transform.position;
		SubscribeToGlobalEvents();
	}

	private void Update()
	{
		if (currentShakeTime > 0)
		{
			transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

			currentShakeTime -= Time.deltaTime * decreaseFactor;
		}
		else
		{
			currentShakeTime = 0f;
			transform.localPosition = originalPos;
		}
	}

	private void Shake()
	{
		if (currentShakeTime > 0f)
			return;

		currentShakeTime = maxShakeTime;
	}


	public void SubscribeToGlobalEvents()
	{
		Events.shipsCrashEvent += Shake;
	}

	public void UnsubscribeFromGlobalEvents()
	{
		Events.shipsCrashEvent -= Shake;
	}
}
