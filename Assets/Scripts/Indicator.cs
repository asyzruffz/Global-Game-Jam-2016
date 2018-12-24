using UnityEngine;
using System.Collections;

public class Indicator : MonoBehaviour {

	public int ActivePlyr = 1;
	public GameObject p1, p2;
	public GameObject native1, native2;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		ActivePlyr = TurnManager.Instance.activePlayer;
		if (ActivePlyr == 1) {
			p1.GetComponent<Animator>().SetInteger("animState", 1);
			p2.GetComponent<Animator>().SetInteger("animState", 0);
			native1.GetComponent<Animator>().SetInteger("animState", 1);
			native2.GetComponent<Animator>().SetInteger("animState", 0);

		} 
		else 
		{
			p2.GetComponent<Animator>().SetInteger("animState", 1);
			p1.GetComponent<Animator>().SetInteger("animState", 0);
			native2.GetComponent<Animator>().SetInteger("animState", 1);
			native1.GetComponent<Animator>().SetInteger("animState", 0);
		}

	}
}
