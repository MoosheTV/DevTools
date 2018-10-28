using System;
using System.Threading.Tasks;

namespace Devtools.Client.Menus
{
	public class MenuItem
	{
		private static int _menuItems = short.MaxValue;

		internal Func<Task> Select, Activate, Left, Right;

		public Menu Menu { get; }

		public int Priority { get; }

		public string Label { get; set; }

		protected Client Client { get; }

		public virtual bool IsVisible { get; set; } = true;

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
			return Task.FromResult( 0 );
		}

		protected virtual Task OnActivate() {
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
