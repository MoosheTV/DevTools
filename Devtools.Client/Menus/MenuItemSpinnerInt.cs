using System;
using System.Threading.Tasks;

namespace Devtools.Client.Menus
{
	public class MenuItemSpinnerInt : MenuItemSpinner
	{
		public MenuItemSpinnerInt( Client client, Menu owner, string label, int defaultValue, int min, int max, int step, bool modulus = false, int priority = -1 ) : base( client, owner, label, defaultValue, min, max, step, modulus, priority ) {

		}

		protected override Task OnLeft() {
			Value = Math.Max( MinValue - (Modulus ? 1 : 0), Value - Step );
			return base.OnLeft();
		}

		protected override Task OnRight() {
			Value = Math.Min( MaxValue + (Modulus ? 1 : 0), Value + Step );
			return base.OnRight();
		}
	}
}
