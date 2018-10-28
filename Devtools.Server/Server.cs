using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using Devtools.Client.Helpers;

// ReSharper disable ClassNeverInstantiated.Global

namespace Devtools.Server
{
	public class Server : BaseScript
	{
		public HttpHandler Http { get; }
		public PlayerController PlayerManager { get; }

		public Server() {
			Log.Info( "Instantiating DevTools" );
			Http = new HttpHandler( this );
			new VersionCheck( this );
			PlayerManager = new PlayerController( this );
			Log.Success( "Successfully Instantiated DevTools" );
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

		public void RegisterExport( string exportName, Delegate callback ) {
			Exports.Add( exportName, callback );
		}

		public dynamic GetExport( string resourceName ) {
			return Exports[resourceName];
		}

	}
}
