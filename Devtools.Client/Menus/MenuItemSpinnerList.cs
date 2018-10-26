using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace Devtools.Client.Menus
{
	public class MenuItemSpinnerList<T> : MenuItemSpinner
	{
		protected override dynamic MaxValue => Data.Count;
		public List<T> Data { get; set; }

		public MenuItemSpinnerList( Client client, Menu owner, string label, List<T> list, int defaultIndex, bool modulus = false, int priority = -1 ) : base( client, owner, label, defaultIndex, 0, list.Count, 1, modulus, priority ) {
			Data = list;
		}

		public override string GetDisplay() {
			return Data.ElementAt( (int)MathUtil.Clamp( (int)Value, 0, MaxValue - 1 ) ).ToString();
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
