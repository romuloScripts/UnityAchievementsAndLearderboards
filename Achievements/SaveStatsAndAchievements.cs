
using UnityEngine;

namespace Achievement{
[CreateAssetMenu(fileName = "SaveStatsAndAchievements", menuName = "Shieldnator/SaveStatsAndAchievements", order = 0)]
public class SaveStatsAndAchievements : ScriptableObject {
    
    public AchievementStat[] stats;
	public SteamAchievement[] achievements;
    public AchievementStat[] temporaryStats;

    public void SteamSave(){
        #if STEAM
        SteamStatsAndAchievements.SaveStats();
        #endif
    }

    public void ExitAndSteamSave(){
        SteamSave();
        foreach (var item in temporaryStats){
            item.ResetData();
        }
    }
}
}