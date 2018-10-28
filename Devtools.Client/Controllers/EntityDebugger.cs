using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Devtools.Client.Helpers;

namespace Devtools.Client.Controllers
{
	public class EntityDebugger : ClientAccessor
	{
		private static readonly Color DefaultCrosshair = Color.FromArgb( 120, 120, 120 );
		private static readonly Color ActiveCrosshair = Color.FromArgb( 255, 255, 255 );

		private static readonly Vector2 DefaultPos = new Vector2( 0.6f, 0.5f );

		private static readonly PlayerList Players = new PlayerList();

		public bool IsEnabled { get; set; }

		public Entity _trackingEntity;
		private bool _track;

		public EntityDebugger( Client client ) : base( client ) {
			Client.RegisterTickHandler( OnTick );
		}

		private Dictionary<string, string> GetDataFor( Entity entity ) {
			var list = new Dictionary<string, string>();
			try {
				var pos = entity.Position;
				var rot = entity.Rotation;
				var vel = entity.Velocity;
				list["Model Name"] = GetModelName( entity.Model );
				list["Model Hash"] = $"{entity.Model.Hash}";
				list["Model Hash (Hex)"] = $"0x{entity.Model.Hash:X}";
				list[""] = "";

				var player = Players.FirstOrDefault( p => p.Character == entity );
				if( player != null ) {
					list["Player Name"] = player.Name;
					list["Server ID"] = $"{player.ServerId}";
					list["Is Talking"] = $"{(Function.Call<bool>( Hash.NETWORK_IS_PLAYER_TALKING, player ) ? "~g~True" : "~r~False")}~s~";
					list["  "] = "";
					list["Health"] = $"{player.Character.Health} / {player.Character.MaxHealth}";
					list["Invincible"] = $"{(player.IsInvincible ? "~g~True" : "~r~False")}~s~";
				}
				else if( entity is Vehicle veh ) {
					list["Engine Health"] = $"{veh.EngineHealth:n1} / 1,000.0";
					list["Body Health"] = $"{veh.BodyHealth:n1} / 1,000.0";
					list["Speed"] = $"{veh.Speed / 0.621371f:n3} MP/H";
					list["RPM"] = $"{veh.CurrentRPM:n3}";
					list["Current Gear"] = $"{veh.CurrentGear}";
					list["Acceleration"] = $"{veh.Acceleration:n3}";
				}
				else {
					list["Health"] = $"{entity.Health} / {entity.MaxHealth}";
				}
				list[" "] = "";
				list["Distance"] = $"{Math.Sqrt( Game.PlayerPed.Position.DistanceToSquared( entity.Position ) ):n3} Meters";
				list["Heading"] = $"{entity.Heading:n3}";
				list["Position"] = $"{pos.X:n5} {pos.Y:n5} {pos.Z:n5}";
				list["Rotation"] = $"{rot.X:n5} {rot.Y:n5} {rot.Z:n5}";
				list["Velocity"] = $"{vel.X:n5} {vel.Y:n5} {vel.Z:n5}";
			}
			catch( Exception ex ) {
				Log.Error( ex );
			}
			return list;
		}

		private async Task OnTick() {
			try {
				if( Game.IsControlJustPressed( 2, Control.ReplayStartStopRecordingSecondary ) ) {
					IsEnabled = !IsEnabled;
					UiHelper.ShowNotification( $"Entity Debugger is now {(IsEnabled ? "~g~Enabled" : "~r~Disabled")}~s~." );
				}

				if( !IsEnabled ) {
					return;
				}

				var rightClick = Game.IsControlPressed( 2, Control.Aim );

				if( rightClick ) {
					_trackingEntity = GetEntityInCrosshair();
					if( _trackingEntity == null )
						_track = false;
				}

				if( _trackingEntity == null && !_track ) {
					_trackingEntity = GetEntityInCrosshair();
				}


				var color = DefaultCrosshair;
				if( _trackingEntity != null ) {
					color = ActiveCrosshair;
				}

				UiHelper.DrawCrosshair( color );

				if( _trackingEntity != null ) {
					if( rightClick ) {
						_track = true;
					}
					var playerPos = Game.PlayerPed.Position;
					if( !_trackingEntity.Exists() || playerPos.DistanceToSquared( _trackingEntity.Position ) > 1000f ) {
						_track = false;
						_trackingEntity = null;
						return;
					}

					DrawData( _trackingEntity );
				}
			}
			catch( Exception ex ) {
				Log.Error( ex );
				await BaseScript.Delay( 100 );
			}
		}

		private Entity GetEntityInCrosshair() {
			var raycast = World.Raycast( GameplayCamera.Position, CameraForwardVec(), 100f, IntersectOptions.Everything, Game.PlayerPed );
			if( !raycast.DitHit || !raycast.DitHitEntity || raycast.HitPosition == default( Vector3 ) ) {
				return null;
			}
			return raycast.HitEntity;
		}

		private Vector3 CameraForwardVec() {
			if( Client.NoClip.IsEnabled ) {
				return Client.NoClip.CurrentCamera.UpVector;
			}
			var rotation = (float)(Math.PI / 180.0) * Function.Call<Vector3>( Hash.GET_GAMEPLAY_CAM_ROT, 2 );
			return Vector3.Normalize( new Vector3( (float)-Math.Sin( rotation.Z ) * (float)Math.Abs( Math.Cos( rotation.X ) ), (float)Math.Cos( rotation.Z ) * (float)Math.Abs( Math.Cos( rotation.X ) ), (float)Math.Sin( rotation.X ) ) );
		}

		private void DrawData( Entity entity ) {
			var entityPos = entity.Position;
			var pos = UiHelper.WorldToScreen( entityPos );
			if( pos.X <= 0f || pos.Y <= 0f || pos.X >= 1f || pos.Y >= 1f ) {
				pos = DefaultPos;
			}
			var dist = (float)Math.Sqrt( Game.PlayerPed.Position.DistanceToSquared( entityPos ) );
			var offsetX = MathUtil.Clamp( (1f - dist / 100f) * 0.1f, 0f, 0.1f );
			pos.X += offsetX;

			var data = GetDataFor( entity );

			// Draw Box
			UiHelper.DrawRect( pos.X + 0.12f, pos.Y, 0.24f, data.Count * 0.024f + 0.048f, Color.FromArgb( 120, 0, 0, 0 ) );

			var offsetY = data.Count * 0.012f;
			pos.Y -= offsetY;

			pos.X += 0.02f;
			// Draw data
			foreach( var entry in data ) {
				if( !string.IsNullOrEmpty( entry.Value ) )
					UiHelper.DrawText( $"{entry.Key}: {entry.Value}", pos );
				pos.Y += 0.024f;
			}
		}

		private static string GetModelName( Model model ) {
			var name = "";
			if( model.IsVehicle ) {
				name = Enum.GetName( typeof( VehicleHash ), (VehicleHash)model.Hash ) ?? "";
			}

			if( model.IsProp ) {
				name = Enum.GetName( typeof( ObjectHash ), (ObjectHash)model.Hash ) ?? "";
			}

			if( string.IsNullOrEmpty( name ) ) {
				name = Enum.GetName( typeof( PedHash ), (PedHash)model.Hash ) ?? "Unknown";
			}
			return name;
		}

	}
}
