using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using NUnit.Framework;

public sealed class Tests {
	static IEnumerable<string> GetAssetPaths() {
		return AssetDatabase.GetAllAssetPaths()
			.Where(p => p.StartsWith("Assets")); // Limitation: works only with project assets
	}

	[TestCaseSource("GetAssetPaths")]
	public static void IsDefaultResultsEquals(string path) {
		var defaultResult1 = TestCases.GetDependencies(path);
		var defaultResult2 = TestCases.CollectDependencies(path);
		CollectionAssert.AreEquivalent(defaultResult1, defaultResult2);
	}

	[TestCaseSource("GetAssetPaths")]
	public static void IsCustomSolutionMakeSameResultCompareToGetDependencies(string path) {
		var defaultResult = TestCases.GetDependencies(path);
		var customResult = TestCases.CustomCollectDependencies(TestCases.CreateReader(), path);
		Assert.NotNull(defaultResult);
		Assert.NotNull(customResult);
		CollectionAssert.AreEquivalent(defaultResult, customResult);
	}

	[TestCaseSource("GetAssetPaths")]
	public static void IsCustomSolutionMakeSameResultCompareToCollectDependencies(string path) {
		var defaultResult = TestCases.CollectDependencies(path);
		var customResult = TestCases.CustomCollectDependencies(TestCases.CreateReader(), path);
		Assert.NotNull(defaultResult);
		Assert.NotNull(customResult);
		CollectionAssert.AreEquivalent(defaultResult, customResult);
	}
}
