using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using Devtools.Client.Controllers;
using Devtools.Client.Menus;

// ReSharper disable ClassNeverInstantiated.Global

namespace Devtools.Client
{
	public class Client : BaseScript
	{
		public DevTools Tools { get; }
		public MenuController Menu { get; }
		public NoclipController NoClip { get; }
		public EntityDebugger Debugger { get; }

		private bool _isInstantiated;

		public Client() {
			Menu = new MenuController( this );
			Tools = new DevTools( this );
			NoClip = new NoclipController( this );
			Debugger = new EntityDebugger( this );

			RegisterEventHandler( "playerSpawned", new Action( OnSpawn ) );

			if( Game.PlayerPed.Position != default( Vector3 ) )
				OnSpawn();
		}

		private void OnSpawn() {
			if( !_isInstantiated )
				TriggerServerEvent( "Client.Ready" );
			_isInstantiated = true;
		}

		public void RegisterEventHandler( string eventName, Delegate action ) {
			EventHandlers[eventName] += action;
		}

		public void RegisterTickHandler( Func<Task> tick ) {
			Tick += tick;
		}

		public void DeregisterTickHandler( Func<Task> tick ) {
			Tick -= tick;
		}
	}
}
