using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace AssetDatabaseExtensions {
	public sealed class AssetDatabaseReader {
		readonly Settings         _settings;
		readonly DependencyFinder _finder;

		readonly Dictionary<string, string[]> _dependencies   = new Dictionary<string, string[]>();
		readonly HashSet<string>              _circularChecks = new HashSet<string>();

		public AssetDatabaseReader(Settings settings) {
			_settings = settings;
			_finder   = new DependencyFinder(_settings);
		}

		public string[] CollectDependencies(string assetPath) {
			_circularChecks.Clear();
			return CollectDependenciesWithCircularCheck(assetPath).Dependencies;
		}

		CollectResult CollectDependenciesWithCircularCheck(string assetPath) {
			string[] result;
			if ( !_dependencies.TryGetValue(assetPath, out result) ) {
				var isCircularDependency = _circularChecks.Contains(assetPath);
				if ( isCircularDependency ) {
					if ( _settings.Debug ) {
						Debug.LogFormat("CollectDependenciesWithCircularCheck: found circular dependency: '{0}', skip it", assetPath);
					}
					return CollectResult.CircularDependency();
				}
				_circularChecks.Add(assetPath);
				result = CollectDependenciesSlowPath(assetPath);
				_dependencies[assetPath] = result;
			}
			return CollectResult.NormalDependency(result);
		}

		string[] CollectDependenciesSlowPath(string assetPath) {
			if ( _settings.Debug ) {
				Debug.LogFormat("=> CollectDependenciesSlowPath: '{0}'", assetPath);
			}
			var directDependencies = _finder.LookupDirectDependencies(assetPath);
			var allDependencies = new List<string>();
			foreach ( var dependencyPath in directDependencies ) {
				var childDependencies = CollectDependenciesWithCircularCheck(dependencyPath);
				if ( !childDependencies.IsCircularDependency ) {
					allDependencies.Add(dependencyPath);
					allDependencies.AddRange(childDependencies.Dependencies);
				}
			}
			var result = allDependencies.Distinct().ToArray();
			if ( _settings.Debug ) {
				Debug.LogFormat(
					"<= CollectDependenciesSlowPath: '{0}' ({1}):\n{2}",
					assetPath,
					result.Length,
					string.Join("\n", result));
			}
			return result;
		}
	}
}