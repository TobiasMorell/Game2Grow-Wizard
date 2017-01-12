using UnityEngine;
using UnityEngine.UI;
using ItemClasses;
using Spells;

public class AttributeManager : MonoBehaviour{
	public int strength, intellect, vitality;
	private int remainingSkillpoints;
	#pragma warning disable 0649
	[SerializeField] Text strTxt, intTxt, vitTxt;
	[SerializeField] private int skillpointsPrLevel;
	[SerializeField] private Button[] skillUpButtons;
	#pragma warning restore 0649

	public void ChangeStrength(int value){
		increaseStrength(value);
		consumeSkillpoint ();
	}
	public void ChangeIntellect(int value){
		increaseIntellect(value);
		consumeSkillpoint ();
	}
	public void ChangeVitality(int value) {
		increaseVitality(value);
		consumeSkillpoint ();
	}

	private void increaseStrength(int value)
	{
		strength += value;
		strTxt.text = strength.ToString();
		updateDamage();
	}
	private void updateDamage()
	{
		GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>().DamageModifier = 1 + (0.2f * Mathf.Pow(strength, 2));
	}
	private void increaseIntellect(int value)
	{
		intellect += value;
		intTxt.text = intellect.ToString();
		updateInt();
	}
	private void updateInt()
	{
		var sc = GameObject.FindGameObjectWithTag("Player").GetComponent<SpellCaster>();
		sc.DamageModifier = 1 + (0.2f * Mathf.Pow(intellect, 2));
		sc.manaUI.maxValue = 10 + 2 * intellect;
		sc.ManaRegenPrTick = intellect;
	}
	private void increaseVitality(int value)
	{
		vitality += value;
		vitTxt.text = vitality.ToString();
		updateHP();
	}
	private void updateHP()
	{
		Debug.Log("Setting HP to: " + (10 + vitality * 2));
		GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>().SetHP(10 + vitality * 2);
	}

	public void RegisterEquip(ItemClasses.Item item) {
		if (item != null) {
			increaseStrength (item.GetValueFromAttribute(Attribute.Strength));
			increaseIntellect (item.GetValueFromAttribute(Attribute.Intellect));
			increaseVitality (item.GetValueFromAttribute(Attribute.Vitality));
		}
	}
	public void RegisterUnequip(ItemClasses.Item item) {
		if (item != null) {
			increaseStrength(-item.GetValueFromAttribute(Attribute.Strength));
			increaseIntellect(-item.GetValueFromAttribute(Attribute.Intellect));
			increaseVitality(-item.GetValueFromAttribute(Attribute.Vitality));
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

	public void LevelUp()
	{
		AssignSkillpoints(skillpointsPrLevel);
	}

	void Start() {
		strTxt.text = strength.ToString();
		intTxt.text = intellect.ToString();
		vitTxt.text = vitality.ToString();

		updateDamage();
		updateHP();
		updateInt();
	}
}
