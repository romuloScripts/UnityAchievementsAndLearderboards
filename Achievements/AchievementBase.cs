using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Achievement{
public class AchievementBase : ScriptableObject { 

    public virtual bool Check(){
        return true;
    }
}
}