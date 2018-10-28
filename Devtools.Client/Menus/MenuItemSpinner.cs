using System;
using System.Threading.Tasks;
using CitizenFX.Core.Native;

namespace Devtools.Client.Menus
{
	public class MenuItemSpinner : MenuItem
	{
		internal Func<dynamic, dynamic> ValueUpdate;

		protected dynamic MinValue { get; }
		protected virtual dynamic MaxValue { get; }
		protected dynamic Step { get; }

		protected bool Modulus { get; }

		private dynamic _value;
		public virtual dynamic Value
		{
			get => _value;
			set {
				if( Modulus ) {
					if( value < 0 ) {
						_value = MaxValue;
					}
					else {
						_value = value % (MaxValue + 1);
					}
				}
				else {
					_value = value;
				}

				_value = ValueUpdate.Invoke( _value );
			}
		}

		public MenuItemSpinner( Client client, Menu owner, string label, dynamic defaultValue, dynamic min, dynamic max, dynamic step, bool modulus = false, int priority = -1 ) : base( client, owner, label, priority ) {
			MinValue = min;
			MaxValue = max;
			Step = step;
			_value = defaultValue;
			Modulus = modulus;
		}

		public virtual string GetDisplay() {
			return Value.ToString();
		}

		protected override Task OnLeft() {
			API.PlaySoundFrontend( -1, "NAV_LEFT_RIGHT", "HUD_FREEMODE_SOUNDSET", false );
			return base.OnLeft();
		}

		protected override Task OnRight() {
			API.PlaySoundFrontend( -1, "NAV_LEFT_RIGHT", "HUD_FREEMODE_SOUNDSET", false );
			return base.OnRight();
		}
	}
}
