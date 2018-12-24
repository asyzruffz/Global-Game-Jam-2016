using UnityEngine;
using System.Collections;

public class Shard : MonoBehaviour {

	public void OnShardDeath() {
		if (gameObject.activeInHierarchy) {
			gameObject.SetActive(false);
		}
	}
}
