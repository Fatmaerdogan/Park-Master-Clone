using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Events
{
	public static Action<StartPoint> touchStartPointEvent;
	public static Action startPathDrawingEvent;
	public static Action<Path> finishPathDrawingEvent;
	public static Action resetEvent;
	public static Action shipsCrashEvent;
	public static Action shipReachFinishPointEvent;
	public static Action finishLevelEvent;
	public static Action collectionEvent;
}

