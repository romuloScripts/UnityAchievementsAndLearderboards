﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Achievement{
[CreateAssetMenu(fileName = "Set", menuName = "Social Stats/UpdateStatSet", order = 0)]
public class UpdateStatSet : UpdateStatBase {
	
	public override void SetStat(ref int data, Action action, int n=1){
		data = n;
		action.Invoke();
	}
}
}
