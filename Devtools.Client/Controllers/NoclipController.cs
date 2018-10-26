using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Devtools.Client.Helpers;

namespace Devtools.Client.Controllers
{
	public class NoclipController : ClientAccessor
	{
		private const float MinY = -89f, MaxY = 89f;
		private const float MaxSpeed = 32f;

		private static readonly List<Control> DisabledControls = new List<Control> {
			Control.MoveLeftOnly,
			Control.MoveLeftRight,
			Control.MoveUpDown,
			Control.MoveUpOnly,
			Control.SelectNextWeapon,
			Control.SelectPrevWeapon,
			Control.WeaponWheelLeftRight,
			Control.WeaponWheelUpDown,
			Control.WeaponWheelNext,
			Control.WeaponWheelPrev,
			Control.Duck
		};

		public bool IsEnabled { get; set; }

		public float Speed { get; set; } = 1f;

		public Camera CurrentCamera { get; set; }

		public NoclipController( Client client ) : base( client ) {
			Client.RegisterTickHandler( OnTick );
			Client.RegisterTickHandler( CheckInputRotation );
		}

		private async Task OnTick() {
			try {
				if( Game.IsControlJustPressed( 2, Control.SaveReplayClip ) ) {
					IsEnabled = !IsEnabled;
				}

				if( !IsEnabled ) {
					if( CurrentCamera != null ) {
						CurrentCamera.Delete();
						CurrentCamera = null;
						World.RenderingCamera = null;
						Game.PlayerPed.IsPositionFrozen = false;
						Game.PlayerPed.IsCollisionEnabled = true;
						Game.PlayerPed.CanRagdoll = true;
						Game.PlayerPed.IsVisible = true;
						Game.PlayerPed.Opacity = 255;
						Game.PlayerPed.Task.ClearAllImmediately();
						await BaseScript.Delay( 100 );
					}
					return;
				}

				// Create camera on toggle
				if( CurrentCamera == null ) {
					CurrentCamera = World.CreateCamera( Game.PlayerPed.Position, GameplayCamera.Rotation, 75f );
					CurrentCamera.AttachTo( Game.PlayerPed, Vector3.Zero );
					World.RenderingCamera = CurrentCamera;
					Game.PlayerPed.IsPositionFrozen = true;
					Game.PlayerPed.IsCollisionEnabled = false;
					Game.PlayerPed.Opacity = 0;
					Game.PlayerPed.CanRagdoll = false;
					Game.PlayerPed.IsVisible = false;
					Game.PlayerPed.Task.ClearAllImmediately();
				}

				// Speed Control
				if( Game.IsControlPressed( 2, Control.SelectPrevWeapon ) ) {
					Speed = Math.Min( Speed + 0.1f, MaxSpeed );
				}
				else if( Game.IsControlPressed( 2, Control.SelectNextWeapon ) ) {
					Speed = Math.Max( 0.1f, Speed - 0.1f );
				}

				var multiplier = 1f;
				if( Game.IsControlPressed( 2, Control.FrontendLs ) ) {
					multiplier = 2f;
				}
				else if( Game.IsControlPressed( 2, Control.CharacterWheel ) ) {
					multiplier = 4f;
				}
				else if( Game.IsControlPressed( 2, Control.Duck ) ) {
					multiplier = 0.25f;
				}

				// Forward
				if( Game.IsControlPressed( 2, Control.MoveUpOnly ) ) {
					Game.PlayerPed.PositionNoOffset = Game.PlayerPed.Position + CurrentCamera.UpVector * (Speed * multiplier);
				}
				// Backward
				else if( Game.IsControlPressed( 2, Control.MoveUpDown ) ) {
					Game.PlayerPed.PositionNoOffset = Game.PlayerPed.Position - CurrentCamera.UpVector * (Speed * multiplier);
				}
				// Left
				if( Game.IsControlPressed( 2, Control.MoveLeftOnly ) ) {
					var pos = Game.PlayerPed.GetOffsetPosition( new Vector3( -Speed * multiplier, 0f, 0f ) );
					Game.PlayerPed.PositionNoOffset = new Vector3( pos.X, pos.Y, Game.PlayerPed.Position.Z );
				}
				// Right
				else if( Game.IsControlPressed( 2, Control.MoveLeftRight ) ) {
					var pos = Game.PlayerPed.GetOffsetPosition( new Vector3( Speed * multiplier, 0f, 0f ) );
					Game.PlayerPed.PositionNoOffset = new Vector3( pos.X, pos.Y, Game.PlayerPed.Position.Z );
				}

				// Up (E)
				if( Game.IsControlPressed( 2, Control.Context ) ) {
					Game.PlayerPed.PositionNoOffset = Game.PlayerPed.GetOffsetPosition( new Vector3( 0f, 0f, multiplier * Speed / 2 ) );
				}

				// Down (Q)
				if( Game.IsControlPressed( 2, Control.ContextSecondary ) ) {
					Game.PlayerPed.PositionNoOffset = Game.PlayerPed.GetOffsetPosition( new Vector3( 0f, 0f, multiplier * -Speed / 2 ) );
				}


				// Disable controls
				foreach( var ctrl in DisabledControls ) {
					Game.DisableControlThisFrame( 2, ctrl );
				}

				Game.PlayerPed.Heading = Math.Max( 0f, (360 + CurrentCamera.Rotation.Z) % 360f );
				Game.PlayerPed.Opacity = 0;
				API.DisablePlayerFiring( Game.Player.Handle, false );
			}
			catch( Exception ex ) {
				Log.Error( ex );
			}
		}

		private async Task CheckInputRotation() {
			try {
				if( CurrentCamera == null ) {
					await BaseScript.Delay( 100 );
					return;
				}

				var rightAxisX = Game.GetDisabledControlNormal( 0, (Control)220 );
				var rightAxisY = Game.GetDisabledControlNormal( 0, (Control)221 );

				if( !(Math.Abs( rightAxisX ) > 0) && !(Math.Abs( rightAxisY ) > 0) ) return;
				var rotation = CurrentCamera.Rotation;
				rotation.Z += rightAxisX * -10f;

				var yValue = rightAxisY * -5f;
				if( rotation.X + yValue > MinY && rotation.X + yValue < MaxY )
					rotation.X += yValue;
				CurrentCamera.Rotation = rotation;
			}
			catch( Exception ex ) {
				Log.Error( ex );
			}
		}
	}
}