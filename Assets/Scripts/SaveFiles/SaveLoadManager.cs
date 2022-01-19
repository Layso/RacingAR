using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour {
	[SerializeField] private string SaveFileName = "SavedTrack";
	public static SaveLoadManager Instance { get; private set; }

	private void Awake() {
		if (Instance == null) {
			Instance = this;
		} else {
			Destroy(gameObject);
		}
	}

	public void SaveLatestTrack(List<Transform> NodeList) {
		if (NodeList != null && NodeList.Count > 2) {
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream file = File.Create(GetPath());

			TrackSaveRecord data = new TrackSaveRecord();
			data.RoadNodes = new List<Vector3>();
			foreach (var node in NodeList) {
				data.RoadNodes.Add(node.position);
			}

			string json = JsonUtility.ToJson(data);
			//print(json);
			formatter.Serialize(file, json);
			file.Close();
			Debug.Log("Game data saved!");
		}

		print(GetPath());
	}

	public bool LoadLatestTrack(out TrackSaveRecord SaveRecord) {
		SaveRecord = null;
		if (File.Exists(GetPath())) {
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream file = File.Open(GetPath(), FileMode.Open);
			string json = (string)formatter.Deserialize(file);
			SaveRecord = JsonUtility.FromJson<TrackSaveRecord>(json);
			file.Close();

			if (SaveRecord != null && SaveRecord.RoadNodes != null && SaveRecord.RoadNodes.Count > 2) {
				foreach (Vector3 point in SaveRecord.RoadNodes) {
					print(point);
				}
				Debug.Log("Game data loaded!");
				return true;
			}
		}

		return false;
	}

	private string GetPath() {
		return Application.persistentDataPath + "/" + SaveFileName + ".dat";
	}
}
