using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class BatController : Enemy {
	private int hp;

	// Use this for initialization
	void Awake () {
		
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}

	private void moveHorizontal(float direction)
	{
		//mayby check for obstacles
		transform.position += Vector3.right * direction * Time.deltaTime;
	}

	private void moveVertical(int direction)
	{
		transform.position += Vector3.up * direction * Time.deltaTime;
	}
}
