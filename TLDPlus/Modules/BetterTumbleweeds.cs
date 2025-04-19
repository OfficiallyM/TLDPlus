using System.Runtime.Serialization;
using TLDPlus.Core;
using UnityEngine;
using Logger = TLDPlus.Utility.Logger;

namespace TLDPlus.Modules
{
	[DataContract]
	internal class BetterTumbleweeds : Module
	{
		public override string Name => "Better tumbleweeds";

		[DataMember]
		public int tumbleweedMax = 20;

		public static BetterTumbleweeds S;

		private int _tumbleweedCount = 0;

		public override void OnEnable()
		{
			S = this;
			if (itemdatabase.d.gthumbleweed.GetComponent<TumbleweedHelper>() == null)
				itemdatabase.d.gthumbleweed.AddComponent<TumbleweedHelper>().Module = this;

			foreach (thumbleweedScript existing in GameObject.FindObjectsOfType<thumbleweedScript>())
			{
				if (existing.gameObject.GetComponent<TumbleweedHelper>() == null)
					existing.gameObject.AddComponent<TumbleweedHelper>().Module = this;
			}
		}

		public override void OnDisable() 
		{
			if (itemdatabase.d.gthumbleweed.GetComponent<TumbleweedHelper>() != null)
				GameObject.Destroy(itemdatabase.d.gthumbleweed.GetComponent<TumbleweedHelper>());

			foreach (thumbleweedScript existing in GameObject.FindObjectsOfType<thumbleweedScript>())
			{
				if (existing.gameObject.GetComponent<TumbleweedHelper>() != null)
					GameObject.Destroy(existing.gameObject.GetComponent<TumbleweedHelper>());
			}
		}

		public override void Update()
		{
			if (!Enabled) return;
        }

		public override bool DrawCustomConfig()
		{
			GUILayout.Label("Max tumbleweeds");
			int newMax = Mathf.RoundToInt(GUILayout.HorizontalSlider(tumbleweedMax, 1, 100));
			GUILayout.Label(newMax.ToString());

			if (newMax != tumbleweedMax)
			{
				tumbleweedMax = newMax;
				return true;
			}

			return false;
		}

		public override void ResetToDefaults()
		{
			tumbleweedMax = 20;
		}

		public void IncrementCount()
		{
			_tumbleweedCount++;
		}

		public void DecrementCount()
		{
			_tumbleweedCount--;
			if (_tumbleweedCount < 0)
				_tumbleweedCount = 0;
		}

		public int GetCount() => _tumbleweedCount;
	}

	internal class TumbleweedHelper : MonoBehaviour
	{
		public BetterTumbleweeds Module;
		public void Start()
		{
			if (Module == null)
				Module = BetterTumbleweeds.S;

			Module.IncrementCount();

			Logger.Log($"Tumbleweed spawned, count {Module.GetCount()}/{Module.tumbleweedMax}", Logger.LogLevel.Debug);

			if (Module.GetCount() > Module.tumbleweedMax)
			{
				Logger.Log($"Destroyed spawned tumble weed above max count of {Module.tumbleweedMax}", Logger.LogLevel.Debug);
				GameObject.Destroy(this.gameObject);
			}
		}

		public void OnDestroy()
		{
			Module.DecrementCount();
		}
	}
}
