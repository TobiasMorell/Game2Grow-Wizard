using System;
using ItemClasses;
using Spells;

public static class GameRegistry
{
	private static ItemDatabase _itemDatabase;
	private static SpellDatabase _spellDatabase;

	public static ItemDatabase ItemDatabase() {
		return _itemDatabase;
	}
	public static void AssignItemDatabase(ItemDatabase instance) {
		if (_itemDatabase != null)
			throw new AccessViolationException ("The game may only have one ItemDatabase!");

		_itemDatabase = instance;
	}

	public static SpellDatabase SpellDatabase() {
		return _spellDatabase;
	}
	public static void AssignSpellDatabase(SpellDatabase instance) {
		if (_spellDatabase != null)
			throw new AccessViolationException ("The game may only have one SpellDatabase!");

		_spellDatabase = instance;
	}
}

