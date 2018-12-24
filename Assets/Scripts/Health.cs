using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	public GameObject fire, earth, water;

	void Update() {
		// Strictly for testing purposes
		/*if(Input.GetMouseButtonUp(0)) {
			fire.GetComponent<Animator>().SetInteger("animState", 1);
			earth.GetComponent<Animator>().SetInteger("animState", 1);
			water.GetComponent<Animator>().SetInteger("animState", 1);
		}*/
	}

	public void UpdateCrystals(bool[] crystals) {
		if(crystals[0]) {
			fire.GetComponent<Animator>().SetInteger("animState", 1);
		}
		if(crystals[1]) {
			earth.GetComponent<Animator>().SetInteger("animState", 1);
		}
		if(crystals[2]) {
			water.GetComponent<Animator>().SetInteger("animState", 1);
		}
	}
}
