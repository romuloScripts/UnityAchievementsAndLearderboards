using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Max", menuName = "Shieldnator/UpdateStatMax", order = 0)]
public class UpdateStatMax : UpdateStatBase {
	
	public override void SetStat(ref int data, Action action, int n=1){
		if(n > data){
			data = n;
		}
		action.Invoke();
	}
}
