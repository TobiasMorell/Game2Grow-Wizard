using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using ItemClasses;

public class AttributeManager : MonoBehaviour{
	public int mana_regen_multiplier = 1;
	public float manaRegenInterval;

	public int strength, intellect, vitality;
	[SerializeField] Text strTxt, intTxt, vitTxt;
	private int remainingSkillpoints;
	[SerializeField] private int skillpointsPrLevel;
	[SerializeField] private Button[] skillUpButtons;

	public void ChangeStrength(int value){
		strength += value;
		strTxt.text = strength.ToString();
		consumeSkillpoint ();
	}
	public void ChangeIntellect(int value){
		intellect += value;
		intTxt.text = intellect.ToString();
		consumeSkillpoint ();
	}
	public void ChangeVitality(int value) {
		vitality += value;
		vitTxt.text = vitality.ToString();
		consumeSkillpoint ();
	}

	public void RegisterEquip(ItemClasses.Item item) {
		if (item != null) {
			ChangeStrength (item.GetValueFromAttribute(Attribute.Strength));
			ChangeIntellect (item.GetValueFromAttribute(Attribute.Intellect));
			ChangeVitality (item.GetValueFromAttribute(Attribute.Vitality));
		}
	}
	public void RegisterUnequip(ItemClasses.Item item) {
		if (item != null) {
			ChangeStrength (-item.GetValueFromAttribute(Attribute.Strength));
			ChangeIntellect (-item.GetValueFromAttribute(Attribute.Intellect));
			ChangeVitality (-item.GetValueFromAttribute(Attribute.Vitality));
		}
	}

	private void consumeSkillpoint() {
		remainingSkillpoints--;
		if(remainingSkillpoints == 0) {
			toggleButtons(false);
		}
	}
	private void toggleButtons(bool newState) {
		foreach (var but in skillUpButtons)
		{
			but.gameObject.SetActive(newState);
		}
	}

	public void AssignSkillpoints(int points) {
		remainingSkillpoints += points;
		toggleButtons(true);
	}

	void Start() {
		strTxt.text = strength.ToString();
		intTxt.text = intellect.ToString();
		vitTxt.text = vitality.ToString();
	}
}
