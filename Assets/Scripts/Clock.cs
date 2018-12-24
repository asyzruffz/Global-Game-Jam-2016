using UnityEngine;
using System.Collections;

public class Clock : MonoBehaviour {

	public GameObject arrowHand;
	public float period = 1f;

	private float timer = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float callibrate = 6f;

		if(Input.GetMouseButtonUp(0)) {
			Restart();
		}

		if (timer <= period) {
			float angle = callibrate * timer / period * Mathf.Rad2Deg;
			arrowHand.transform.rotation = Quaternion.AngleAxis (-angle, Vector3.forward);
		} else {
			arrowHand.transform.rotation = Quaternion.AngleAxis (0, Vector3.forward);
		}

		timer += Time.deltaTime;
	}

	public void Restart() {
		timer = 0;
	}
}
