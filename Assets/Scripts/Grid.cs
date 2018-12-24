using UnityEngine;
using System.Collections.Generic;

public class Grid : Singleton<Grid> {

	static readonly float ROOT3 = Mathf.Sqrt(3);
	static readonly string HEX_PATH = "Prefab/Hex";

	public float hexRadius = 0.45f;
	public int width = 16;
	public int height = 10;
	Transform localTransform;

	public static readonly int DownRight = 0;
	public static readonly int UpRight = 1;
	public static readonly int Up = 2;
	public static readonly int UpLeft = 3;
	public static readonly int DownLeft = 4;
	public static readonly int Down = 5;
	
	static readonly Coord[,] NEIGHBOR_SHIFTS = new Coord[,] { // even-q coord system
		{ new Coord(1, 1), new Coord(1, 0), new Coord(0, -1), new Coord(-1, 0), new Coord(-1, 1), new Coord(0, 1) },
		{ new Coord(1, 0), new Coord(1, -1), new Coord(0, -1), new Coord(-1, -1), new Coord(-1, 0), new Coord(0, 1) } };

	public Hex[,] hexes;
	public Hex selected;
	public Health player1Health, player2Health;
	public bool[] player1BaseHits = new bool[3];
	public bool[] player2BaseHits = new bool[3];

	protected override void SingletonAwake() {
		localTransform = transform;
		GameObject hex = Resources.Load<GameObject>(HEX_PATH);
		hexes = new Hex[width, height];

		float xShift = hexRadius * 1.5f;
		float yShift = hexRadius * ROOT3;
		float xPos = (-width / 2f) * xShift;
		
		for (int i = 0; i < width; i++) {
			float yPos = (height / 2f + (i / 2f) % 1f) * yShift;
			for (int j = 0; j < height; j++) {
				hexes[i, j] = Create(hex, i, j, new Vector2(xPos, yPos));
				if (i==0 && j==0) {
					hexes[i, j].SetColour(HexColour.Base1);
				} else if (i==width-1 && j==height-1) {
					hexes[i, j].SetColour(HexColour.Base2);
				}
				yPos -= yShift;
			}
			xPos += xShift;
		}
	}

	Hex Create(GameObject hex, int x, int y, Vector2 pos) {
		GameObject copy = Instantiate(hex);
		Hex newHex = copy.GetComponent<Hex>();
		newHex.x = x;
		newHex.y = y;
		newHex.cachedTransform.parent = localTransform;
		newHex.gameObject.layer = gameObject.layer;
		newHex.cachedTransform.localPosition = pos;
		newHex.cachedTransform.eulerAngles = Vector3.zero;
		newHex.cachedTransform.localScale = Vector3.one;
		return newHex;
	}
	
	public int HexDistance(Hex from, Hex to) {
		int z = from.y - (from.x + (from.x & 1)) / 2;
		int zOther = to.y - (to.x + (to.x & 1)) / 2;
		return Mathf.Max(Mathf.Abs(from.x - to.x), Mathf.Abs(to.x + zOther - from.x - z), Mathf.Abs(z - zOther));
	}

	public Hex ValidPosition(int x, int y) {
		if (x >= 0 && x < width && y >= 0 && y < height) {
			return hexes[x, y];
		} else {
			return null;
		}
	}

	public Hex[] GetNeighbors(Hex pos) {
		Hex[] result = new Hex[6];
		int parity = pos.x & 1;
		for (int i = 0; i < 6; i++) {
			Coord shift = NEIGHBOR_SHIFTS[parity, i];
			result[i] = ValidPosition(pos.x+shift.x, pos.y+shift.y);
		}
		return result;
	}

	public bool PlayerAdjacent(Hex hex) {
		Hex[] neighbors = GetNeighbors(hex);
		for (int i=0;i<neighbors.Length; i++) {
			if (neighbors[i]!=null && neighbors[i].PlayerColour() == TurnManager.Instance.activePlayer) {
				return true;
			}
		}
		return false;
	}

	public List<Hex> FloodFill(Hex hex) {
		List<Hex> similarHexes = new List<Hex>();
		FloodFill (similarHexes, hex, hex.GetColour());
		return similarHexes;
	}

	public void FloodFill(List<Hex> similarHexes, Hex hex, HexColour col) {
		if (similarHexes.Contains(hex) || hex.GetColour() != col || hex.power > 0 || hex.IsBase()) {
			return;
		}
		Hex[] neighborHex = GetNeighbors(hex);
		similarHexes.Add(hex);
		for(int i = 0; i < neighborHex.Length; i++) {
			if(neighborHex[i] != null) {
				FloodFill(similarHexes, neighborHex[i], col);
			}
		}
	}

	public int CheckWinning() {
		if (player1BaseHits [0] && player1BaseHits [1] && player1BaseHits [2]) {
			return 2;
		} else if (player2BaseHits [0] && player2BaseHits [1] && player2BaseHits [2]) {
			return 1;
		} else {
			return 0;
		}
	}
}

public class Coord {
	public int x;
	public int y;
	public Coord(int x, int y) {
		this.x = x;
		this.y = y;
	}
}
