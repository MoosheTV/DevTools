using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using Devtools.Client.Helpers;

namespace Devtools.Client.Menus
{
	public class MenuController : ClientAccessor
	{
		private const int HoldDownInterval = 25; // How long it waits before triggering the menu action while holding down a control.
		private const int MaxItemsVisible = 7; // Maximum amount of items which can be visible at any given time.

		private const float MenuWidth = 0.20925925925f;
		private const float MenuPosX = 0.8739583333f;

		private const float HeaderPosY = 0.07356770832f;
		private const float HeaderHeight = 0.07518518518f;
		private static readonly Color HeaderColor = Color.FromArgb( 200, 144, 45, 45 );

		private const float InfoHeight = 0.02488888888f;
		private const float InfoOffsetY = 0.04976666666f;
		private static readonly Color InfoColor = Color.FromArgb( 200, 45, 45, 45 );
		private static readonly Color InfoTextColor = Color.FromArgb( 120, 255, 255, 255 );

		private const float ItemHeight = 0.032f;
		private const float ItemOffsetY = 0.02911111111f;
		private static readonly Color ItemColor = Color.FromArgb( 230, 20, 20, 20 );
		private static readonly Color ActiveItemColor = Color.FromArgb( 200, 144, 144, 144 );

		private static readonly Color TextColor = Color.FromArgb( 255, 255, 255 );
		private static readonly Color ActiveTextColor = Color.FromArgb( 255, 20, 20, 20 );

		private const float CheckboxWidth = 48 / 1920f;
		private const float CheckboxHeight = 48 / 1080f;
		private const float CheckboxInnerWidth = 44 / 1920f;
		private const float CheckboxInnerHeight = 44 / 1080f;

		private readonly AspectRatioCache _aspectRatio = new AspectRatioCache();
		public float AspectRatio => _aspectRatio.Value;

		private Menu _currentMenu;
		public Menu CurrentMenu
		{
			get => _currentMenu;
			set {
				_currentMenu?.OnExit();
				_currentMenu = value;
				_currentMenu.OnEnter();
			}
		}

		public bool DisableControls { get; set; }

		private int _viewportMin, _viewportMax = MaxItemsVisible;

		private readonly Dictionary<Control, bool> _lastPresssed = new Dictionary<Control, bool> {
			[Control.FrontendLeft] = false,
			[Control.FrontendRight] = false,
			[Control.FrontendUp] = false,
			[Control.FrontendDown] = false,
			[Control.FrontendAccept] = false,
			[Control.FrontendCancel] = false
		};

		private readonly Dictionary<Control, Menu> _hotkeyMenus = new Dictionary<Control, Menu>();

		public MenuController( Client client ) : base( client ) {
			Client.RegisterTickHandler( OnControlTick );
			Client.RegisterTickHandler( OnRenderTick );
		}

		public bool RegisterMenuHotkey( Control control, Menu menu ) {
			if( _hotkeyMenus.ContainsKey( control ) ) {
				return false;
			}

			_hotkeyMenus.Add( control, menu );
			return true;
		}

		#region Drawing
		private async Task OnRenderTick() {
			try {
				if( CurrentMenu == null ) {
					await BaseScript.Delay( 100 );
					return;
				}

				// Viewport
				var idx = CurrentMenu.CurrentIndex;
				var currentItem = CurrentMenu.CurrentItem;
				var visibleItems = CurrentMenu.GetVisibleItems().ToList();
				var vCount = visibleItems.Count;
				if( _viewportMin >= idx ) {
					_viewportMin = idx;
					_viewportMax = Math.Min( vCount - 1, idx + MaxItemsVisible );
				}
				if( _viewportMin <= idx - MaxItemsVisible - 1 ) {
					_viewportMin = Math.Max( 0, idx - MaxItemsVisible );
				}

				if( _viewportMax <= idx ) {
					_viewportMax = idx;
					_viewportMin = Math.Max( 0, idx - MaxItemsVisible );
				}
				if( _viewportMax - _viewportMin < MaxItemsVisible || _viewportMax > idx + MaxItemsVisible - 1 ) {
					_viewportMax = Math.Min( vCount - 1, idx + MaxItemsVisible );
				}

				// Header
				var yOffset = HeaderPosY;
				UiHelper.DrawRect( MenuPosX, yOffset, MenuWidth, HeaderHeight, HeaderColor );
				UiHelper.DrawText( CurrentMenu.Title, new Vector2( MenuPosX, HeaderPosY - 0.02f ), TextColor, 0.75f,
									alignment: Alignment.Center, font: Font.HouseScript );

				// Info
				yOffset += InfoOffsetY;
				UiHelper.DrawRect( MenuPosX, yOffset, MenuWidth, InfoHeight, InfoColor );
				UiHelper.DrawText( $"{idx + 1} / {vCount}", new Vector2( MenuPosX - (MenuWidth / 2 - 0.002f),
									yOffset - InfoHeight / 2 + 0.0024f ), InfoTextColor );

				// Items
				yOffset += ItemOffsetY - 0.0002f;
				var count = 0;
				foreach( var item in visibleItems.Where( p => count >= _viewportMin & count++ <= _viewportMax ) ) {
					var isSelected = currentItem == item;
					UiHelper.DrawRect( MenuPosX, yOffset, MenuWidth, ItemHeight, isSelected ? ActiveItemColor : ItemColor );

					if( item is MenuItemCheckbox cbox ) {
						HandleCheckboxRender( cbox, yOffset, isSelected );
					}
					else if( item is MenuItemSpinner spin ) {
						HandleSpinnerRender( spin, yOffset, isSelected );
					}
					else {
						HandleStandardRender( item, yOffset, isSelected );
					}

					yOffset += ItemOffsetY + 0.0028f;
				}
			}
			catch( Exception ex ) {
				Log.Error( ex );
			}
		}

