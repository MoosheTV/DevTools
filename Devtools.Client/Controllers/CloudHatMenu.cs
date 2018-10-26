using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core.Native;
using Devtools.Client.Helpers;
using Devtools.Client.Menus;

namespace Devtools.Client.Controllers
{
	public class CloudHatMenu : Menu
	{
		private static readonly Dictionary<CloudHatType, string> CloudHats = new Dictionary<CloudHatType, string> {
			[CloudHatType.Altostratus] = "altostratus",
			[CloudHatType.Clear01] = "Clear 01",
			[CloudHatType.Cloudy01] = "Cloudy 01",
			[CloudHatType.HorizonBand1] = "horizonband1",
			[CloudHatType.HorizonBand2] = "horizonband2",
			[CloudHatType.HorizonBand3] = "horizonband3",
			[CloudHatType.Horsey] = "horsey",
			[CloudHatType.Rain] = "RAIN",
			[CloudHatType.Shower] = "shower"
		};

		private CloudHatType _cloudHat;
		/// <summary>
		/// Sets the cloud hat type to whatever you define it to.
		/// </summary>
		public CloudHatType CloudHat
		{
			get => _cloudHat;
			set {
				try {
					_cloudHat = value;
					var val = CloudHats.ContainsKey( _cloudHat ) ? CloudHats[_cloudHat] : Enum.GetName( typeof( CloudHatType ), _cloudHat ) ?? "";
					Function.Call( Hash._SET_CLOUD_HAT_TRANSITION, val, 1f );
				}
				catch( Exception ex ) {
					Log.Error( ex );
				}
			}
		}

		public CloudHatMenu( Client client, Menu parent ) : base( "Cloud Hat Menu", parent ) {
			foreach( CloudHatType type in Enum.GetValues( typeof( CloudHatType ) ) ) {
				Add( new MenuItemWeather( client, this, Enum.GetName( typeof( CloudHatType ), type ) ?? "", type ) );
			}
		}

		private class MenuItemWeather : MenuItem
		{
			private string TypeChar { get; }

			public MenuItemWeather( Client client, Menu owner, string label, CloudHatType type, int priority = -1 ) : base( client, owner, label, priority ) {
				TypeChar = CloudHats.ContainsKey( type ) ? CloudHats[type] : Enum.GetName( typeof( CloudHatType ), type ) ?? "";
			}

			protected override async Task OnActivate() {
				await base.OnActivate();

				Function.Call( Hash._SET_CLOUD_HAT_TRANSITION, TypeChar, 1f );
			}
		}
	}
}
