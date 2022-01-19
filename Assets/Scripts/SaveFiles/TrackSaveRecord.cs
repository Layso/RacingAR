using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TrackSaveRecord {
	public List<Vector3> RoadNodes = new List<Vector3>();
	public List<Vector3> TrafficCones = new List<Vector3>();
	public List<Vector3> Cinderblocks = new List<Vector3>();
}