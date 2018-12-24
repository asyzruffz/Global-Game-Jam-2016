using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public enum HexColour { Red1, Green1, Blue1, Red2, Green2, Blue2, Base1, Base2, Blank }

public class Hex : MonoBehaviour {

	public int x;
	public int y;
	public Transform cachedTransform;
	public bool inGrid = true;
	public int power = 0;
	public Text powerValue;

	public GameObject red1, green1, blue1, base1, red2, green2, blue2, base2, selected;
	public GameObject red1Mon, green1Mon, blue1Mon, red2Mon, green2Mon, blue2Mon;

	void Awake() {
		cachedTransform = transform;
	}

	void OnMouseOver() {
		if (inGrid && Input.GetMouseButtonUp(0)) {
			if (GetColour () == HexColour.Blank && Grid.Instance.PlayerAdjacent(this)) {
				SetColour (TurnManager.Instance.Take ());
				TurnManager.Instance.SwitchPlayer();
			} else if (TurnManager.Instance.activePlayer == PlayerColour() && power == 0 && !IsBase()) {
				List<Hex> neighbors = new List<Hex>();
				neighbors = Grid.Instance.FloodFill (this);
				power = Mathf.Min (10, neighbors.Count);
				powerValue.text = power.ToString ();
				setPowerDisplay(power > 0);
				for (int i = 0; i < neighbors.Count; i++) {
					if (neighbors[i] != this) {
						neighbors[i].SetColour(HexColour.Blank);
					}
				}
				SetColour (GetColour());
				TurnManager.Instance.SwitchPlayer();
				AudioManager.Instance.combine.Play();
			} else if (TurnManager.Instance.activePlayer == PlayerColour() && power > 0) {
				if (Grid.Instance.selected != null) {
					Hex oldHex = Grid.Instance.selected;
					Grid.Instance.selected = null;
					oldHex.SetColour(oldHex.GetColour ());
				}
				Grid.Instance.selected = this;
				SetColour(GetColour());
			} else if (Grid.Instance.selected != null && TurnManager.Instance.activePlayer != PlayerColour() 
					   && TurnManager.Instance.activePlayer != 0
			           && Grid.Instance.HexDistance(this, Grid.Instance.selected) <= Grid.Instance.selected.power) {

				if (!IsBase() && targetingAllowed(Grid.Instance.selected,this)) {
					AudioManager.Instance.AttackNoise(Grid.Instance.selected.GetColour ());
					SetColour (HexColour.Blank);
					power = 0;
					SetColour (GetColour ());
					setPowerDisplay (false);
					TurnManager.Instance.SwitchPlayer ();
				} else if (IsBase()) {
					AudioManager.Instance.AttackNoise(Grid.Instance.selected.GetColour ());
					int hitWith = (int)Grid.Instance.selected.GetColour ();
					switch(PlayerColour()) {
					case 1:
						Grid.Instance.player1BaseHits[hitWith-3] = true;
						break;
					case 2:
						Grid.Instance.player2BaseHits[hitWith] = true;
						break;
					default:
						break;
					}
					Grid.Instance.player1Health.UpdateCrystals(Grid.Instance.player1BaseHits);
					Grid.Instance.player2Health.UpdateCrystals(Grid.Instance.player2BaseHits);
					if(Grid.Instance.CheckWinning() != 0) {
						Debug.Log("Player "+TurnManager.Instance.activePlayer+" wins!");
						TurnManager.Instance.endGameText.SetActive(true);
					} else {
						TurnManager.Instance.SwitchPlayer ();
					}
				}
			}
		}
	} 

	void setPowerDisplay(bool show) {
		if(TurnManager.Instance.activePlayer == 1){
			powerValue.color = Color.white;
			powerValue.GetComponent<Outline>().effectColor = Color.black;
		} else {
			powerValue.color = Color.black;
			powerValue.GetComponent<Outline>().effectColor = Color.white;
		}
		gameObject.transform.GetChild(0).gameObject.SetActive(show);
	}

