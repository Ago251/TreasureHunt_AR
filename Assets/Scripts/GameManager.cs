using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : Singleton<GameManager> {
	public bool placeMode;
	private int treasureToFind;
	public int TreasureToFind {
		get => treasureToFind;
		set {
			treasureToFind = value;
			treasureUI.text = "Treasure: " + treasureToFind;
		}
	}
	public TextMeshProUGUI treasureUI;

	private void Start() {
		treasureUI.text = "Place the map";
	}

	public void DisablePlaceMode() {
		placeMode = false;
	}
}