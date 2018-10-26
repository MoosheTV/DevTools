using System;
using System.Drawing;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using Devtools.Client.Menus;

namespace Devtools.Client.Helpers
{
	public static class UiHelper
	{
		public static readonly Color DefaultColor = Color.FromArgb( 255, 255, 255 );

		public static void DrawText( string text, Vector2 pos, Color? color = null, float scale = 0.25f,
			bool shadow = false, float shadowOffset = 1f, Alignment alignment = Alignment.Left, Font font = Font.ChaletLondon ) {
			try {
				Function.Call( Hash.SET_TEXT_FONT, font );
				Function.Call( Hash.SET_TEXT_PROPORTIONAL, 0 );
				Function.Call( Hash.SET_TEXT_SCALE, scale, scale );
				if( shadow ) {
					Function.Call( Hash.SET_TEXT_DROPSHADOW, shadowOffset, 0, 0, 0, 255 );
				}
				var col = color ?? DefaultColor;
				Function.Call( Hash.SET_TEXT_COLOUR, col.R, col.G, col.B, col.A );
				Function.Call( Hash.SET_TEXT_EDGE, 1, 0, 0, 0, 255 );
				Function.Call( Hash.SET_TEXT_JUSTIFICATION, alignment );
				Function.Call( Hash._SET_TEXT_ENTRY, "STRING" );
				Function.Call( Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text );
				Function.Call( Hash._DRAW_TEXT, pos.X, pos.Y );
			}
			catch( Exception ex ) {
				Log.Error( ex );
			}
		}

		public static void DrawRect( float xPos, float yPos, float xScale, float yScale, Color color ) {
			try {
				Function.Call( Hash.DRAW_RECT, xPos, yPos, xScale, yScale, color.R, color.G, color.B, color.A );
			}
			catch( Exception ex ) {
				Log.Error( ex );
			}
		}

		public static void ShowNotification( string text ) {
			Function.Call( Hash._SET_NOTIFICATION_TEXT_ENTRY, "STRING" );
			Function.Call( Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text );
			Function.Call( Hash._DRAW_NOTIFICATION, false, false );
		}

		public static void Draw3DText( string text, Vector3 position, Color color, float scale = 1f,
			Font font = Font.ChaletLondon, bool center = true ) {
			var pos = WorldToScreen( position );
			var cam = API.GetGameplayCamCoords();
			var dist = Math.Max( 0.0001f, Math.Sqrt( cam.DistanceToSquared( position ) ) );
			var textScale = (float)(1f / dist * 2f * (1f / World.RenderingCamera.FieldOfView * 100f) * scale);
			DrawText( text, pos, color, textScale, false, 1f, center ? Alignment.Center : Alignment.Left, font );
		}

		public static void DrawCrosshair( Color? crosshairColor = null ) {
			var color = crosshairColor ?? DefaultColor;
			DrawRect( 0.5f, 0.5f, 0.008333333f, 0.001851852f, color );
			DrawRect( 0.5f, 0.5f, 0.001041666f, 0.014814814f, color );
		}

		public static Vector2 WorldToScreen( Vector3 position ) {
			var screenX = new OutputArgument();
			var screenY = new OutputArgument();
			return !Function.Call<bool>( Hash._WORLD3D_TO_SCREEN2D, position.X, position.Y, position.Z, screenX, screenY ) ?
				Vector2.Zero :
				new Vector2( screenX.GetResult<float>(), screenY.GetResult<float>() );
		}

		public static async Task<string> PromptTextInput( string defaultText = "", int maxChars = 80, MenuController controller = null ) {
			var startValue = controller?.DisableControls ?? false;
			if( controller != null ) {
				controller.DisableControls = true;
			}

			var result = "";
			try {
				API.DisplayOnscreenKeyboard( 0, "FMMC_MPM_NA", "", defaultText, "", "", "", maxChars );
				while( API.UpdateOnscreenKeyboard() == 0 ) {
					Game.DisableAllControlsThisFrame( 2 );
					await BaseScript.Delay( 0 );
				}
				result = API.GetOnscreenKeyboardResult();
			}
			catch( Exception ex ) {
				Log.Error( ex );
			}

			if( controller != null ) {
				await BaseScript.Delay( 0 );
				controller.DisableControls = startValue;
			}
			return result;
		}

	}
}