	bool targetingAllowed(Hex attacker, Hex target) {
		HexColour attackColour = attacker.GetColour ();
		HexColour targetColour = target.GetColour ();
		return ((attackColour == HexColour.Red1 || attackColour == HexColour.Red2) && (targetColour == HexColour.Green1 || targetColour == HexColour.Green2))
			|| ((attackColour == HexColour.Green1 || attackColour == HexColour.Green2) && (targetColour == HexColour.Blue1 || targetColour == HexColour.Blue2))
			|| ((attackColour == HexColour.Blue1 || attackColour == HexColour.Blue2) && (targetColour == HexColour.Red1 || targetColour == HexColour.Red2));
	}

	public bool IsBase() {
		return base1.activeInHierarchy || base2.activeInHierarchy;
	}

	public int PlayerColour() {
		if (red1.activeInHierarchy || blue1.activeInHierarchy || green1.activeInHierarchy || base1.activeInHierarchy || red1Mon.activeInHierarchy || blue1Mon.activeInHierarchy || green1Mon.activeInHierarchy) {
			return 1;
		} else if (red2.activeInHierarchy || blue2.activeInHierarchy || green2.activeInHierarchy || base2.activeInHierarchy || red2Mon.activeInHierarchy || blue2Mon.activeInHierarchy || green2Mon.activeInHierarchy) {
			return 2;
		} else {
			return 0;
		}
	}

	public void SetColour(HexColour col) {
		red1.SetActive(col == HexColour.Red1 && power == 0);
		green1.SetActive(col == HexColour.Green1 && power == 0);
		blue1.SetActive(col == HexColour.Blue1 && power == 0);
		red1Mon.SetActive(col == HexColour.Red1 && power > 0);
		green1Mon.SetActive(col == HexColour.Green1 && power > 0);
		blue1Mon.SetActive(col == HexColour.Blue1 && power > 0);
		base1.SetActive(col == HexColour.Base1);
		red2.SetActive(col == HexColour.Red2 && power == 0);
		green2.SetActive(col == HexColour.Green2 && power == 0);
		blue2.SetActive(col == HexColour.Blue2 && power == 0);
		red2Mon.SetActive(col == HexColour.Red2 && power > 0);
		green2Mon.SetActive(col == HexColour.Green2 && power > 0);
		blue2Mon.SetActive(col == HexColour.Blue2 && power > 0);
		base2.SetActive(col == HexColour.Base2);
		selected.SetActive(Grid.Instance.selected == this);
	}

	public HexColour GetColour() {
		if (red1.activeInHierarchy || red1Mon.activeInHierarchy) {
			return HexColour.Red1;
		} else if (green1.activeInHierarchy || green1Mon.activeInHierarchy) {
			return HexColour.Green1;
		} else if (blue1.activeInHierarchy || blue1Mon.activeInHierarchy) {
			return HexColour.Blue1;
		} else if (base1.activeInHierarchy) {
			return HexColour.Base1;
		} else if (red2.activeInHierarchy || red2Mon.activeInHierarchy) {
			return HexColour.Red2;
		} else if (green2.activeInHierarchy || green2Mon.activeInHierarchy) {
			return HexColour.Green2;
		} else if (blue2.activeInHierarchy || blue2Mon.activeInHierarchy) {
			return HexColour.Blue2;
		} else if (base2.activeInHierarchy) {
			return HexColour.Base2;
		} else {
			return HexColour.Blank;
		}
	}

	public override bool Equals(System.Object obj) {
		if (obj == null) { return false; }
		Hex p = obj as Hex;
		if ((System.Object)p == null) { return false; }
		return (x == p.x) && (y == p.y);
	}
	
	public bool Equals(Hex p) {
		if ((object)p == null) { return false; }
		return (x == p.x) && (y == p.y);
	}
	
	public override int GetHashCode() {
		return x ^ y;
	}
}
