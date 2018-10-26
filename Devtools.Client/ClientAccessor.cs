namespace Devtools.Client
{
	public class ClientAccessor
	{
		protected Client Client { get; }

		public ClientAccessor( Client client ) {
			Client = client;
		}
	}
}
