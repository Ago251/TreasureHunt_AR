using System;
using UnityEngine;

public class TreasureController : MonoBehaviour
{
	private void Start() {
		GameManager.instance.TreasureToFind += 1;
	}

	private void OnDisable() {
		GameManager.instance.TreasureToFind -= 1;
	}
}
