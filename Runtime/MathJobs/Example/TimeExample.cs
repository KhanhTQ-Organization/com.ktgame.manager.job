using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace com.ktgame.manager.job.math.math
{
	public class TimeExample : MonoBehaviour
	{
		[Button]
		public void Show(int day, int hour, int minute, int second, int mlsecond)
		{
			var time = new TimeSpan(day, hour, minute, second, mlsecond);
			Debug.Log(time.Show());
		}
	}
}
