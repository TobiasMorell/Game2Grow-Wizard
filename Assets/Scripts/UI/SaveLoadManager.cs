using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;
using System.Text;
using System;
using System.Collections.Generic;

public class SaveLoadManager : MonoBehaviour {
	private GameObject _player;
	private GameObject[] enemies;

	[SerializeField] StringPrefab[] enemyPrefabs;
	[Serializable]
	public struct StringPrefab
	{
		public string key;
		public GameObject value;
	}
	Dictionary<string,GameObject> enemyDict = new Dictionary<string, GameObject>();

	void Start()
	{
		foreach (var prefab in enemyPrefabs) {
			if (!enemyDict.ContainsKey (prefab.key)) {
				enemyDict.Add (prefab.key, prefab.value);
			}
		}
		this._player = GameObject.FindGameObjectWithTag ("Player");
		this.enemies = GameObject.FindGameObjectsWithTag ("Hostile");
	}

	public void SaveGame(string savePath){
		string saveFolder = Directory.GetCurrentDirectory () + "/Saves";
		if (!Directory.Exists (saveFolder))
			Directory.CreateDirectory (saveFolder);

		//Setup Xml writer and file-stream
		XmlWriterSettings ws = new XmlWriterSettings ();
		ws.Indent = true;
		ws.Encoding = Encoding.UTF8;
		using (FileStream fs = new FileStream (saveFolder + '/' + savePath + ".xml", FileMode.Create)) {
			using (XmlWriter writer = XmlWriter.Create (fs, ws)) {
				writer.WriteStartDocument ();
				writer.WriteStartElement ("Save");
				writePlayer (writer);
				writeEnemies (writer);
				writer.WriteEndElement ();
				writer.WriteEndDocument ();
			}
		}
	}
	private void writePlayer(XmlWriter wr) {
		wr.WriteStartElement ("Player");
		writePosition (_player.transform.position, wr);
		writeStatus (_player.GetComponent<Wizard> (), wr);
		wr.WriteEndElement ();
	}
	private void writePosition(Vector3 position, XmlWriter wr) {
		wr.WriteStartElement ("Position");
		wr.WriteStartElement ("x");
		wr.WriteValue (position.x);
		wr.WriteEndElement ();
		wr.WriteStartElement ("y");
		wr.WriteValue (position.y);
		wr.WriteEndElement ();
		wr.WriteStartElement ("z");
		wr.WriteValue (position.z);
		wr.WriteEndElement ();
		wr.WriteEndElement ();
	}
	private void writeStatus(Entity entity, XmlWriter wr) {
		wr.WriteStartElement ("Status");
		wr.WriteElementString ("HP", entity.healthBar.value.ToString());
		if (entity is Wizard) {
			var wiz = (Wizard)entity;
			wr.WriteElementString ("Mana", wiz.mana.value.ToString ());
		}
		wr.WriteEndElement ();
	}
	private void writeEnemies(XmlWriter wr) {
		wr.WriteStartElement ("Enemies");
		foreach(GameObject go in enemies) {
			Enemy e = go.GetComponent<Enemy> ();
			wr.WriteStartElement ("Enemy");
			if(e is BlobController)
				wr.WriteAttributeString ("Type", "Blob");
			else if(e is BatController)
				wr.WriteAttributeString("Type", "Bat");
			writePosition (go.transform.position, wr);
			writeStatus (e, wr);
			wr.WriteEndElement ();
		}
		wr.WriteEndElement ();
	}

	public void LoadGame(string loadPath) {
		string fullPath = Directory.GetCurrentDirectory () + "/Saves/" + loadPath + ".xml";
		if (File.Exists (fullPath)) {
			Uri basePath = new Uri (fullPath);
			using(XmlReader reader = XmlReader.Create(basePath.AbsoluteUri)){
				readPosition (_player.transform, reader);
				readStatus (_player.GetComponent<Wizard> (), reader);
				readEnemies (reader);
			}
		} else
			Debug.Log ("The specified save-game did not exist.");
	}
	private void readPosition(Transform readTo, XmlReader reader) {
		reader.ReadToFollowing ("Position");
		reader.ReadToDescendant ("x");
		Vector3 loadedPosition = new Vector3 ();
		loadedPosition.x = reader.ReadElementContentAsFloat ();
		reader.ReadToNextSibling ("y");
		loadedPosition.y = reader.ReadElementContentAsFloat ();
		reader.ReadToNextSibling ("z");
		loadedPosition.z = reader.ReadElementContentAsFloat ();
		readTo.position = loadedPosition;
	}
	private void readStatus(Entity entity, XmlReader reader) {
		reader.ReadToFollowing ("Status");
		reader.ReadToDescendant ("HP");
		entity.healthBar.value = reader.ReadElementContentAsFloat ();
		if (entity is Wizard) {
			Wizard wiz = (Wizard)entity;
			reader.ReadToNextSibling ("Mana");
			wiz.mana.value = reader.ReadElementContentAsFloat ();
		}
	}
	private void readEnemies(XmlReader reader) {
		Debug.Log ("Started parsing enemies");
		reader.ReadToFollowing("Enemies");

		GameObject enemy = null;
		while (reader.ReadToFollowing("Enemy")) {
			switch (reader.GetAttribute ("Type")) {
			case "Bat":
				enemy = Instantiate (enemyDict ["Bat"]);
				break;
			case "Blob":
				enemy = Instantiate (enemyDict ["Blob"]);
				break;
			}
			readPosition (enemy.transform, reader);
			readStatus (enemy.GetComponent<Enemy> (), reader);
		}

	}
}
