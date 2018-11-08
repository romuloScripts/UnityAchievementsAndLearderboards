using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Achievement{
[CreateAssetMenu(fileName = "AchievementStat", menuName = "Social Stats/AchievementStat", order = 0)]
public class AchievementStat : ScriptableObject {
	
	public string apiName;
	public int data;
	public UpdateStatBase updateMethod;
	public List<AchievementBase> achievements;

	[System.NonSerialized]
	private List<AchievementBase> currentAchievements= null;

	public void SetStat(int n=1){
		updateMethod.SetStat(ref data, CheckAchievements, n);
	}

	public void ResetData(){
		data = 0;
	}

	void OnEnable() {
		ResetData();
    }

	public void CheckAchievements(){
		if(currentAchievements == null){
           currentAchievements = new List<AchievementBase>();
		   foreach (var item in achievements)
		   {
			    currentAchievements.Add(item);
		   }
       	}
		for (int i = currentAchievements.Count-1; i >= 0; i--){
			if(currentAchievements[i].Check()){
				currentAchievements.RemoveAt(i);
			}
		}
	}
}
}