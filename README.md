# Unity Engine Asset Database Reader

Unity editor extension class to navigate through asset dependencies without using standard methods.
When your use case is relied on assetPath only, it can helps with elimination of asset management overhead.

## Disclaimer

Use it when you really understand profits and limitations.

It's based on internals of Unity object structure (YAML and meta files format), which can be changed in further engine version. Compatibility isn't guaranteed.

## Usage example

You can get asset dependencies using AssetDatabase.GetDependencies:
```
using UnityEditor;
...
var paths = AssetDatabase.GetDependencies(path);
```

Or EditorUtility.CollectDependencies:
```
using UnityEditor;
...
var asset = AssetDatabase.LoadAssetAtPath<Object>(path);
var objects = EditorUtility.CollectDependencies(new [] { asset });
var paths = dependencies.Select(obj => AssetDatabase.GetAssetPath(obj));
```

As alternative, you can use AssetDatabaseReader.CollectDependencies:
```
using AssetDatabaseExtensions;
...
var settings = new Settings(new [] { ".asset", ".unity", ".prefab", ".mat" });
var reader = new AssetDatabaseReader(settings);
var paths = reader.CollectDependencies(path);
```

It return almost the same results (see below), but without possible additional conversions (path => UnityEngine.Object => UnityEngine.Object[] => path).

## Advantages

- **Results caching** - once value is calculated in AssetDatabaseReader instance scope, it will be saved for further usage
- **No duplication** - each path in response is unique, unlike of AssetDatabase/EditorUtility (it can return several different objects from the path)
- **Don't include origin asset path** - origin asset path isn't present in response (AssetDatabase/EditorUtility return it in many cases)
- **Don't include default assets** - any Unity default assets is skipped (AssetDatabase/EditorUtility return it)

## Limitations

- **Not fully compatible with AssetDatabase/EditorUtility** - please review *Advantages* section
- **For static asset structure** - asset database changes isn't tracked, so be careful with using same AssetDatabaseReader for long time, prefer to create new instances for short operations or ensure database consistancy
- **Only for project-related Assets** - don't works with and don't follow any references outside Assets directory
- **Bad performance** - at first glance, performance is very poor compared to standard methods

## Profiling

|Method|Memory|Time|
|---|---|---|
|GetDependencies|107.3 kb|90.11 ms|
|CollectDependencies|107.3 kb|93.81 ms|
|AssetDatabaseReader|14.9 mb|673.28 ms|
 
  
  
