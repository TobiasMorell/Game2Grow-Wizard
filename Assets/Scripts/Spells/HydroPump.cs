using UnityEngine;
using System.Collections;

namespace Spells
{
	public class HydroPump : Castable
	{
		[SerializeField] float knockbackVelocity;
		[SerializeField] float maxScale;

		Animator wizard_animator;

		protected void OnTriggerEnter2D (UnityEngine.Collider2D other)
		{
			if (other.CompareTag ("Hostile")) {
				if (particleEffect)
					Instantiate (particleEffect, other.transform.position + new Vector3 (0, 2), Quaternion.identity);

				var rb = other.GetComponent<Rigidbody2D> ();

				rb.velocity = Vector2.up * 0.5f * knockbackVelocity;

				if (movingRight)
					rb.velocity += Vector2.right * knockbackVelocity;
				else
					rb.velocity += Vector2.left * knockbackVelocity;
			}
		}

		void Update() {
			if (transform.localScale.y < maxScale) {
				Vector3 scale = transform.localScale;
				Vector3 position = transform.position;

				//Scale up the beam a little and move it so the end matches the wizard's hand
				scale.y += Speed * Time.deltaTime;
				if (movingRight)
					position.y += Speed * Time.deltaTime;
				else
					position.y -= Speed * Time.deltaTime;

				transform.localScale = scale;
			}
		}

		public override void Cast (GameObject primaryTarget)
		{
			base.Cast (primaryTarget);
			wizard_animator = primaryTarget.GetComponentInParent<Animator> ();
			wizard_animator.SetBool ("Channeled_frontal", true);
			if (!movingRight) {
				Flip ();
				//Also Flip y-axis (sprite is rotated)
				Vector3 scale = transform.localScale;
				scale.y *= -1;
				transform.localScale = scale;
			}
		}

		public override void StopCast ()
		{
			wizard_animator.SetBool ("Channeled_frontal", false);
			Destroy (this.gameObject);
		}
	}
}

