using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Achievement{
[CreateAssetMenu(fileName = "Add", menuName = "Shieldnator/UpdateStatAdd", order = 0)]
public class UpdateStatAdd : UpdateStatBase {
	
	public override void SetStat(ref int data, Action action, int n=1){
		data += n;
		action.Invoke();
	}
}
}

