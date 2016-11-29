using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;
using System.Text;
using System;
using System.Collections.Generic;
using Assets.Scripts.Enemies;

public class SaveLoadManager : MonoBehaviour {
	private GameObject _player;
	private GameObject[] enemies;

	[SerializeField] StringPrefab[] prefabs;
	[Serializable]
	public struct StringPrefab
	{
		public string key;
		public GameObject value;
	}
	Dictionary<string,GameObject> prefabDictionary = new Dictionary<string, GameObject>();

	void Start()
	{
		//Add prefabs to dictionary - used for instantiation
		foreach (var prefab in prefabs) {
			if (!prefabDictionary.ContainsKey (prefab.key)) {
				prefabDictionary.Add (prefab.key, prefab.value);
			}
		}
		//Find GameObjects from scene
		this._player = GameObject.FindGameObjectWithTag ("Player");
		this.enemies = GameObject.FindGameObjectsWithTag ("Hostile");
	}

	public void SaveGame(string savePath){
		//Create a folder for saves incase it does not already exist
		string saveFolder = Directory.GetCurrentDirectory () + "/Saves";
		if (!Directory.Exists (saveFolder))
			Directory.CreateDirectory (saveFolder);

		//Setup Xml writer and file-stream
		XmlWriterSettings ws = new XmlWriterSettings ();
		ws.Indent = true;
		ws.Encoding = Encoding.UTF8;
		//Setup the writer and ouptut
		using (FileStream fs = new FileStream (saveFolder + '/' + savePath + ".xml", FileMode.Create)) {
			using (XmlWriter writer = XmlWriter.Create (fs, ws)) {
				//Write start of document
				writer.WriteStartDocument ();
				writer.WriteStartElement ("Save");

				//Add things to save here!
				_player.GetComponent<Wizard> ().Save (writer);
				SaveEnemies (writer);

				writer.WriteEndElement ();
				writer.WriteEndDocument ();
			}
		}
	}

	private void SaveEnemies(XmlWriter wr) {
		wr.WriteStartElement ("Enemies");
		foreach(GameObject go in enemies) {
			go.GetComponent<Enemy> ().Save (wr);
		}
		wr.WriteEndElement ();
	}

	public void LoadGame(string loadPath) {
		string fullPath = Directory.GetCurrentDirectory () + "/Saves/" + loadPath + ".xml";
		if (File.Exists (fullPath)) {
			Uri basePath = new Uri (fullPath);
			using(XmlReader reader = XmlReader.Create(basePath.AbsoluteUri)){
				if (_player == null)
					_player = Instantiate (prefabDictionary ["Player"]);
				reader.ReadStartElement ("Save");
				_player.GetComponent<Wizard> ().Load (reader);
				LoadEnemies (reader);
			}
		} else
			Debug.Log ("The specified save-game did not exist.");
	}
	private void LoadEnemies(XmlReader reader) {
		reader.ReadStartElement("Enemies");

		GameObject enemy = null;
		bool enemyToParse = reader.ReadToFollowing ("Enemy");
		Debug.Log ("Started parsing enemies..." + enemyToParse);
		do {
			if(!enemyToParse)
				break;
			
			Debug.Log ("Found an enemy");
			switch (reader.GetAttribute ("Type")) {
			case "Bat":
				enemy = Instantiate (prefabDictionary ["Bat"]);
				break;
			case "Blob":
				enemy = Instantiate (prefabDictionary ["Blob"]);
				break;
			} 
			enemy.GetComponent<Enemy> ().Load (reader);
		} while(reader.ReadToNextSibling("Enemy"));

	}
}
