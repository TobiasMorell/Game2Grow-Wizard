using UnityEngine;
using System.Collections;

public class PlayerCleanup : MonoBehaviour {
	
	// Update is called once per frame'
	[ExecuteInEditMode]
	void Update () {
		foreach (var clone in GameObject.FindGameObjectsWithTag("Player"))
		{
			if (clone.name.Contains("(Clone)"))
			{
				Destroy(clone);
				Debug.Log("Clone destroyed");
			}
		}
	}
}
