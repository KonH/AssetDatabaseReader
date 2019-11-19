using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AssetDatabaseExtensions {
	sealed class GuidMapper {
		readonly Settings _settings;

		readonly Dictionary<string, string> _guidToAssetPaths = new Dictionary<string, string>();

		public GuidMapper(Settings settings) {
			_settings = settings;
		}

		public string GetAssetPath(string guid) {
			string result;
			if ( !_guidToAssetPaths.TryGetValue(guid, out result) ) {
				result = AssetDatabase.GUIDToAssetPath(guid);
				if ( !_settings.IsValidPath(result) ) {
					if ( _settings.Debug ) {
						Debug.LogFormat("GetAssetPath: '{0}' => '{1}', but path isn't valid", guid, result);
					}
					result = string.Empty;
				}
				if ( _settings.Debug ) {
					Debug.LogFormat("GetAssetPath: '{0}' => '{1}'", guid, result);
				}
				_guidToAssetPaths[guid] = result;
			}
			return result;
		}
	}
}