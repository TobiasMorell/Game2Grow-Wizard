using System;
using UnityEngine;
using UnityEngine.UI;

public class ExpManager : MonoBehaviour
{
	public static ExpManager instance {
		get;
		private set;
	}
	[SerializeField] Slider expSlider;
	[SerializeField] Text lvlText;

	public static int Level {
		get;
		private set;
	}

	private int expToNextLvl;
	private int _currentExp;
	Wizard player;

	void Start() {
		instance = this;
		Level = 1;
		expSlider.value = 0;
		calculateNextLvl ();
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Wizard>();
	}
	private void calculateNextLvl() {
		expToNextLvl = Mathf.CeilToInt((320f - Mathf.Pow (Level, 4f)) * Level + Mathf.Pow (Level, 2f));
		expSlider.maxValue = expToNextLvl;
		Debug.Log ("Exp to next level: " + expToNextLvl);
	}

	private void LevelUp() {
		_currentExp = _currentExp - expToNextLvl;
		Level++;
		lvlText.text = "Level " + Level;
		calculateNextLvl ();
		expSlider.value = _currentExp;
		Debug.Log (_currentExp);
		player.LevelUp ();
	}

	public void GiveExp(int exp) {
		_currentExp += exp;
		expSlider.value = _currentExp;
		if (_currentExp >= expToNextLvl)
			LevelUp ();
	}
}

