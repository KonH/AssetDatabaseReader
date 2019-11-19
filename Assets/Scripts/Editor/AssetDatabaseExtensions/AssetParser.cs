using System.Text.RegularExpressions;
using UnityEngine;

namespace AssetDatabaseExtensions {
	sealed class AssetParser {
		static readonly Regex GuidRegex = new Regex("guid: ([a-z0-9]*)");

		readonly Settings _settings;

		public AssetParser(Settings settings) {
			_settings = settings;
		}

		public string[] GetAllGuidsFromAssetBody(string body) {
			if ( _settings.Debug ) {
				Debug.Log("=> GetAllGuidsFromAssetBody:\n" + body);
			}
			var matches = GuidRegex.Matches(body);
			var result = new string[matches.Count];
			for ( var i = 0; i < matches.Count; i++ ) {
				result[i] = matches[i].Groups[1].Value;
			}
			if ( _settings.Debug ) {
				Debug.LogFormat("<= GetAllGuidsFromAssetBody ('{0}'):\n{1}", result.Length, string.Join("\n", result));
			}
			return result;
		}
	}
}
