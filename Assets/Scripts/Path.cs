using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Path : MonoBehaviour, IEventsDependence
{
	[SerializeField] private int teamId;
	[SerializeField] private float lineSmoothness;
	[SerializeField] private float distanceBetweenPoints;
	[SerializeField] private float pointsFixedHeight;

	public int TeamId
	{
		get { return teamId; }
		set { teamId = value; }
	}

	public List<Vector3> KeyPoints
	{
		get { return keyPoints; }
	}

	[SerializeField] private LineRenderer lineRenderer;
	private bool isDrawing;
	private List<Vector3> keyPoints = new List<Vector3>();
	private Camera camera;


	private void Start()
	{
		camera = Camera.main;
		SubscribeToGlobalEvents();
	}
    public void MaterialCreat(Color color)
    {
        lineRenderer.materials[0].color = color;
    }
    private void Update()
	{
		if (!isDrawing)
			return;

		if (IsTouchInterrupted() || IsPathOutOfFloor())
			FinishDrawPath();

		ProcessPathDrowing();
		ProcessPathDrowinfThroughFinishPoint();
	}

	#region Stop drawing conditions
	private bool IsTouchInterrupted()
	{
		return Input.GetMouseButtonUp(0);
	}

	private bool IsPathOutOfFloor()
	{
		RaycastHit hit;
		return !IsPointerOnObjectOfLayer(Layers.SEA_FOR_PATH, out hit);
	}
	#endregion

	private void TryStartDrawPath(StartPoint startPoint)
	{
		if (teamId != startPoint.TeamId)
			return;

		ResetPath();
		InitializeNewPath(startPoint.transform.position);
		StartDrawPath();
	}

	private void InitializeNewPath(Vector3 startPos)
	{
		AddPoint(startPos);
		AddPoint(startPos);
	}

	private void StartDrawPath()
	{
		isDrawing = true;
		Events.startPathDrawingEvent?.Invoke();
	}

	private void FinishDrawPath()
	{
		isDrawing = false;

		if (keyPoints.Count < 3)
		{
			ResetPath();
		}
		else
		{
			Events.finishPathDrawingEvent?.Invoke(this);
		}
	}

	private void ProcessPathDrowing()
	{

        RaycastHit hit;
		if (!IsPointerOnObjectOfLayer(Layers.SEA_FOR_PATH, out hit))
			return;
        UpdatePath(hit.point);
	}

	private void ProcessPathDrowinfThroughFinishPoint()
	{
		RaycastHit hit;
		if (!IsPointerOnObjectOfLayer(Layers.FINISH_POINT, out hit))
			return;

		var finishPoint = hit.collider.GetComponent<FinishPoint>();
		if (finishPoint.TeamId == teamId)
		{
			var finishPointPos = ModifyPointPositionWithFixedHeight(finishPoint.transform.position);
			UpdatePath(finishPointPos);
			FinishDrawPath();
		}
	}

	private bool IsPointerOnObjectOfLayer(string targetLayerName, out RaycastHit properHit)
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit[] hits = Physics.RaycastAll(ray, 100f);

		foreach(RaycastHit hit in hits)
		{
			if (LayerMask.LayerToName(hit.collider.gameObject.layer) == targetLayerName)
			{
				properHit = hit;
				return true;
			}
		}

		properHit = new RaycastHit();
		return false;
	}

	private void UpdatePath(Vector3 fingerProjectionPos)
	{

        if (keyPoints.Count <= 1)
			return;

        fingerProjectionPos = ModifyPointPositionWithFixedHeight(fingerProjectionPos);
		MoveLastPoint(fingerProjectionPos);

		if (AreTwoLastPointsFarEnough())
			AddPoint(fingerProjectionPos);

		UpdateSmoothLineAppearance();
	}

	private void MoveLastPoint(Vector3 pos)
	{
		keyPoints[keyPoints.Count - 1] = pos;
	}

	private bool AreTwoLastPointsFarEnough()
	{
		return Vector3.Distance(keyPoints[keyPoints.Count - 2], keyPoints[keyPoints.Count - 1]) > distanceBetweenPoints;
	}

	private void AddPoint(Vector3 pos)
	{
		pos = ModifyPointPositionWithFixedHeight(pos);
		var newKeyPoint = new Vector3();
		newKeyPoint = pos;
		keyPoints.Add(newKeyPoint);
	}

	private void UpdateSmoothLineAppearance()
	{

		Vector3[] smoothedPoints = LineSmoother.SmoothLine(keyPoints.ToArray(), lineSmoothness);
		lineRenderer.positionCount = smoothedPoints.Length;
		lineRenderer.SetPositions(smoothedPoints);
	}

	private void ResetPath()
	{
		keyPoints.Clear();
		lineRenderer.positionCount = 0;
	}

	private Vector3 ModifyPointPositionWithFixedHeight(Vector3 pos)
	{
		return new Vector3(pos.x, pointsFixedHeight, pos.z);
	}


	public void SubscribeToGlobalEvents()
	{
		Events.touchStartPointEvent += TryStartDrawPath;
		Events.resetEvent += ResetPath;
	}

	public void UnsubscribeFromGlobalEvents()
	{
		Events.touchStartPointEvent -= TryStartDrawPath;
		Events.resetEvent -= ResetPath;
	}
}
