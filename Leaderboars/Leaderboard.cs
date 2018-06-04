using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class LeaderboardEntrie{
	public int score;
	public int pos;
	public string name;
	public bool user;
}

public class Leaderboard : ScriptableObject {

	public string boardName;
	public LeaderboardEntrie[] entries;
	public EntrieEvent onDownloaded;

	[System.Serializable]
	public class EntrieEvent: UnityEvent<LeaderboardEntrie[]>{}

	public virtual void Download(int type){
	 	
	}

	public virtual void Upload(int score){
		
	} 


}