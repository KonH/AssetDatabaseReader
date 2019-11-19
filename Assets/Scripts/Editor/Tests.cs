using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using AssetDatabaseExtensions;

public sealed class Tests {
	static Settings Settings = new Settings(new [] { ".asset", ".unity", ".prefab", ".mat" }, true);

	static string[] DefaultSolution(string path) {
		var asset = AssetDatabase.LoadAssetAtPath<Object>(path);
		var roots = new [] { asset };
		var dependencies = EditorUtility.CollectDependencies(roots);
		return dependencies
			.Where(a => a != asset) // Limitation: don't return original asset as dependency
			.Select(obj => AssetDatabase.GetAssetPath(obj))
			.Where(p => p != path) // Limitation: don't return original path as dependency
			.Where(Settings.IsValidPath) // Limitation: don't return any default resources
			.Distinct() // Limitation: don't return duplicate paths
			.ToArray();
	}

	static string[] CustomSolution(string path) {
		return new AssetDatabaseReader(Settings).CollectDependencies(path);
	}

	static IEnumerable<string> GetAssetPaths() {
		return AssetDatabase.GetAllAssetPaths()
			.Where(p => p.StartsWith("Assets")); // Limitation: works only with project assets
	}

	[TestCaseSource("GetAssetPaths")]
	public static void IsCustomSolutionMakeSameResult(string path) {
		var defaultResult = DefaultSolution(path);
		var customResult = CustomSolution(path);
		Assert.NotNull(defaultResult);
		Assert.NotNull(customResult);
		CollectionAssert.AreEquivalent(defaultResult, customResult);
	}
}