		private void HandleSpinnerRender( MenuItemSpinner type, float yOffset, bool isSelected ) {
			const float xPos = MenuPosX + (MenuWidth / 2);
			var text = new Text( $"{(isSelected ? "< " : "")}{type.GetDisplay()}{(isSelected ? " >" : "")}", new PointF( xPos, yOffset - 0.012f ), 0.25f, TextColor, Font.ChaletLondon, Alignment.Right );
			UiHelper.DrawText( $"{(isSelected ? "< " : "")}{type.GetDisplay()}{(isSelected ? " >" : "")}", new Vector2( xPos - text.ScaledWidth / 1920f, yOffset - 0.01f ), isSelected ? TextColor : ActiveItemColor, alignment: Alignment.Center );
			HandleStandardRender( type, yOffset, isSelected );
		}

		private void HandleCheckboxRender( MenuItemCheckbox item, float yOffset, bool isSelected ) {
			const float scaleX = CheckboxWidth * 0.95f / 2;
			const float scaleY = CheckboxHeight * 0.95f / 2;
			const float innerScaleX = CheckboxInnerWidth * 0.95f / 2;
			const float innerScaleY = CheckboxInnerHeight * 0.95f / 2;
			const float centerPosX = MenuPosX - (MenuWidth / 2) * 0.9f;
			UiHelper.DrawRect( centerPosX, yOffset, scaleX, scaleY, isSelected ? TextColor : ActiveItemColor );
			if( !item.IsChecked.Invoke() ) {
				UiHelper.DrawRect( centerPosX, yOffset, innerScaleX, innerScaleY, ItemColor );
			}

			UiHelper.DrawText( item.Label, new Vector2( MenuPosX - (MenuWidth / 2 - 0.003f - (scaleX * 1.25f)), yOffset - (ItemHeight / 2 - 0.0036f) ), isSelected ? ActiveTextColor : TextColor, 0.3f );
		}

		private void HandleStandardRender( MenuItem item, float yOffset, bool isSelected ) {
			UiHelper.DrawText( item.Label, new Vector2( MenuPosX - (MenuWidth / 2 - 0.003f), yOffset - (ItemHeight / 2 - 0.0036f) ), isSelected ? ActiveTextColor : TextColor, 0.3f );
		}
		#endregion

		#region Controls
		private async Task OnControlTick() {
			try {
				if( DisableControls ) {
					return;
				}

				if( CurrentMenu == null ) {
					foreach( var kvp in _hotkeyMenus ) {
						if( !Game.IsControlJustPressed( 2, kvp.Key ) ) continue;

						CurrentMenu = kvp.Value;
						API.PlaySoundFrontend( -1, "SELECT", "HUD_FREEMODE_SOUNDSET", false );

						// Wait until the player releases the key to not interfere w/ Menu action controls
						while( true ) {
							await BaseScript.Delay( 100 );
							if( !Game.IsControlPressed( 2, kvp.Key ) ) break;
						}
					}
					return;
				}

				// Listen to all controls for triggering menu actions.
				foreach( var kvp in new Dictionary<Control, bool>( _lastPresssed ) ) {
					if( Game.IsControlPressed( 2, kvp.Key ) ) {
						OnMenuControl( kvp.Key );
						if( !kvp.Value ) {
							var start = DateTime.Now;
							while( true ) {
								if( !Game.IsControlPressed( 2, kvp.Key ) ) return;

								if( (DateTime.Now - start).TotalMilliseconds > HoldDownInterval * 12 ) break;

								await BaseScript.Delay( 10 );
							}
							_lastPresssed[kvp.Key] = true;
						}
						else {
							await BaseScript.Delay( HoldDownInterval );
						}
					}
					else if( kvp.Value ) {
						_lastPresssed[kvp.Key] = false;
					}

					Game.DisableControlThisFrame( 2, kvp.Key );
				}
			}
			catch( Exception ex ) {
				Log.Error( ex );
			}
		}

		private void OnMenuControl( Control control ) {
			try {
				if( CurrentMenu == null ) return;

				switch( control ) {
				case Control.FrontendDown:
					API.PlaySoundFrontend( -1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", true );
					CurrentMenu.CurrentIndex++;
					CurrentMenu.CurrentItem.Select.Invoke();
					break;

				case Control.FrontendUp:
					API.PlaySoundFrontend( -1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", true );
					CurrentMenu.CurrentIndex--;
					CurrentMenu.CurrentItem.Select.Invoke();
					break;

				case Control.FrontendLeft:
					CurrentMenu.CurrentItem.Left.Invoke();
					break;

				case Control.FrontendRight:
					CurrentMenu.CurrentItem.Right.Invoke();
					break;

				case Control.FrontendCancel:
					API.PlaySoundFrontend( -1, "BACK", "HUD_FREEMODE_SOUNDSET", false );
					CurrentMenu = CurrentMenu.Parent;
					break;

				case Control.FrontendAccept:
					API.PlaySoundFrontend( -1, "SELECT", "HUD_FREEMODE_SOUNDSET", false );
					CurrentMenu.CurrentItem.Activate.Invoke();
					break;
				}
			}
			catch( Exception ) {
				// ignored
			}
		}
		#endregion
	}

	internal class AspectRatioCache : CachedValue<float>
	{
		public AspectRatioCache() : base( 5000 ) {
		}

		protected override float Update() {
			return API.GetAspectRatio( true );
		}
	}
}
