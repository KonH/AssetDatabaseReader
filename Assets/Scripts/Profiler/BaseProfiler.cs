using UnityEngine;
using UnityEngine.Profiling;
using UnityEditor;
using System.Linq;

public abstract class BaseProfiler : MonoBehaviour {
	void Start() {
		Invoke("Test", 0.5f);

	}

	void Test() {
		var assetPaths = AssetDatabase.GetAllAssetPaths()
			.Where(p => p.StartsWith("Assets"))
			.ToArray();
		Profiler.BeginSample("CollectAllDependencies");
		var acc = 0;
		foreach ( var assetPath in assetPaths ) {
			var result = GetDependencies(assetPath);
			acc += result.Length;
		}
		Profiler.EndSample();
		Debug.Log(acc);
	}

	protected abstract string[] GetDependencies(string assetPath);
}
