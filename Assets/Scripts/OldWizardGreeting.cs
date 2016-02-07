using UnityEngine;
using System.Collections;

public class OldWizardGreeting : MonoBehaviour {
    private BoxCollider2D greetingTrigger;
    private Animator oldWizardAnimator;

    void Start()
    {
        greetingTrigger = GetComponent<BoxCollider2D>();
        oldWizardAnimator = GetComponent<Animator>();
    }

	// Use this for initialization
	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            oldWizardAnimator.SetTrigger("Greet");
        }
    }
}
