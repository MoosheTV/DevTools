using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Devtools.Client.Helpers;
using Devtools.Client.Menus;

namespace Devtools.Client.Controllers
{
	public class TimeMenu : Menu
	{
		public DateTime CurrentDate
		{
			get {
				var day = Function.Call<int>( Hash.GET_CLOCK_DAY_OF_MONTH );
				var month = Function.Call<int>( Hash.GET_CLOCK_MONTH );
				var year = Function.Call<int>( Hash.GET_CLOCK_YEAR );
				return new DateTime( year, month, day, 0, 0, 0 );
			}
			set => Function.Call( Hash.SET_CLOCK_DATE, value.Day, value.Month, value.Year );
		}

		// NOTE: 1:1 is 48 minutes (GTA Standard). To calculate a multiplier given minutes, use equation: (86400 / (mins * 60)) / 30
		public float SpeedMultiplier { get; set; }

		public int Hour
		{
			get => _pauseClock ? _pauseHour : Function.Call<int>( Hash.GET_CLOCK_HOURS );
			set => Function.Call( Hash.NETWORK_OVERRIDE_CLOCK_TIME, MathUtil.Clamp( value % 24, 0, 23 ), Minute, Second );
		}

		public int Minute
		{
			get => _pauseClock ? _pauseMinute : Function.Call<int>( Hash.GET_CLOCK_MINUTES );
			set => Function.Call( Hash.NETWORK_OVERRIDE_CLOCK_TIME, Hour, MathUtil.Clamp( value % 60, 0, 59 ), Second );
		}

		public int Second
		{
			get => _pauseClock ? _pauseSecond : Function.Call<int>( Hash.GET_CLOCK_SECONDS );
			set => Function.Call( Hash.NETWORK_OVERRIDE_CLOCK_TIME, Hour, Minute, MathUtil.Clamp( value % 60, 0, 59 ) );
		}

		private bool _pauseClock;
		private int _pauseHour, _pauseMinute, _pauseSecond;
		public bool PauseClock
		{
			get => _pauseClock;
			set {
				Function.Call( Hash.PAUSE_CLOCK, value );
				_pauseHour = Hour;
				_pauseMinute = Minute;
				_pauseSecond = Second;
				_pauseClock = value;
			}
		}

		private DateTime _lastUpdate;

		public TimeMenu( Client client, Menu parent ) : base( "Time Menu", parent ) {
			var freeze = new MenuItemCheckbox( client, this, "Freeze Time" ) {
				IsChecked = () => PauseClock
			};
			freeze.Activate += () => {
				PauseClock = !PauseClock;
				return Task.FromResult( 0 );
			};
			Add( freeze );

			var speed = new MenuItemSpinnerF( client, this, "Speed Multiplier", 1f, 0.1f, 30f, 0.1f, true );
			speed.ValueUpdate += val => {
				SpeedMultiplier = val;
				return val;
			};
			Add( speed );

			var hour = new MenuItemSpinnerInt( client, this, "Hour", Hour, 0, 23, 1, true );
			hour.ValueUpdate += ( val ) => {
				Hour = val;
				return val;
			};
			Add( hour );

			var min = new MenuItemSpinnerInt( client, this, "Minute", Minute, 0, 59, 1, true );
			min.ValueUpdate += val => {
				Minute = val;
				return val;
			};
			Add( min );

			var sec = new MenuItemSpinnerInt( client, this, "Second", Second, 0, 59, 1, true );
			sec.ValueUpdate += val => {
				Second = val;
				return val;
			};
			Add( sec );

			_lastUpdate = DateTime.UtcNow;
			client.RegisterTickHandler( OnTick );
		}

		private async Task OnTick() {
			try {
				var ms = (float)(DateTime.UtcNow - _lastUpdate).TotalMilliseconds;

				var secondsOfDay = (float)(Hour * 3600 + Minute * 60 + Second);
				if( !PauseClock )
					secondsOfDay += (ms * (30f / 1000)) * SpeedMultiplier;
				if( secondsOfDay >= 86400 ) {
					secondsOfDay = secondsOfDay % 86400;
					CurrentDate = CurrentDate.AddDays( 1 );
				}

				Hour = (int)Math.Floor( secondsOfDay / 3600 ) % 24;
				Minute = (int)Math.Floor( secondsOfDay / 60 ) % 60;
				Second = (int)Math.Floor( secondsOfDay % 60 );

				_lastUpdate = DateTime.UtcNow;

				if( !PauseClock )
					await BaseScript.Delay( (int)Math.Floor( 30 / SpeedMultiplier ) / 1000 );
			}
			catch( Exception ex ) {
				Log.Error( ex );
			}
		}
	}
}
