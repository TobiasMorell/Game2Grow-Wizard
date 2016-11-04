using System;
using ItemClasses;
using Spells;
using Assets.Scripts.Inventory;

public static class GameRegistry
{
	private static ItemDropFabric _itemFabric;

	public static ItemDatabase ItemDatabase {
		get;
		private set;
	}
	public static void AssignItemDatabase(ItemDatabase instance) {
		if (ItemDatabase != null)
			throw new AccessViolationException ("The game may only have one ItemDatabase!");

		ItemDatabase = instance;
	}

	public static SpellDatabase SpellDatabase {
		get;
		private set;
	}
	public static void AssignSpellDatabase(SpellDatabase instance) {
		if (SpellDatabase != null)
			throw new AccessViolationException ("The game may only have one SpellDatabase!");

		SpellDatabase = instance;
	}

	public static ItemDropFabric ItemDropFabric {
		get;
		private set;
	}
	public static void AssignItemDropFabric(ItemDropFabric instance) {
		if (ItemDropFabric != null)
			throw new AccessViolationException ("The game may only have one ItemDropFabric!");
		ItemDropFabric = instance;
	}
}

