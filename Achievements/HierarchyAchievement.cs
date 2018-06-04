using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HierarchyAchievement", menuName = "Shieldnator/HierarchyAchievement", order = 0)]
public class HierarchyAchievement : AchievementBase{
    public List<Achievement> achievements;

    [System.NonSerialized]
    private List<Achievement> currentAchievements = null;

    public override bool Check(){
/*        if(currentAchievements == null){
           currentAchievements = achievements;
       } */
       if(currentAchievements == null){
           currentAchievements = new List<Achievement>();
		   foreach (var item in achievements)
		   {
			    currentAchievements.Add(item);
		   }
       }
       CheckAchievements();
       if(currentAchievements.Count <= 0){
           return true;
       }else{
           return false;
       }
    }

    void CheckAchievements(){
		for (int i = 0; i < currentAchievements.Count ; i++){
			if(currentAchievements[i].Check()){
				currentAchievements.RemoveAt(i);
                i--;
			}else{
                break;
            }
		}
	}
}