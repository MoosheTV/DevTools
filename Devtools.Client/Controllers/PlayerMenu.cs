using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Devtools.Client.Helpers;
using Devtools.Client.Menus;

namespace Devtools.Client.Controllers
{
	public class PlayerMenu : Menu
	{
		private float SpeedMultiplier { get; set; } = 1f;
		private bool InfiniteStamina { get; set; }
		private bool NeverWanted { get; set; }

		private MenuItemSpinnerInt _wantedItem;

		public PlayerMenu( Client client, Menu parent ) : base( "Player Menu", parent ) {
			var heal = new MenuItem( client, this, "Heal Player" );
			heal.Activate += () => {
				if( Game.PlayerPed.Health < Game.PlayerPed.MaxHealth )
					Game.PlayerPed.Health = Math.Min( 100, Game.PlayerPed.MaxHealth );
				return Task.FromResult( 0 );
			};
			Add( heal );

			var armor = new MenuItem( client, this, "Give Armor" );
			armor.Activate += () => {
				Game.PlayerPed.Armor = 100;
				return Task.FromResult( 0 );
			};
			Add( armor );

			var gaw = new MenuItem( client, this, "Give All Weapons" );
			gaw.Activate += () => {
				foreach( WeaponHash weapon in Enum.GetValues( typeof( WeaponHash ) ) ) {
					Game.PlayerPed.Weapons.Give( weapon, 250, false, true );
				}
				return Task.FromResult( 0 );
			};
			Add( gaw );

			var pedModel = new MenuItemSubMenu( client, this, new PlayerPedMenu( client, this ), "Change Ped Model" );
			Add( pedModel );

			_wantedItem = new MenuItemSpinnerInt( client, this, "Wanted Level", Game.Player.WantedLevel, 0, 5, 1 );
			_wantedItem.ValueUpdate += ( val ) => {
				Game.Player.WantedLevel = val;
				return val;
			};
			Add( _wantedItem );

			var neverWanted = new MenuItemCheckbox( client, this, "Never Wanted", NeverWanted ) {
				IsChecked = () => NeverWanted
			};
			neverWanted.Activate += () => {
				NeverWanted = !NeverWanted;
				return Task.FromResult( 0 );
			};
			Add( neverWanted );

			var godMode = new MenuItemCheckbox( client, this, "God Mode" ) {
				IsChecked = () => Game.PlayerPed.IsInvincible
			};
			godMode.Activate += () => {
				Game.PlayerPed.IsInvincible = !Game.PlayerPed.IsInvincible;
				return Task.FromResult( 0 );
			};
			Add( godMode );

			var ragdoll = new MenuItemCheckbox( client, this, "Can Ragdoll" ) {
				IsChecked = () => Game.PlayerPed.CanRagdoll
			};
			ragdoll.Activate += () => {
				Game.PlayerPed.CanRagdoll = !Game.PlayerPed.CanRagdoll;
				return Task.FromResult( 0 );
			};
			Add( ragdoll );

			var stamina = new MenuItemCheckbox( client, this, "Infinite Stamina" );
			stamina.Activate += () => {
				InfiniteStamina = !InfiniteStamina;
				return Task.FromResult( 0 );
			};
			stamina.IsChecked = () => InfiniteStamina;
			Add( stamina );

			var speedMult = new MenuItemSpinnerF( client, this, "Sprint Speed", 1f, 1f, 1.5f, 0.05f );
			speedMult.ValueUpdate += val => {
				SpeedMultiplier = val;
				return val;
			};
			Add( speedMult );

			client.RegisterTickHandler( OnTick );
		}

		private Task OnTick() {
			try {
				Game.Player.SetRunSpeedMultThisFrame( SpeedMultiplier );
				if( InfiniteStamina ) {
					Function.Call( Hash.RESTORE_PLAYER_STAMINA, Game.Player.Handle, 1f );
				}

				if( NeverWanted ) {
					Game.Player.WantedLevel = 0;
				}

				_wantedItem.Value = Game.Player.WantedLevel;
			}
			catch( Exception ex ) {
				Log.Error( ex );
			}
			return Task.FromResult( 0 );
		}
	}
}
