using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using Devtools.Client.Helpers;
using Devtools.Client.Menus;

namespace Devtools.Client.Controllers
{
	public class HudMenu : Menu
	{
		private static readonly List<HudComponent> DisabledComponents = new List<HudComponent>();

		public HudMenu( Client client, Menu parent ) : base( "HUD Menu", parent ) {
			var radar = new MenuItemCheckbox( client, this, "Hide Minimap" ) {
				IsChecked = API.IsRadarHidden
			};
			radar.Activate += () => {
				API.DisplayRadar( API.IsRadarHidden() );
				return Task.FromResult( 0 );
			};
			Add( radar );

			Add( new MenuItemSubMenu( client, this, new HudComponentMenu( client, this ), "HUD Components" ) );

			var resetTimecycle = new MenuItem( client, this, "Reset Timecycle" );
			resetTimecycle.Activate += () => {
				Function.Call( Hash.CLEAR_TIMECYCLE_MODIFIER );
				return Task.FromResult( 0 );
			};
			Add( resetTimecycle );

			var multTimecycle = new MenuItemSpinnerF( client, this, "Timecycle Strength", 1f, 0f, 5f, 0.05f, true );
			multTimecycle.ValueUpdate += ( val ) => {
				Function.Call( Hash.SET_TIMECYCLE_MODIFIER_STRENGTH, val );
				return val;
			};
			Add( multTimecycle );

			var timecycles = new MenuItemSubMenu( client, this, new TimeCycleMenu( client, this ), "Timecycles" );
			Add( timecycles );

			client.RegisterTickHandler( OnTick );
		}

		private async Task OnTick() {
			try {
				foreach( var comp in new List<HudComponent>( DisabledComponents ) ) {
					API.HideHudComponentThisFrame( (int)comp );
				}
			}
			catch( Exception ex ) {
				Log.Error( ex );
				await BaseScript.Delay( 1000 );
			}
		}

		private class HudComponentMenu : Menu
		{
			public HudComponentMenu( Client client, Menu parent ) : base( "HUD Components", parent ) {
				var hide = new MenuItem( client, this, "Hide All Components" );
				hide.Activate += () => {
					foreach( HudComponent comp in Enum.GetValues( typeof( HudComponent ) ) ) {
						if( DisabledComponents.Contains( comp ) ) continue;
						DisabledComponents.Add( comp );
					}
					return Task.FromResult( 0 );
				};
				Add( hide );

				var show = new MenuItem( client, this, "Show All Components" );
				show.Activate += () => {
					DisabledComponents.Clear();
					return Task.FromResult( 0 );
				};
				Add( show );

				foreach( HudComponent comp in Enum.GetValues( typeof( HudComponent ) ) ) {
					Add( new MenuItemHudComponent( client, this, Enum.GetName( typeof( HudComponent ), comp ) ?? "", comp, true ) );
				}
			}
		}

		private class MenuItemHudComponent : MenuItemCheckbox
		{
			private HudComponent Component { get; }

			public MenuItemHudComponent( Client client, Menu owner, string label, HudComponent comp, bool isChecked = false, int priority = -1 ) : base( client, owner, label, isChecked, priority ) {
				Component = comp;

				IsChecked = () => !DisabledComponents.Contains( Component );
			}

			protected override Task OnActivate() {
				if( DisabledComponents.Contains( Component ) ) {
					DisabledComponents.Remove( Component );
				}
				else {
					DisabledComponents.Add( Component );
				}
				return base.OnActivate();
			}
		}
	}
}
