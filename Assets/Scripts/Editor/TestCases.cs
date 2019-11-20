using System.Linq;
using UnityEngine;
using UnityEditor;
using AssetDatabaseExtensions;

public static class TestCases {
	static Settings Settings = new Settings(new [] { ".asset", ".unity", ".prefab", ".mat" }, true);

	public static AssetDatabaseReader CreateReader() {
		return new AssetDatabaseReader(Settings);
	}

	public static string[] GetDependencies(string path) {
		var dependencies = AssetDatabase.GetDependencies(path);
		return dependencies
			.Where(p => p != path) // Limitation: don't return original path as dependency
			.Where(Settings.IsValidPath) // Limitation: don't return any default resources
			.Distinct() // Limitation: don't return duplicate paths
			.ToArray();
	}

	public static string[] CollectDependencies(string path) {
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

	public static string[] CustomCollectDependencies(AssetDatabaseReader reader, string path) {
		return reader.CollectDependencies(path);
	}
}