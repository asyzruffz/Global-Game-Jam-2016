using UnityEngine;
using System.Collections.Generic;

public class TurnManager : Singleton<TurnManager> {

	public GameObject endGameText;
	public Clock clock;
	public Hex[] player1; 
	public Hex[] player2; 
	public int activePlayer = 1;

	HexColour RandomHexColour(int player) {
		return (HexColour)Random.Range(3*(player-1), 3*player);
	}

	protected override void SingletonAwake() {
		for (int i=0; i<player1.Length; i++) {
			player1[i].SetColour(RandomHexColour(1));
			player1[i].inGrid = false;
			player2[i].SetColour(RandomHexColour(2));
			player2[i].inGrid = false;
		}
		player1[0].selected.SetActive(true);
	}

	public HexColour Take() {
		HexColour nextColour;
		if (activePlayer == 1) {
			nextColour = player1[0].GetColour();
			for (int i=player1.Length-1; i>0; i--) {
				player1[i-1].SetColour(player1[i].GetColour());
			}
			player1[player1.Length-1].SetColour(RandomHexColour(1));
		} else {
			nextColour = player2[0].GetColour();
			for (int i=player2.Length-1; i>0; i--) {
				player2[i-1].SetColour(player2[i].GetColour());
			}
			player2[player2.Length-1].SetColour(RandomHexColour(2));
		}
		return nextColour;
	}

	public void SwitchHexPlayer1() {
		HexColour old = player1 [1].GetColour ();
		player1 [1].SetColour (player1 [0].GetColour ());
		player1 [0].SetColour (old);
		player1[0].selected.SetActive(true);

	}

	public void SwitchHexPlayer2() {
		HexColour old = player2 [1].GetColour ();
		player2 [1].SetColour (player2 [0].GetColour ());
		player2 [0].SetColour (old);
		player2[0].selected.SetActive(true);

	}

	public void SwitchPlayer() {
		activePlayer = activePlayer==1?2:1;
		Hex oldHex = Grid.Instance.selected;
		Grid.Instance.selected = null;
		if(oldHex != null) {
			oldHex.SetColour(oldHex.GetColour ());
		}
		player1[0].selected.SetActive(activePlayer == 1);
		player2[0].selected.SetActive(activePlayer == 2);
	}
}
