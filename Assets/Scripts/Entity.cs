using System;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Effects;

public abstract class Entity : MonoBehaviour
{
	[SerializeField] protected int HP;
	[SerializeField] protected int Damage;
	[SerializeField] protected int expForKill;

	public Slider healthBar;
	[SerializeField] protected float MoveSpeed;
	public SpriteRenderer sprite_renderer;
	protected Rigidbody2D rb;

	[HideInInspector] public bool facingRight;
	protected Animator animator;

	protected Vector2 Direction {
		get { return this.facingRight ? Vector2.right : Vector2.left; }
	}

	public virtual void Awake() {
		animator = this.GetComponent<Animator> ();
		sprite_renderer = this.GetComponent<SpriteRenderer> ();
		rb = this.GetComponent<Rigidbody2D>();
	}

	public virtual void Start() {
		this.healthBar.maxValue = HP;
		this.healthBar.value = HP;
	}

	public virtual void Update() {
		if(effectUpdate != null) effectUpdate ();
		checkDeathFromFalling ();
	}

	private void checkDeathFromFalling()
	{
		if (transform.position.y < -20)
			die();
	}
	protected virtual void die(){
		effectUpdate = null;
		Destroy (this.gameObject, 0f);
		ExpManager.instance.GiveExp (expForKill);
	}

	public virtual void TakeDamage(int damage, bool ignoreImmunity) {
		this.HP -= damage;
		this.healthBar.value = HP;

		if (HP <= 0) {
			die ();
		}
	}

	public void TakeDamage(int damage) {
		TakeDamage (damage, false);
	}

	public void Flip()
	{
		this.facingRight = !facingRight;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}

	#region Effects
	public delegate void effectUpdateDelegate();
	public effectUpdateDelegate effectUpdate;

	public void ApplyEffect(HostileEffect effectToApply)
	{
		effectToApply.onApplication (this);
	}
	#endregion
}

