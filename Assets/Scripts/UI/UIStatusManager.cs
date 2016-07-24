using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIStatusManager : MonoBehaviour {
	public static UIStatusManager Instance {
		get;
		private set;
	}
	private List<Slider> activeSliders;

	struct StatusQueueElement {
		public float timer;
		public Sprite icon;
		public Color color;

		public StatusQueueElement(float t, Sprite s, Color c) {
			timer = t;
			icon = s;
			color = c;
		}
	};
	private Queue<StatusQueueElement> pendingEffects;

	[SerializeField] Slider statusSliderPrefab;


	void Start() {
		if (Instance != null)
			throw new InvalidOperationException ("There may only be one instance of the UIStatusManager.");
		Instance = this;
		activeSliders = new List<Slider> ();
		pendingEffects = new Queue<StatusQueueElement> ();
	}

	void Update() {
		for (int i = activeSliders.Count - 1; i >= 0; i--) {
			Slider slider = activeSliders [i];
			slider.value += Time.deltaTime;
			if (slider.value >= slider.maxValue)
				removeEffect (slider);
		}
		foreach (var newStatus in pendingEffects) {
			addStatus (newStatus);
		}
		pendingEffects.Clear ();
	}

	private void addStatus(StatusQueueElement sqe) {
		//Instantiate and set image
		var nextIcon = Instantiate<Slider> (statusSliderPrefab);
		nextIcon.GetComponent<Image> ().sprite = sqe.icon;
		//Set up the timer
		nextIcon.maxValue = sqe.timer;
		nextIcon.value = 0;

		//Set position
		nextIcon.transform.SetParent (this.gameObject.transform, false);
		var rt = nextIcon.gameObject.GetComponent<RectTransform>();
		rt.pivot = new Vector2(activeSliders.Count + 1, 1);

		//And add to list
		activeSliders.Add(nextIcon);
	}
	private void removeEffect(Slider effectSlider) {
		//Remove the slider from the list and destroy it
		int index = activeSliders.FindIndex (es => es.value >= es.maxValue);
		activeSliders.RemoveAt (index);
		Destroy (effectSlider.gameObject);

		//And move the remainder of the icons right
		for (int i = index; i < activeSliders.Count; i++) {
			Slider s = activeSliders [i];
			var rt = s.gameObject.GetComponent<RectTransform>();
			rt.pivot = new Vector2(i, 1);
		}
	}

	public void AddDebuff(Sprite icon, float duration) {
		pendingEffects.Enqueue (new StatusQueueElement (duration, icon, Color.red));
	}

	public void AddBuff(Sprite icon, float duration) {
		pendingEffects.Enqueue (new StatusQueueElement (duration, icon, Color.green));
	}

	public void debug(Sprite icon) {
		AddDebuff (icon, 2f);
		//AddBuff (icon, 2f);
	}
}
