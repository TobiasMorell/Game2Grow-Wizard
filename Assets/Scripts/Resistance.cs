using UnityEngine;
using Assets.Scripts.Effects;
using System.Collections.Generic;
using Spells;

public class ResistanceSchool
{
	public School school;
	public float resistancePoints;

	public ResistanceSchool (School school, float resPoints) {
		this.school = school;
		this.resistancePoints = resPoints;
	}

	public void IncreaseResistance(float increase) {
		resistancePoints += increase;
	}
	public void DecreaseResistance(float decrease) {
		resistancePoints -= decrease;
	}
}

public class Resistance : MonoBehaviour
{
	List<ResistanceSchool> resistanceValues;
	public float VulnerabilityModifier;

	void Start() {
		resistanceValues = new List<ResistanceSchool> ();
	}

	/// <summary>
	/// Increases the resistance to a school.
	/// </summary>
	/// <param name="school">School.</param>
	/// <param name="resistance">Resistance.</param>
	public void IncreaseResistanceToSchool(School school, float resistance) {
		//Find the appropriate resistance (add a new one to the list if not already present)
		ResistanceSchool rs = resistanceValues.Find (RS => RS.school == school);
		if (rs == null) {
			rs = new ResistanceSchool (school, 0);
			resistanceValues.Add (rs);
		}

		rs.IncreaseResistance (resistance);
	}

	/// <summary>
	/// Decreases the resistance to a school.
	/// </summary>
	/// <param name="school">School.</param>
	/// <param name="resistance">Resistance.</param>
	public void DecreaseResistanceToSchool(School school, float resistance) {
		IncreaseResistanceToSchool (school, -resistance);
	}

	/// <summary>
	/// Scales the damage of an incomming spell.
	/// </summary>
	/// <returns>The scaled damage.</returns>
	/// <param name="damage">Original damage of the spell.</param>
	/// <param name="school">School of the spell.</param>
	public float ScaleDamage(float damage, School school) {
		float scale = CalculateScale (school);
		return damage * scale;
	}

	/// <summary>
	/// Calculates the damage modification scale for the given school.
	/// </summary>
	/// <returns>The damage modification scale.</returns>
	/// <param name="school">School.</param>
	private float CalculateScale(School school) {
		ResistanceSchool rs = resistanceValues.Find (RS => RS.school == school);

		if (rs == null)
			return 1;

		return (1 - (rs.resistancePoints / 1000)) * VulnerabilityModifier;
	}
}

