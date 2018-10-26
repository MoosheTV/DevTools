using System;
using System.Threading.Tasks;
using CitizenFX.Core.Native;

namespace Devtools.Client.Menus
{
	public class MenuItem
	{
		private static int _menuItems = short.MaxValue;

		internal Func<Task> Select, Activate, Left, Right;

		protected Menu Menu { get; }

		public int Priority { get; }

		public string Label { get; set; }

		protected Client Client { get; }

		public MenuItem( Client client, Menu owner, string label, int priority = -1 ) {
			Client = client;
			Menu = owner;
			Label = label;
			_menuItems--;
			Priority = priority < 0 ? _menuItems : priority;

			Select += OnSelect;
			Activate += OnActivate;
			Left += OnLeft;
			Right += OnRight;
		}

		protected virtual Task OnSelect() {
			API.PlaySoundFrontend( -1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", true );
			return Task.FromResult( 0 );
		}

		protected virtual Task OnActivate() {
			API.PlaySoundFrontend( -1, "SELECT", "HUD_FREEMODE_SOUNDSET", false );
			return Task.FromResult( 0 );
		}

		protected virtual Task OnLeft() {
			return Task.FromResult( 0 );
		}

		protected virtual Task OnRight() {
			return Task.FromResult( 0 );
		}
	}
}
