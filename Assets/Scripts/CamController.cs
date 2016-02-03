using UnityEngine;
using System.Collections;

public class CamController : MonoBehaviour {
	[HideInInspector] public Vector3 startPoint;
	[HideInInspector] public bool inTransistion = false;

	private Vector3 Target;
	public float smoothTime;

	private Vector3 velocity = new Vector3 ();

    void Awake()
    {
        Target = GetComponentInParent<Transform>().position;
    }

	public void pointTo(Vector2 worldPosition)
	{
		Target = worldPosition;
		inTransistion = true;
	}

	void Update()
	{
		if (inTransistion) {
			startPoint = Vector3.SmoothDamp (startPoint, Target, ref velocity, smoothTime);
			transform.position = startPoint;
			if (Mathf.Abs(Target.x - startPoint.x) < 0.3) {
				transform.position = Target;
				inTransistion = false;
			}
		}
	}

	public void Attach(GameObject parent)
	{
		this.gameObject.transform.parent = parent.transform;
		this.transform.localPosition = new Vector3 (0f, 0.1f, -1f);
	}
}
