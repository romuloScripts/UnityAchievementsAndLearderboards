using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class FillScores : MonoBehaviour {

	public Leaderboard leaderboard;

	public LineScore line1;
	public LineScore line2;
	public LineScore userLine;

	private List<LineScore> lines = new List<LineScore>();
	private bool bLine = false;

	private LineScore line {
		get { return bLine?line2:line1; }
	}

	private void OnEnable() {
		leaderboard.onDownloaded.AddListener(fill);
	}

	private void OnDisable() {
		leaderboard.onDownloaded.RemoveListener(fill);
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
		ClearLines();
		LineScore lin;
		foreach (var entrie in entries) {
			if (entrie.user && userLine) {
				lin = Instantiate<LineScore>(userLine);
			} else{
				lin = Instantiate<LineScore>(line);
			}
			lin.gameObject.SetActive(true);
			lin.transform.SetParent(transform, false);
			lin.SetEntrie(entrie);
			lines.Add(lin);
			if(line2)
				bLine = !bLine;
		}
	}

	public void ClearLines () {
		foreach (var item in lines){
			Destroy(item.gameObject);
		}
		lines.Clear();
	}

}
