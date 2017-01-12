using UnityEngine;

namespace Assets.Scripts.NPC
{
	public class FlyingEnemy : Enemy
	{
		[SerializeField] private float flapSpeed = 45;
		private BoxCollider2D groundChecker;

		public override void Awake ()
		{
			base.Awake ();
			groundChecker = GetComponentInChildren<BoxCollider2D> ();
		}
		public override void Update ()
		{
			flap ();
		}

		public override void ResumeMovement ()
		{
			base.ResumeMovement ();
			rb.velocity = Vector2.up * flapSpeed * 4f;
		}
		public void flap()
		{
			if (groundChecker.IsTouchingLayers(LayerMask.GetMask("Ground")) && canMove) {
				//anim.SetTrigger ("flap");
				rb.velocity = Vector2.up * flapSpeed * Random.Range(0.9f, 1.4f); 
			}
		}
	}
}

