using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine;

public class FillLines : MonoBehaviour {

	public Leaderboard[] leaderboards;

	public LineScore line1;
	public LineScore line2;
	public LineScore userLine;
	public RectTransform grid;
	public float heightGrid;

	private List<LineScore> lines = new List<LineScore>();
	private bool bLine = false;
	private bool[] donwloadedLeaderboards;

	private LineScore line {
		get { return bLine?line2:line1; }
	}

	private void OnEnable() {
		foreach (var leaderboard in leaderboards){
			leaderboard.onDownloaded.AddListener(fill);	
		}
	}

	private void OnDisable() {
		foreach (var leaderboard in leaderboards){
			leaderboard.onDownloaded.RemoveListener(fill);	
		}
	}

	void Start () {
		if(line1)
			line1.gameObject.SetActive(false);
		if(line2)
			line2.gameObject.SetActive(false);
		if(userLine)
			userLine.gameObject.SetActive(false);
	}

	void fill (LeaderboardEntrie[] entries) {
		if(entries.Length < lines.Count) return;
		else ClearLines();
		LineScore lin;
		float gridHeight=0;
		bLine = false;
		foreach (var entrie in entries) {
			lin = Instantiate<LineScore>(line);
			lin.gameObject.SetActive(true);
			lin.transform.SetParent(transform, false);
			lin.SetEntrie(entrie);
			lines.Add(lin);
			if(line2)
				bLine = !bLine;
			gridHeight +=lin.GetComponent<RectTransform>().sizeDelta.y;
		}
		Vector2 gridRect =grid.sizeDelta;
		gridRect.y = -heightGrid + gridHeight;
		grid.sizeDelta = gridRect;
	}

	public void ClearLines () {
		foreach (var item in lines){
			Destroy(item.gameObject);
		}
		lines.Clear();
	}
}
