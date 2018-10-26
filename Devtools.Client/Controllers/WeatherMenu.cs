using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Devtools.Client.Helpers;
using Devtools.Client.Menus;

namespace Devtools.Client.Controllers
{
	class WeatherMenu : Menu
	{

		private WeatherType _weather;
		/// <summary>
		/// Sets the weather type to whatever you define it to.
		/// </summary>
		public WeatherType Weather
		{
			get => _weather;
			set {
				try {
					var newVal = (Enum.GetName( typeof( WeatherType ), value ) ?? "").ToUpper();
					Function.Call( Hash._SET_WEATHER_TYPE_OVER_TIME, newVal, 3f );
					_weather = value;
				}
				catch( Exception ex ) {
					Log.Error( ex );
				}
			}
		}

		public WeatherMenu( Client client, Menu parent ) : base( "Weather Menu", parent ) {
			foreach( WeatherType type in Enum.GetValues( typeof( WeatherType ) ) ) {
				Add( new MenuItemWeather( client, this, Enum.GetName( typeof( WeatherType ), type ) ?? "", type ) );
			}
		}

		private class MenuItemWeather : MenuItem
		{
			private string TypeChar { get; }

			public MenuItemWeather( Client client, Menu owner, string label, WeatherType type, int priority = -1 ) : base( client, owner, label, priority ) {
				TypeChar = Enum.GetName( typeof( WeatherType ), type )?.ToUpper() ?? "";
			}

			protected override async Task OnActivate() {
				await base.OnActivate();

				Function.Call( Hash._SET_WEATHER_TYPE_OVER_TIME, TypeChar, 1f );
				await BaseScript.Delay( 900 );
				Function.Call( Hash.SET_WEATHER_TYPE_PERSIST, TypeChar );
			}
		}
	}
}
