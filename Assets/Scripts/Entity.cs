using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class Entity : MonoBehaviour
{
	[SerializeField] protected int HP;
	[SerializeField] protected int Damage;
	public Slider healthBar;
	[SerializeField] protected float MoveSpeed;
	protected SpriteRenderer sprite_renderer;
	protected Rigidbody2D rb;

	[HideInInspector] public bool facingRight;
	protected Animator animator;

	protected Vector2 Direction {
		get { return this.facingRight ? Vector2.right : Vector2.left; }
	}

	public virtual void Awake() {
		animator = this.GetComponent<Animator> ();
		rb = this.GetComponent<Rigidbody2D>();
	}

	public virtual void Start() {
		this.healthBar.maxValue = HP;
		this.healthBar.value = HP;
	}

	public virtual void TakeDamage(int damage) {
		this.HP -= damage;
		this.healthBar.value = HP;

		if (HP <= 0)
			Destroy (this.gameObject, 0f);
	}

	public void Flip()
	{
		this.facingRight = !facingRight;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}
}

