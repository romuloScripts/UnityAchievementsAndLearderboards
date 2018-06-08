using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Achievement{
[CreateAssetMenu(fileName = "Achievement", menuName = "Shieldnator/Achievement", order = 0)]
public class Achievement : AchievementBase {

    public string apiName;
    public bool achieved;
    public List<Conditions> conditions;
    
    [System.Serializable]
    public class Conditions{
        public AchievementStat stat;
        public int maxValue;
    }  

    public override bool Check(){
        if(achieved) return true;
        for (int i = 0; i < conditions.Count; i++){
            if(conditions[i].stat.data < conditions[i].maxValue){
                return false;
            }
        }
        Unlock();
        return true;
    }

    protected virtual void Unlock(){
        //TODO unlock
    }

    [ContextMenu("API Name")]
    public void SetAPIName(){
       apiName = name;
    }

    [ContextMenu("Add API Name")]
    public void AddAPIName(){
       apiName = apiName + name;
    }

    [ContextMenu("ConditionValue by name")]
    public void SetConditionValue(){
       conditions[0].maxValue = int.Parse(name);
    }

}
}