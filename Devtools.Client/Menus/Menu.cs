using System;
using System.Collections.Generic;
using System.Linq;
using CitizenFX.Core.Native;

namespace Devtools.Client.Menus
{
	public class Menu : List<MenuItem>
	{
		public string Title { get; protected set; }
		public Menu Parent { get; }

		private int _index;
		public int CurrentIndex
		{
			get => Math.Max( 0, Math.Min( _index, Count - 1 ) );
			set {
				if( value < 0 ) {
					value = Count - 1;
				}
				_index = Math.Max( 0, value % Count );

				var item = this.ElementAt( _index );
				item.Select.Invoke();
			}
		}

		public MenuItem CurrentItem => this.ElementAt( CurrentIndex );

		public Menu( string title, Menu parent = null ) {
			Title = title;
			Parent = parent;
		}

		public virtual void OnEnter() {
			if( Parent == null )
				API.PlaySoundFrontend( -1, "SELECT", "HUD_FREEMODE_SOUNDSET", false );
		}

		public virtual void OnExit() {
			if( Parent == null )
				API.PlaySoundFrontend( -1, "BACK", "HUD_FREEMODE_SOUNDSET", false );
		}

		public new bool Remove( MenuItem i ) {
			var remove = base.Remove( i );
			Refresh();
			return remove;
		}

		public new void Add( MenuItem item ) {
			base.Add( item );
			Refresh();
		}

		public new void AddRange( IEnumerable<MenuItem> items ) {
			base.AddRange( items );
			Refresh();
		}

		private void Refresh() {
			Sort( ( a, b ) => b.Priority - a.Priority );
			CurrentIndex = _index;
		}
	}

}
