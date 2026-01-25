using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class TerrainManager : MonoBehaviour {
    public static TerrainManager instance;


    [Header("Chunk Grid Settings")]
    public GameObject prefab;
    public int chunkWidth = 5;
    public int chunkHeight = 5;
    public Vector3 origin = new(0, 0);
    public float chunkSize = 10;
    public bool regenerateChunks = false;

    [Header("Terrain Settings")]
    public float heightScale = 20;
    public Texture2D groundTexture;
    public RenderTexture elevationTexture;
    public Shader groundShader;
    private Texture2D elevationTextureCPU;
    public bool regenerateTerrain = false;

    public static float GetHeightAt(Transform transform) {
        if (instance == null) return 0;
        return instance._GetHeightAt(transform);
    }


    // this function will need to be updated to fit the map size
    private float _GetHeightAt(Transform transform) {
        if (elevationTextureCPU == null 
            || elevationTextureCPU.width != elevationTexture.width 
            || elevationTextureCPU.height != elevationTextureCPU.height) {
            elevationTextureCPU = new(elevationTexture.width, elevationTexture.height);
        }

        Vector2 xz = new (transform.position.x - 200f / 500f, transform.position.z - 200f / 500f);
        Vector2 corner0 = new(-200, -200);
        Vector2 corner1 = new(200, 200);
        Vector2 uv = (xz - corner0) / (corner1 - corner0);

        Color color = elevationTextureCPU.GetPixelBilinear(uv.x, uv.y);
        float elevation = color.r + color.g / 256f;
        return heightScale * elevation;
    }

    void Update() {
        instance = this;

        if (regenerateChunks) {
            ClearChildren();
            SpawnGrid();
            regenerateChunks = false;
        }

        if (regenerateTerrain) {
            elevationTextureCPU = new(elevationTexture.width, elevationTexture.height);

            RenderTexture.active = elevationTexture;
            elevationTextureCPU.ReadPixels(new Rect(0, 0, elevationTexture.width, elevationTexture.height), 0, 0);
            elevationTextureCPU.Apply();
            // update the ground shader values here
            regenerateTerrain = false;
        }
    }

    void SpawnGrid() {
        for (int x = 0; x < chunkWidth; x++) {
            for (int z = 0; z < chunkHeight; z++) {
                Vector3 spawnPos = origin + new Vector3(x * chunkSize, 0, z * chunkSize);
                GameObject obj = PrefabUtility.InstantiatePrefab(prefab, transform) as GameObject;
                obj.transform.localPosition = spawnPos;
            }
        }
    }

    void ClearChildren() {
        for (int i = transform.childCount - 1; i >= 0; i--) {
            #if UNITY_EDITOR
            DestroyImmediate(transform.GetChild(i).gameObject);
            #else
            Destroy(transform.GetChild(i).gameObject);
            #endif
        }
    }
}
