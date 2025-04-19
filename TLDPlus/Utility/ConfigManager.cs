using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using TLDLoader;
using TLDPlus.Core;
using TLDPlus.Modules;

namespace TLDPlus.Utility
{
	internal static class ConfigManager
	{
		private static string _configPath;

		public static List<Module> LoadModules()
		{
			if (_configPath == null)
				_configPath = Path.Combine(ModLoader.GetModConfigFolder(TLDPlus.Mod), "Config.json");


			List<Module> defaultModules = CreateDefaultModules();
			List<Module> loadedModules = new List<Module>();

			// Load modules from saved configuration.
			if (File.Exists(_configPath))
			{
				try
				{
					using (FileStream stream = File.OpenRead(_configPath))
					{
						var serializer = new DataContractJsonSerializer(typeof(List<Module>), GetKnownTypes());
						loadedModules = serializer.ReadObject(stream) as List<Module>;
					}
				}
				catch
				{
					Logger.Log("Failed to load TLDPlus config, reverting to defaults.", Logger.LogLevel.Warning);
					return CreateDefaultModules();
				}
			}

			// Merge saved modules with defaults to ensure new modules
			// are loaded correctly.
			foreach (Module defaultModule in defaultModules)
			{
				bool exists = loadedModules.Any(m => m.Name == defaultModule.Name);
				if (!exists)
				{
					Logger.Log($"New module '{defaultModule.Name}' detected.", Logger.LogLevel.Info);
					loadedModules.Add(defaultModule);
				}
			}

			// Trigger OnEnable() for each loaded module
			foreach (var module in loadedModules)
			{
				if (module.Enabled)
					module.OnEnable();
			}

			return loadedModules;
		}

		public static void SaveModules(List<Module> modules)
		{
			using (FileStream stream = File.Create(_configPath))
			{
				var serializer = new DataContractJsonSerializer(typeof(List<Module>), GetKnownTypes());
				serializer.WriteObject(stream, modules);
			}
		}

		private static List<Module> CreateDefaultModules()
		{
			return new List<Module>
			{
				new BetterTankCaps(),
				new HoldToShit(),
				new BetterTumbleweeds(),
				new MaxVehicleSpeed(),
			};
		}

		private static DataContractJsonSerializerSettings GetKnownTypes()
		{
			return new DataContractJsonSerializerSettings
			{
				KnownTypes = new[] { 
					typeof(BetterTankCaps),
					typeof(HoldToShit),
					typeof(BetterTumbleweeds),
					typeof(MaxVehicleSpeed),
				}
			};
		}
	}
}
