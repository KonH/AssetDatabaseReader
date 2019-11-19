using System.Collections.Generic;

namespace AssetDatabaseExtensions {
	public sealed class Settings {
		public readonly HashSet<string> Extensions;
		public readonly bool            Debug;

		public Settings(IEnumerable<string> extensions, bool debug = false) {
			Extensions = new HashSet<string>(extensions);
			Debug      = debug;
		}

		public bool IsValidPath(string path) {
			return path.StartsWith("Assets");
		}
	}
}
