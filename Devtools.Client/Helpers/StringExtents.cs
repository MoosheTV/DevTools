using System.Text;

namespace Devtools.Client.Helpers
{
	public static class StringExtents
	{
		public static string ToTitleCase( this string s ) {
			if( s == null ) return null;

			var words = s.AddSpacesToCamelCase().Split( ' ' );
			for( var i = 0; i < words.Length; i++ ) {
				if( words[i].Length == 0 ) continue;

				var firstChar = char.ToUpper( words[i][0] );
				var rest = "";
				if( words[i].Length > 1 ) {
					rest = words[i].Substring( 1 ).ToLower();
				}
				words[i] = firstChar + rest;
			}
			return string.Join( " ", words );
		}

		public static string AddSpacesToCamelCase( this string s ) {
			var chars = s.ToCharArray();
			var sb = new StringBuilder();
			foreach( var c in chars ) {
				if( char.IsUpper( c ) ) {
					sb.Append( $" {c}" );
				}
				else {
					sb.Append( c );
				}
			}
			return sb.ToString().Trim();
		}
	}
}
