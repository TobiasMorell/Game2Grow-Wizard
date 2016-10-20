using UnityEngine;
using System.Collections;
using Spells;

public class Star : Bolt {
	float amplitudeY = 2.5f;
	float baseY;
	float omegaY = 8.0f;
	float index;

	protected override void Start() {
		base.Start ();
		baseY = this.transform.position.y;
	}

	// Update is called once per frame
	protected override void Update () {
		base.Update ();
		index += Time.deltaTime;
		float y = Mathf.Abs (amplitudeY*Mathf.Sin (omegaY*index)+baseY);
		transform.localPosition= new Vector3(transform.position.x , y , 0);
	}
}
