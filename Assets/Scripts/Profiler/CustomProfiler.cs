using AssetDatabaseExtensions;
using UnityEditor;

public class CustomProfiler : BaseProfiler {
	AssetDatabaseReader _reader = TestCases.CreateReader();

	protected override string[] GetDependencies(string assetPath) {
		return TestCases.CustomCollectDependencies(_reader, assetPath);
	}
}
