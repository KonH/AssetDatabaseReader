public class GetDependenciesProfiler : BaseProfiler {
	protected override string[] GetDependencies(string assetPath) {
		return TestCases.GetDependencies(assetPath);
	}
}
