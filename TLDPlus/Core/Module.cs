using System.Runtime.Serialization;
using TLDPlus.Modules;
using TLDPlus.Utility;
using UnityEngine;

namespace TLDPlus.Core
{
	[KnownType(typeof(BetterTankCaps))]
	[KnownType(typeof(HoldToShit))]
	[KnownType(typeof(BetterTumbleweeds))]
	[DataContract]
	internal abstract class Module
	{
		public abstract string Name { get; }

		[DataMember] public bool Enabled = true;
		[DataMember] public bool ShowGUI = false;

		public virtual void Update() { }
		public virtual void FixedUpdate() { }
		public virtual void OnEnable() { }
		public virtual void OnDisable() { }

		public virtual void OnGUIConfig()
		{
			GUILayout.Label(Name);
			if (GUILayout.Button($"{(ShowGUI ? "Contract" : "Expand")} settings", GUILayout.ExpandWidth(false), GUILayout.MaxWidth(200)))
			{
				ShowGUI = !ShowGUI;
				ConfigChangeTracker.MarkDirty();
			}

			if (!ShowGUI) return;

			GUILayout.BeginVertical("box");

			bool newEnabled = GUILayout.Toggle(Enabled, "Enabled");
			if (newEnabled != Enabled)
			{
				Enabled = newEnabled;
				if (Enabled)
					OnEnable();
				else
					OnDisable();
				ConfigChangeTracker.MarkDirty();
			}

			if (DrawCustomConfig()) ConfigChangeTracker.MarkDirty();

			if (GUILayout.Button("Reset to defaults"))
			{
				ResetToDefaults();
				ConfigChangeTracker.MarkDirty();
			}

			GUILayout.EndVertical();
		}

		public virtual bool DrawCustomConfig() => false;
		public virtual void ResetToDefaults() { }
	}
}
