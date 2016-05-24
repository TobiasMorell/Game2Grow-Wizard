﻿using System;
using UnityEngine;

public enum Effects
{
	Fire, Poison, Freeze
}

public class HostileEffect
{
	public Effects effect;
	private float duration;
	private float timer;
	float Strength { get; set; }

	public bool IsOver()
	{
		return timer >= duration;
	}

	public void onUpdate()
	{
		timer += Time.deltaTime;
	}
}

