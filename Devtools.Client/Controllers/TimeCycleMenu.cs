using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core.Native;
using Devtools.Client.Helpers;
using Devtools.Client.Menus;

namespace Devtools.Client.Controllers
{
	public class TimeCycleMenu : Menu
	{
		public TimeCycleMenu( Client client, Menu parent ) : base( "Time Cycles", parent ) {
			foreach( TimeCycle cycle in Enum.GetValues( typeof( TimeCycle ) ) ) {
				Add( new MenuItemTimeCycle( client, this, Enum.GetName( typeof( TimeCycle ), cycle ) ?? "default", cycle ) );
			}
		}

		private class MenuItemTimeCycle : MenuItem
		{
			private TimeCycle TimeCycle { get; }

			public MenuItemTimeCycle( Client client, Menu owner, string label, TimeCycle timeCycle, int priority = -1 ) : base( client, owner, label, priority ) {
				TimeCycle = timeCycle;
			}

			protected override Task OnActivate() {
				Function.Call( Hash.SET_TIMECYCLE_MODIFIER, Label );
				return base.OnActivate();
			}
		}
	}
}
