using System.Collections.Generic;
using System.Reflection;
using TLDLoader;
using TLDPlus.Utility;
using UnityEngine;
using Logger = TLDPlus.Utility.Logger;
using Module = TLDPlus.Core.Module;

namespace TLDPlus
{
    public class TLDPlus : Mod
	{
		// Mod meta stuff.
		public override string ID => "M_TLDPlus";
		public override string Name => "TLDPlus";
		public override string Author => "M-";
		public override string Version => "1.0.0";
		public override bool LoadInMenu => true;

		internal static TLDPlus Mod;

		internal static bool debug = false;

		private List<Module> _modules;
		private bool _showUI = false;
		private bool _loaded = false;
		private Vector2 _scrollPosition = Vector2.zero;

		private SettingsView _settingsView;

		public int screenWidth;
		public int screenHeight;

		public TLDPlus()
		{
			Mod = this;

			Logger.Init();
		}

		public override void Config()
		{
			SettingAPI setting = new SettingAPI(this);

			if (_loaded)
			{
				if (GUI.Button(new Rect(10, 10, 200, 20), "Toggle module settings"))
				{
					_showUI = !_showUI;
					_settingsView.selectedMod = null;
				}
			}

			// Debug stuff.
			debug = setting.GUICheckbox(debug, "Debug mode", 10, _loaded ? 40 : 10);
		}

		public override void OnMenuLoad()
		{
			_loaded = false;
			_settingsView = GameObject.FindObjectOfType<SettingsView>();
		}

		public override void OnLoad()
		{
			_modules = ConfigManager.LoadModules();
			_loaded = true;
		}

		public override void Update()
		{
			if (!_loaded) return;

			foreach (Module module in _modules)
			{
				if (module.Enabled)
					module.Update();
			}

			ConfigChangeTracker.TrySave(_modules);

			// Close settings UI if mod list is closed.
			if (_showUI && !IsSettingsOpen())
				_showUI = false;
		}

		public override void FixedUpdate()
		{
			foreach (Module module in _modules)
			{
				if (module.Enabled)
					module.FixedUpdate();
			}
		}

		public override void OnGUI()
		{
			// Find screen resolution.
			screenWidth = Screen.width;
			screenHeight = Screen.height;
			int resX = settingsscript.s.S.IResolutionX;
			int resY = settingsscript.s.S.IResolutionY;
			if (resX != screenWidth || resY != screenHeight)
			{
				screenWidth = resX;
				screenHeight = resY;
			}

			if (!_showUI) return;

			GUILayout.BeginArea(new Rect(0, 0, 400, screenHeight), string.Empty, "box");
			GUILayout.BeginVertical();
			GUILayout.Space(10);
			GUILayout.BeginHorizontal();
			GUILayout.Label("<b>TLDPlus Settings</b>");
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("X", GUILayout.MaxWidth(25)))
				_showUI = false;
			GUILayout.EndHorizontal();

			_scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
			foreach (Module module in _modules)
			{
				module.OnGUIConfig();
				GUILayout.Space(10);
			}
			GUILayout.EndScrollView();
			GUILayout.EndVertical();
			GUILayout.EndArea();
		}

		private bool IsSettingsOpen()
		{
			FieldInfo visibleField = _settingsView.GetType().GetField("visible", BindingFlags.NonPublic | BindingFlags.Instance);
			return (bool)visibleField.GetValue(_settingsView);
		}
	}
}
