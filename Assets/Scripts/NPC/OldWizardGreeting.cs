using UnityEngine;
using System.Collections;

namespace Assets.Scripts.NPC {
	public class OldWizardGreeting : MonoBehaviour {
	    private Animator oldWizardAnimator;

	    void Start()
	    {
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
}