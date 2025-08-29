using System.Runtime.Serialization;
using TLDPlus.Core;
using TLDPlus.Extensions;
using UnityEngine;

namespace TLDPlus.Modules
{
	[DataContract]
	internal class BetterTankCaps : Module
	{
		public override string Name => "Better tank caps";

		public override void Update()
		{
			if (!Enabled) return;
			if (mainscript.M?.player == null) return;

			RaycastHit hitInfo1;
			if (Physics.Raycast(mainscript.M.player.Cam.transform.position, mainscript.M.player.Cam.transform.forward, out hitInfo1, mainscript.M.player.FrayRange, (int)mainscript.M.player.useLayer))
			{
				if (hitInfo1.collider.GetComponent<tankcapscript>() != null)
				{
					tankcapscript pcap = mainscript.M.player.lookedCap = hitInfo1.collider.GetComponent<tankcapscript>();
					mainscript.M.player.capString = pcap.GetDecimalCapString();
				}
			}
		}
	}
}
