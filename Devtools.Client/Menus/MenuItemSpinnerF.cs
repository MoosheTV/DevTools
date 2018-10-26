using System;
using System.Threading.Tasks;

namespace Devtools.Client.Menus
{
	public class MenuItemSpinnerF : MenuItemSpinner
	{
		public MenuItemSpinnerF( Client client, Menu owner, string label, float defaultValue, float min, float max, float step, bool modulus = false, int priority = -1 ) : base( client, owner, label, defaultValue, min, max, step, modulus, priority ) {

		}

		protected override Task OnLeft() {
			Value = Math.Max( MinValue, Value - Step );
			return base.OnLeft();
		}

		protected override Task OnRight() {
			Value = Math.Min( MaxValue, Value + Step );
			return base.OnRight();
		}

		public override string GetDisplay() {
			return $"{Value:n5}";
		}
	}
}
