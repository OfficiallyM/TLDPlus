using System.Collections.Generic;
using TLDPlus.Core;

namespace TLDPlus.Utility
{
	internal static class ConfigChangeTracker
	{
		private static bool dirty = false;

		public static void MarkDirty()
		{
			dirty = true;
		}

		public static void TrySave(List<Module> modules)
		{
			if (!dirty) return;

			ConfigManager.SaveModules(modules);
			dirty = false;
		}
	}
}
