
namespace TLDPlus.Extensions
{
	internal static class TankCapExtensions
	{
		public static string GetDecimalCapString(this tankcapscript cap)
		{
			if (cap.noText)
			{
				cap.capString = "";
			}
			else
			{
				float amount = cap.Tank.F.GetAmount();
				if (amount > 0)
				{
					string valueString = mainscript.SRound(amount, "{0:0.00}");
					string maxString = mainscript.SRound(cap.Tank.F.maxC, "{0:0.00}");

					bool full = false;
					if (amount == cap.Tank.F.maxC)
						full = true;
					cap.capString = (cap.hasName ? ls.l.g(cap.nameText) + ": \n" : "") + valueString + (full ? " " + ls.l.g(ls.w.liter) + " " + ls.l.g(ls.w.full) : " / " + maxString + " " + ls.l.g(ls.w.liter));
					foreach (mainscript.fluid fluid in cap.Tank.F.fluids)
					{
						float percent = cap.Tank.F.GetPercent(fluid.type) * 100f;
						string percentString = percent == 100 ? mainscript.SRound(percent, "{0:0}") : mainscript.SRound(percent, "{0:0.00}");
						cap.capString = cap.capString + "\n" + percentString + ls.l.g(ls.w.percent) + " " + ls.l.g(mainscript.M.fluids[(int)fluid.type].lname);
					}
				}
				else
					cap.capString = (cap.hasName ? ls.l.g(cap.nameText) + ": \n" : "") + ls.l.g(ls.w.empty);
			}
			return cap.capString;
		}
	}
}
