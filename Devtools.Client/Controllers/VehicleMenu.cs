using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using Devtools.Client.Helpers;
using Devtools.Client.Menus;

namespace Devtools.Client.Controllers
{
	public class VehicleMenu : Menu
	{
		public bool IsInvincible { get; set; }
		public bool IsInvisible { get; set; }
		public bool HasNeon { get; set; }
		public Color NeonColor { get; set; } = Color.FromArgb( 255, 50, 100 );

		public VehicleMenu( Client client, Menu parent ) : base( "Vehicle Menu", parent ) {
			Add( new MenuItemSubMenu( client, this, new VehicleSpawnMenu( client, this ), "Spawn Vehicle" ) );

			var invincible = new MenuItemCheckbox( client, this, "Invincible", IsInvincible ) {
				IsChecked = () => IsInvincible
			};
			invincible.Activate += () => {
				IsInvincible = !IsInvincible;
				return Task.FromResult( 0 );
			};
			Add( invincible );

			var invisible = new MenuItemCheckbox( client, this, "Invisible", IsInvisible ) {
				IsChecked = () => IsInvisible
			};
			invisible.Activate += () => {
				IsInvisible = !IsInvisible;
				return Task.FromResult( 0 );
			};
			Add( invisible );

			var vehicleColors = new List<VehicleColor>();
			foreach( VehicleColor color in Enum.GetValues( typeof( VehicleColor ) ) ) {
				vehicleColors.Add( color );
			}

			var primary = new MenuItemSpinnerList<VehicleColor>( client, this, "Primary Color",
				new List<VehicleColor>( vehicleColors ), 0, true );
			primary.ValueUpdate += ( idx ) => {
				var color = primary.Data.ElementAt( (int)idx );
				if( Game.PlayerPed.CurrentVehicle != null )
					Game.PlayerPed.CurrentVehicle.Mods.PrimaryColor = color;
				return idx;
			};
			Add( primary );

			var secondary = new MenuItemSpinnerList<VehicleColor>( client, this, "Secondary Color",
				new List<VehicleColor>( vehicleColors ), 0, true );
			secondary.ValueUpdate += ( idx ) => {
				var color = secondary.Data.ElementAt( (int)idx );
				if( Game.PlayerPed.CurrentVehicle != null )
					Game.PlayerPed.CurrentVehicle.Mods.SecondaryColor = color;
				return idx;
			};
			Add( secondary );

			var pearl = new MenuItemSpinnerList<VehicleColor>( client, this, "Pearlescent Color",
				new List<VehicleColor>( vehicleColors ), 0, true );
			pearl.ValueUpdate += ( idx ) => {
				var color = pearl.Data.ElementAt( (int)idx );
				if( Game.PlayerPed.CurrentVehicle != null )
					Game.PlayerPed.CurrentVehicle.Mods.PearlescentColor = color;
				return idx;
			};
			Add( pearl );

			var license = new MenuItem( client, this, "License Plate" );
			license.Activate += async () => {
				var text = await UiHelper.PromptTextInput( Game.PlayerPed.CurrentVehicle?.Mods.LicensePlate, 8, client.Menu );
				if( Game.PlayerPed.CurrentVehicle != null )
					Game.PlayerPed.CurrentVehicle.Mods.LicensePlate = text;
			};
			Add( license );

			var styles = new List<LicensePlateStyle>();
			foreach( LicensePlateStyle s in Enum.GetValues( typeof( LicensePlateStyle ) ) ) {
				styles.Add( s );
			}
			var plateStyle = new MenuItemSpinnerList<LicensePlateStyle>( client, this, "License Plate Style", styles, 0, true );
			plateStyle.ValueUpdate += ( idx ) => {
				var style = plateStyle.Data.ElementAt( (int)idx );
				if( Game.PlayerPed.CurrentVehicle != null )
					Game.PlayerPed.CurrentVehicle.Mods.LicensePlateStyle = style;
				return idx;
			};
			Add( plateStyle );

			var hasNeon = new MenuItemCheckbox( client, this, "Neon Lights", HasNeon );
			hasNeon.IsChecked = () => HasNeon;
			hasNeon.Activate += () => {
				HasNeon = !HasNeon;
				return Task.FromResult( 0 );
			};
			Add( hasNeon );

			var neonR = new MenuItemSpinnerInt( client, this, "Neon Color (Red)", NeonColor.R, 0, 255, 1, true );
			neonR.ValueUpdate += ( val ) => {
				NeonColor = Color.FromArgb( val, NeonColor.G, NeonColor.B );
				return val;
			};
			Add( neonR );

			var neonG = new MenuItemSpinnerInt( client, this, "Neon Color (Green)", NeonColor.G, 0, 255, 1, true );
			neonG.ValueUpdate += ( val ) => {
				NeonColor = Color.FromArgb( NeonColor.R, val, NeonColor.B );
				return val;
			};
			Add( neonG );

			var neonB = new MenuItemSpinnerInt( client, this, "Neon Color (Blue)", NeonColor.B, 0, 255, 1, true );
			neonB.ValueUpdate += ( val ) => {
				NeonColor = Color.FromArgb( NeonColor.R, NeonColor.G, val );
				return val;
			};
			Add( neonB );

			Add( new MenuItemVehicleLivery( client, this ) );

			client.RegisterTickHandler( OnTick );
		}

