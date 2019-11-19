using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace AssetDatabaseExtensions {
	sealed class DependencyFinder {
		static readonly string[] EmptyArray = new string[0];

		readonly Settings    _settings;
		readonly AssetParser _parser;
		readonly GuidMapper  _mapper;

		public DependencyFinder(Settings settings) {
			_settings = settings;
			_parser   = new AssetParser(_settings);
			_mapper   = new GuidMapper(_settings);
		}

		public string[] LookupDirectDependencies(string assetPath) {
			if ( _settings.Debug ) {
				Debug.LogFormat("=> LookupDirectDependencies: '{0}'", assetPath);
			}
			if ( string.IsNullOrEmpty(assetPath) || !File.Exists(assetPath) ) {
				if ( _settings.Debug ) {
					Debug.LogFormat("<= LookupDirectDependencies: '{0}' - skip, invalid file", assetPath);
				}
				return EmptyArray;
			}
			var metaFilePath = AssetDatabase.GetTextMetaFilePathFromAssetPath(assetPath);
			if ( !File.Exists(metaFilePath) ) {
				if ( _settings.Debug ) {
					Debug.LogFormat("<= LookupDirectDependencies: '{0}' - skip, no .meta file", assetPath);
				}
				return EmptyArray;
			}
			var extension = Path.GetExtension(assetPath);
			if ( !_settings.Extensions.Contains(extension) ) {
				Debug.LogFormat("<= LookupDirectDependencies: '{0}' - skip, unsupported extension", assetPath);
				return EmptyArray;
			}
			var assetBody = File.ReadAllText(assetPath);
			var guids = _parser.GetAllGuidsFromAssetBody(assetBody);
			var result = guids
				.Select(_mapper.GetAssetPath)
				.Where(p => !string.IsNullOrEmpty(p))
				.ToArray();
			if ( _settings.Debug ) {
				Debug.LogFormat("<= LookupDirectDependencies: '{0}' ({1}):\n{2}",
					assetPath,
					result.Length,
					string.Join("\n", result));
			}
			return result;
		}
	}
}