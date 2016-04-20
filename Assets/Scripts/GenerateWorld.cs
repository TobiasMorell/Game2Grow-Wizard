using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GenerateWorld : MonoBehaviour {
	public GameObject[] tiles;
	public float TileScale = 0.6f;
    public GameObject player;
    public GameObject[] NPCs;

	private GameObject findTile(char type)
	{
		type = char.ToUpper (type);
		switch (type) {
		case 'T':
			return tiles [0];
		case 'D':
			return tiles [1];
		case 'L':
			return tiles [2];
		case 'R':
			return tiles [3];
		case 'X':
		default:
			return null;
		}
	}

	// Use this for initialization
	void Awake () {
		//1. Open and read file - store in an array
		string[] lines = System.IO.File.ReadAllLines(System.Environment.CurrentDirectory + @"/Assets/Maps/Sample.txt");
		int height = lines.Length;

		float startX = -3 * TileScale;
		//2. Convert file to actual map
		foreach (var line in lines) {
			for(int i = 0; i < line.Length; i++) {
				GameObject tile = findTile (line[i]);
				if (tile != null) {
					var tileGO = Instantiate (tile);
                    tileGO.transform.parent = transform;
					tileGO.transform.position = new Vector2 (startX + (i * TileScale), height * TileScale);
				}
			}
			height--;
		}

        placePlayer(startX, lines.Length);
        SpawnNPCs();
	}

    private void placePlayer(float startX, int height)
    {
        /*var pl = Instantiate(player);
        pl.transform.position = new Vector2(startX + 3 * TileScale, (height * TileScale));*/
    }

    private void SpawnNPCs()
    {
        foreach (var npc in NPCs)
        {
			Instantiate(npc);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
