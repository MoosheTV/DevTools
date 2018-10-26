using System;
using System.Threading.Tasks;

namespace Devtools.Client.Menus
{
	public class MenuItemCheckbox : MenuItem
	{
		private bool _isChecked;
		internal Func<bool> IsChecked;

		public MenuItemCheckbox( Client client, Menu owner, string label, bool isChecked = false, int priority = -1 ) : base( client, owner, label, priority ) {
			_isChecked = isChecked;
			IsChecked = () => _isChecked;
		}

		protected override Task OnActivate() {
			_isChecked = !_isChecked;
			return base.OnActivate();
		}
	}
}
