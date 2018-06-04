using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementBase : ScriptableObject { 

    public virtual bool Check(){
        return true;
    }
}