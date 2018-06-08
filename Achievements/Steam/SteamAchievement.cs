
using UnityEngine;

namespace Achievement{
[CreateAssetMenu(fileName = "SteamAchievement", menuName = "Shieldnator/SteamAchievement", order = 0)]
public class SteamAchievement : Achievement {

    static int totalUnloked; 

    protected override void Unlock(){
        Debug.Log(apiName+"Unlocked");
        totalUnloked++;
        Debug.Log(totalUnloked);
        #if STEAM
        SteamStatsAndAchievements.UnlockAchievement(this);
        #endif
    }
}
}