		private async Task OnTick() {
			try {
				if( Game.PlayerPed.CurrentVehicle == null ) {
					await BaseScript.Delay( 100 );
					return;
				}

				foreach( VehicleNeonLight neon in Enum.GetValues( typeof( VehicleNeonLight ) ) ) {
					Game.PlayerPed.CurrentVehicle.Mods.SetNeonLightsOn( neon, HasNeon );
				}

				if( HasNeon ) {
					Game.PlayerPed.CurrentVehicle.Mods.NeonLightsColor = NeonColor;
				}

				Game.PlayerPed.CurrentVehicle.IsInvincible = IsInvincible;
				Game.PlayerPed.CurrentVehicle.Opacity = IsInvisible ? 0 : 255;

				if( IsInvincible ) {
					Game.PlayerPed.CurrentVehicle.Repair();
					Game.PlayerPed.CurrentVehicle.Wash();
				}
			}
			catch( Exception ex ) {
				Log.Error( ex );
			}
		}

		private static async Task<bool> SpawnVehicle( Model model ) {
			if( Game.PlayerPed.CurrentVehicle != null ) {
				Game.PlayerPed.CurrentVehicle.Delete();
			}

			var veh = await World.CreateVehicle( model, Game.PlayerPed.Position, Game.PlayerPed.Heading );
			if( veh == null ) {
				return false;
			}

			Game.PlayerPed.Task.WarpIntoVehicle( veh, VehicleSeat.Driver );
			veh.LockStatus = VehicleLockStatus.Unlocked;
			veh.NeedsToBeHotwired = false;
			veh.IsEngineRunning = true;
			veh.Mods.LicensePlate = "DEVTOOLS";
			return true;
		}

		private class MenuItemVehicleLivery : MenuItemSpinnerInt
		{
			protected override dynamic MaxValue => Game.PlayerPed.CurrentVehicle?.Mods.LiveryCount - 1;

			public override bool IsVisible => (Game.PlayerPed.CurrentVehicle?.Mods.LiveryCount ?? 0) > 0;

			public MenuItemVehicleLivery( Client client, Menu owner, int priority = -1 ) : base( client, owner, "Liveries", 0, 0, 2, 1, true, priority ) {
				ValueUpdate += ( val ) => {
					var v = MathUtil.Clamp( (int)val, 0, Game.PlayerPed.CurrentVehicle?.Mods.LiveryCount - 1?? 0 );
					if( Game.PlayerPed.CurrentVehicle != null )
						Game.PlayerPed.CurrentVehicle.Mods.Livery = v;
					return val;
				};
			}
		}

		private class VehicleSpawnMenu : Menu
		{
			public VehicleSpawnMenu( Client client, Menu parent ) : base( "Vehicle Spawner", parent ) {
				var inputModel = new MenuItem( client, this, "Input Model" );
				inputModel.Activate += async () => {
					try {
						var input = await UiHelper.PromptTextInput( controller: client.Menu );

						Model model = null;
						var enumName = Enum.GetNames( typeof( VehicleHash ) ).FirstOrDefault( s => s.ToLower().StartsWith( input.ToLower() ) ) ?? "";
						var modelName = "";

						if( int.TryParse( input, out var hash ) ) {
							model = new Model( hash );
							modelName = $"{hash}";
						}
						else if( !string.IsNullOrEmpty( enumName ) ) {
							var found = false;
							foreach( VehicleHash p in Enum.GetValues( typeof( VehicleHash ) ) ) {
								if( !(Enum.GetName( typeof( VehicleHash ), p )?.Equals( enumName, StringComparison.InvariantCultureIgnoreCase ) ??
									  false) ) {
									continue;
								}

								model = new Model( p );
								modelName = enumName;
								found = true;
								break;
							}

							if( !found ) {
								UiHelper.ShowNotification( $"~r~ERROR~s~: Could not load model {input}" );
								return;
							}
						}
						else {
							model = new Model( input );
							modelName = input;
						}

						if( await SpawnVehicle( model ) ) {
							UiHelper.ShowNotification( $"~g~Success~s~: Spawned ~y~{modelName}" );
						}
						else {
							UiHelper.ShowNotification( $"~r~ERROR~s~: Could not load model {modelName}. The vehicle model may still be loading." );
						}
					}
					catch( Exception ex ) {
						Log.Error( ex );
					}
				};
				Add( inputModel );

				foreach( VehicleHash hash in Enum.GetValues( typeof( VehicleHash ) ) ) {
					Add( new MenuItemVehicle( client, this, new Model( hash ), Enum.GetName( typeof( VehicleHash ), hash ) ?? "" ) );
				}
			}
		}

		private class MenuItemVehicle : MenuItem
		{
			private Model Model { get; }

			public MenuItemVehicle( Client client, Menu owner, Model model, string modelName, int priority = -1 ) : base( client, owner, modelName, priority ) {
				Model = model;
			}

			protected override async Task OnActivate() {
				await base.OnActivate();

				if( !await SpawnVehicle( Model ) ) {
					UiHelper.ShowNotification( $"~r~ERROR~s~: Could not load model {Label}. The vehicle model may still be loading." );
				}
			}
		}
	}
}
