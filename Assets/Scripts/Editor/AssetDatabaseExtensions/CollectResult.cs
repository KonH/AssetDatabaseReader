namespace AssetDatabaseExtensions {
	struct CollectResult {
		public readonly string[] Dependencies;
		public readonly bool     IsCircularDependency;

		CollectResult(string[] dependencies, bool isCircularDependency) {
			Dependencies         = dependencies;
			IsCircularDependency = isCircularDependency;
		}

		public static CollectResult NormalDependency(string[] dependencies) {
			return new CollectResult(dependencies, false);
		}

		public static CollectResult CircularDependency() {
			return new CollectResult(null, true);
		}
	}
}