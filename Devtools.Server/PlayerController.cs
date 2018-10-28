using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using Devtools.Client.Helpers;

namespace Devtools.Server
{
	public class PlayerController : ServerAccessor
	{
		public PlayerController( Server server ) : base( server ) {
			Server.RegisterEventHandler( "Player.Bring", new Action<Player, int, float, float, float>( OnPlayerBring ) );
		}

		private void OnPlayerBring( [FromSource] Player source, int serverId, float x, float y, float z ) {
			try {
				var target = new PlayerList().FirstOrDefault( p => p.Handle == $"{serverId}" );
				if( target == null ) {
					Log.Warn( $"Player {source.Name} (net:{source.Handle}) tried to bring non-existent player from handle {serverId}." );
					return;
				}

				Log.Verbose( $"Player {source.Name} (net:{source.Handle}) brought player {target.Name} (net:{target.Handle})" );
				target.TriggerEvent( "Player.Bring", int.Parse( source.Handle ), x, y, z );
			}
			catch( Exception ex ) {
				Log.Error( ex );
			}
		}
	}
}
