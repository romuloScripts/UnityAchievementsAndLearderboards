using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Achievement{
public abstract class UpdateStatBase : ScriptableObject {
	
	public virtual void SetStat(ref int data, Action action, int n=1){}
}
}
