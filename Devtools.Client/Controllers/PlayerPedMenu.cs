using System;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using Devtools.Client.Helpers;
using Devtools.Client.Menus;

namespace Devtools.Client.Controllers
{
	public class PlayerPedMenu : Menu
	{
		public PlayerPedMenu( Client client, Menu parent ) : base( "Ped Models", parent ) {
			var inputModel = new MenuItem( client, this, "Input Model" );
			inputModel.Activate += async () => {
				try {
					var input = await UiHelper.PromptTextInput( controller: client.Menu );

					Model model = null;
					var enumName = Enum.GetNames( typeof( PedHash ) ).FirstOrDefault( s => s.ToLower().StartsWith( input.ToLower() ) ) ?? "";
					var modelName = "";

					if( int.TryParse( input, out var hash ) ) {
						model = new Model( hash );
						modelName = $"{hash}";
					}
					else if( !string.IsNullOrEmpty( enumName ) ) {
						var found = false;
						foreach( PedHash p in Enum.GetValues( typeof( PedHash ) ) ) {
							if( !(Enum.GetName( typeof( PedHash ), p )?.Equals( enumName, StringComparison.InvariantCultureIgnoreCase ) ??
								  false) ) {
								continue;
							}

							model = new Model( p );
							modelName = enumName;
							found = true;
							break;
						}

						if( !found ) {
							UiHelper.ShowNotification( $"~r~ERROR~s~: Could not load model {input}" );
							return;
						}
					}
					else {
						model = new Model( input );
						modelName = input;
					}

					if( !await model.Request( 10000 ) || !await Game.Player.ChangeModel( model ) ) {
						UiHelper.ShowNotification( "~r~ERROR~s~: Failed to load ped model." );
					}
					else {
						UiHelper.ShowNotification( $"~g~Success~s~: Changed ped model to ~y~{modelName}~s." );
					}
				}
				catch( Exception ex ) {
					Log.Error( ex );
				}
			};
			Add( inputModel );

			foreach( PedHash hash in Enum.GetValues( typeof( PedHash ) ) ) {
				Add( new MenuItemPedModel( client, this, new Model( hash ), Enum.GetName( typeof( PedHash ), hash ) ) );
			}
		}

		private class MenuItemPedModel : MenuItem
		{
			private Model Model { get; }

			public MenuItemPedModel( Client client, Menu owner, Model model, string modelName, int priority = -1 ) : base( client, owner, modelName, priority ) {
				Model = model;
			}

			protected override async Task OnActivate() {
				if( await Model.Request( 10000 ) ) {
					await Game.Player.ChangeModel( Model );
				}

				await base.OnActivate();
			}
		}
	}
}
