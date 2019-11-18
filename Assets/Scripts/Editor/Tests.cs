using System.Linq;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class Tests {
	static string[] DefaultSolution(string path) {
		var asset = AssetDatabase.LoadAssetAtPath<Object>(path);
		var roots = new [] { asset };
		var dependencies = EditorUtility.CollectDependencies(roots);
		return dependencies.Select(obj => AssetDatabase.GetAssetPath(obj)).ToArray();
	}

	static string[] CustomSolution(string path) {
		return new AssetDatabaseReader().CollectDependencies(path);
	}

	static string[] GetAssetPaths() {
		return AssetDatabase.GetAllAssetPaths();
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
