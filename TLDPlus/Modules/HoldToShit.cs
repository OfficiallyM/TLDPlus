using System.Runtime.Serialization;
using TLDPlus.Core;
using UnityEngine;

namespace TLDPlus.Modules
{
	[DataContract]
	internal class HoldToShit : Module
	{
		public override string Name => "Hold to shit";

		public override void Update()
		{
			if (!Enabled) return;

			fpscontroller player = mainscript.M.player;
            if (player.input.burp)
            {
				if (Random.Range(0.0f, 1f) < player.fartBurpRatio)
				{
					if (player.survival.shit >= player.survival.maxShit * 0.30000001192092896f && Random.Range(0.0f, 1f) > player.fartShitRatio)
						player.Shit();
					else
						player.Fart();
				}
				else
					player.Burp();
			}
        }
	}
}
