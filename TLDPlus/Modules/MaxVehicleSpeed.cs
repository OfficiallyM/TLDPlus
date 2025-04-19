using System;
using System.Runtime.Serialization;
using TLDPlus.Core;
using UnityEngine;
using Logger = TLDPlus.Utility.Logger;

namespace TLDPlus.Modules
{
	[DataContract]
	internal class MaxVehicleSpeed : Module
	{
		public override string Name => "Max vehicle speed";

		[DataMember]
		public float velocity = 416.6667f;

		public override void OnEnable()
		{
			mainscript.M.maxCarVelocity = velocity;
		}

		public override void OnDisable()
		{
			mainscript.M.maxCarVelocity = 200f;
		}

		public override bool DrawCustomConfig()
		{
			GUILayout.Label("Max velocity");
			float newVelocity = Mathf.Round(GUILayout.HorizontalSlider(velocity * 3.6f, 100, 10000));
			newVelocity = float.Parse(GUILayout.TextField(newVelocity.ToString()));
			GUILayout.Label($"{newVelocity} km/h");

			if (newVelocity != velocity * 3.6f)
			{
				velocity = newVelocity / 3.6f;
				mainscript.M.maxCarVelocity = velocity;
				return true;
			}

			return false;
		}

		public override void ResetToDefaults()
		{
			velocity = 416.6667f;
		}
	}
}
