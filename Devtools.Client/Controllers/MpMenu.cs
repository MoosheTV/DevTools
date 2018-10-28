using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Devtools.Client.Helpers;
using Devtools.Client.Menus;

namespace Devtools.Client.Controllers
{
	public class MpMenu : Menu
	{
		private float _voipRange = Function.Call<float>( Hash.NETWORK_GET_TALKER_PROXIMITY );
		private DateTime _lastRangeChange = DateTime.MinValue;

		public MpMenu( Client client, Menu parent ) : base( "Multiplayer", parent ) {

			// VOIP
			var voice = new Menu( "Voice Menu", this );
			var voipRange = new MenuItemSpinnerInt( client, this, "Voice Range (Meters)", -1, -1, 1024, 1 );
			voipRange.ValueUpdate += ( val ) => {
				var range = val;
				if( range == -1 )
					range = ushort.MaxValue;
				Function.Call( Hash.NETWORK_SET_TALKER_PROXIMITY, range * 1f );
				_voipRange = range;
				_lastRangeChange = DateTime.UtcNow;
				return val;
			};
			voice.Add( voipRange );
			var voipChannel = new MenuItemSpinnerInt( client, this, "Voice Channel", 0, 0, ushort.MaxValue, 1, true );
			voipChannel.ValueUpdate += ( val ) => {
				if( val == 0 )
					Function.Call( Hash.NETWORK_CLEAR_VOICE_CHANNEL );
				else
					Function.Call( Hash.NETWORK_SET_VOICE_CHANNEL, (int)val );
				return val;
			};
			voice.Add( voipChannel );
			Add( new MenuItemSubMenu( client, this, voice, "Voice Settings" ) );

			var plist = new PlayerListMenu( client, this );
			Add( new MenuItemSubMenu( client, this, plist, "Players" ) );

			client.RegisterTickHandler( OnTick );
		}

		private async Task OnTick() {
			try {
				var elapsed = (float)(DateTime.UtcNow - _lastRangeChange).TotalSeconds;
				if( elapsed > 5f || _voipRange <= 0f || _voipRange > 250f ) {
					await BaseScript.Delay( 100 );
					return;
				}

				var opacity = MathUtil.Clamp( 4f - elapsed, 0f, 1f );
				World.DrawMarker( MarkerType.VerticalCylinder, Game.PlayerPed.Position - new Vector3( 0f, 0f, _voipRange / 2f ), Vector3.Zero, Vector3.Zero, Vector3.One * _voipRange * 2f,
					Color.FromArgb( (int)Math.Floor( 120 * opacity ), 255, 255, 0 ) );
			}
			catch( Exception ex ) {
				Log.Error( ex );
				await BaseScript.Delay( 1000 );
			}
		}
	}

	public class MpPlayerMenu : MenuItemSubMenu
	{
		public int ServerId { get; }
		public Player Player => new Player( API.GetPlayerFromServerId( ServerId ) );

		public MpPlayerMenu( Client client, Player player, Menu parent ) : base( client, parent, new Menu( player.Name, parent ), player.Name ) {
			ServerId = player.ServerId;

			var menu = Child;

			if( player.ServerId != Game.Player.ServerId ) {
				var spectate = new MenuItem( client, menu, "Spectate Player" );
				spectate.Activate += async () => { await Client.Tools.Spectate( player ); };
				menu.Add( spectate );
			}

			var tp = new MenuItem( client, menu, "Teleport to Player" );
			tp.Activate += () => {
				Game.PlayerPed.PositionNoOffset = Player.Character.Position;
				return Task.FromResult( 0 );
			};
			menu.Add( tp );

			var bring = new MenuItem( client, menu, "Bring Player" );
			bring.Activate += () => {
				var pos = Game.PlayerPed.Position;
				BaseScript.TriggerServerEvent( "Player.Bring", Player.ServerId, pos.X, pos.Y, pos.Z );
				return Task.FromResult( 0 );
			};
			menu.Add( bring );
		}
	}

	public class PlayerListMenu : Menu
	{
		private Client Client { get; }

		public PlayerListMenu( Client client, Menu parent ) : base( "Online Players", parent ) {
			Client = client;
			Client.RegisterTickHandler( OnTick );
		}

		private async Task OnTick() {
			try {
				if( Client.Menu.CurrentMenu != this ) {
					await BaseScript.Delay( 100 );
					return;
				}

				var players = new PlayerList();
				var all = this.OfType<MpPlayerMenu>().ToList();
				var list = all.Where( pm => pm.Player == null || players.All( pl => pl.ServerId != pm.Player.ServerId ) ).ToList();
				foreach( var player in new List<MpPlayerMenu>( list ) ) {
					if( Client.Menu.CurrentMenu != null && Client.Menu.CurrentMenu == player.Child )
						Client.Menu.CurrentMenu = player.Menu;
					Remove( player );
				}

				foreach( var player in players ) {
					if( all.Any( p => p.ServerId == player.ServerId ) ) continue;

					Add( new MpPlayerMenu( Client, player, this ) );
				}
				await BaseScript.Delay( 1000 );
			}
			catch( Exception ex ) {
				Log.Error( ex );
				await BaseScript.Delay( 1000 );
			}
		}
	}
}
