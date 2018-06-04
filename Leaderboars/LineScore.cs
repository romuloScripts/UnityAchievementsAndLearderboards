using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LineScore : MonoBehaviour {
	public Text pos;
	public Text userName;
	public Text score;
	public GameObject top;
	public Color colorUser = Color.yellow;

	public void SetEntrie(LeaderboardEntrie entrie){
		if(pos)
			pos.text = (entrie.pos+1)+"";
		if(userName)
			userName.text = entrie.name+"";
		if(score)
			score.text = entrie.score+"";
		if(entrie.pos != 0 && top){
			top.SetActive(false);
		}
		if(entrie.user){
			if(pos)
				pos.color = colorUser;
			if(userName)
				userName.color = colorUser;
			if(score)
				score.color = colorUser;
		}
	}
}
