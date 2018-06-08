using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace Achievement{
[CreateAssetMenu(fileName = "ResetCondition", menuName = "Shieldnator/UpdateStatResetCondition", order = 0)]
public class UpdateStatResetCondition : UpdateStatBase {
	
	public UpdateStatBase updateStat;
	public AchievementStat stat;
	public bool resetStat;
	public int targetValue;
	public bool minorTarget;
	public UnityEvent onCallSet;

	public override void SetStat(ref int data, Action action, int n=1){
		onCallSet.Invoke();

		if((!minorTarget && stat.data >= targetValue) || (minorTarget && stat.data <= targetValue)){
			updateStat.SetStat(ref data, action, n);
		}else{
			data =0;
			if(resetStat){
				stat.ResetData();
			}
		}
	}
}
}
