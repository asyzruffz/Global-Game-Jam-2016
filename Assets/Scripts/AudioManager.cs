using UnityEngine;
using System.Collections;

public class AudioManager : Singleton<AudioManager> {

	public AudioSource combine, earth, water, fire, win;

	public void AttackNoise(HexColour col) {
		switch (col) {
		case HexColour.Blue1: // fall through
		case HexColour.Blue2: water.Play(); Invoke("StopWater", 1f); break;
		case HexColour.Red1: // fall through
		case HexColour.Red2: fire.Play(); Invoke("StopFire", 1f); break;
		case HexColour.Green1: // fall through
		case HexColour.Green2: earth.Play(); Invoke("StopEarth", 1f); break;
		default: break; // do nothing
		}
	}

	void StopWater() {
		water.Stop();
	}

	void StopFire() {
		fire.Stop();
	}

	void StopEarth() {
		earth.Stop();
	}
}
