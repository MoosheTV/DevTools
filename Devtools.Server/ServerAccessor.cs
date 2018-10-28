namespace Devtools.Server
{
	public class ServerAccessor
	{
		protected Server Server { get; }

		public ServerAccessor( Server server ) {
			Server = server;
		}
	}
}
