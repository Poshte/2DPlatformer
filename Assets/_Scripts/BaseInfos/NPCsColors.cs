using System.Collections.Generic;
using UnityEngine;

public static class NPCsColors
{
	public static Color PlayerColor { get => new(0.4f, 0.1376f, 0.1323f, 1f); }
	public static Color PigColor { get => new(0.8396f, 0.5180f, 0.7214f, 1f); }
	public static Color HorseColor { get => new(0f, 0f, 0f); }

	private static readonly Dictionary<string, Color> colorMap = new() {
		{ "Player", PlayerColor},
		{ "Pig", PigColor},
		{ "Horse", HorseColor}
	};

	public static Color GetColor(string name)
	{
		if (colorMap.ContainsKey(name))
			return colorMap[name];

		return Color.black;
	}
}