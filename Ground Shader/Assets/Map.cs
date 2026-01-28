using UnityEngine;

[ExecuteAlways]
public class Map : MonoBehaviour {
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Terrain")]
    public Vector3 mapSize;
    public GameObject topoMap;
    public Camera topoCam;
    public GameObject walls;
    public Material groundMat;

    public Texture2D groundTexture;
    public Texture2D topoTexture;
    private Texture2D topoTextureCPU;


    public bool updateTerrain = false;


    [Header("Hex UI")]
    public GameObject hexGrid;
    public Camera hexCam;
    public float hexCamSize;
    public RenderTexture hexCamTexture;
    public bool updateHex;

    public static Map instance;



    public static float GetHeightAt(Transform transform) {
        if (instance == null) return 0;
        return instance._GetHeightAt(transform.position);
    }

    public static float GetHeightAt(Vector3 position) {
        if (instance == null) return 0;
        return instance._GetHeightAt(position);
    }

    // this function will need to be updated to fit the map size
    private float _GetHeightAt(Vector3 position) {
        Vector2 uv = new(position.x / mapSize.x, position.z / mapSize.z);
        Color color = topoTexture.GetPixelBilinear(uv.x, uv.y);
        float elevation = Mathf.Pow(color.r, 2.2f) * mapSize.y; // need to do gamma correction here ???
        return elevation;
    }

    
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        instance = this;

        if (updateTerrain) {
            updateTerrain = false;
            
            topoMap.transform.position          = new(mapSize.x / 2, mapSize.y + 10, mapSize.z / 2);
            topoCam.transform.position          = new(mapSize.x / 2, mapSize.y + 11, mapSize.z / 2);
            hexGrid.transform.position          = new(hexGrid.transform.position.x, mapSize.y + 12, hexGrid.transform.position.z);
            hexCam.transform.position           = new(hexCam.transform.position.x, mapSize.y + 13, hexCam.transform.position.z);

            topoMap.transform.localScale        = new(mapSize.x, mapSize.z, 1);
            topoCam.orthographicSize            = mapSize.z / 2;
            topoCam.aspect                      = mapSize.x / mapSize.z;
            walls.transform.localScale          = new(mapSize.x/10, mapSize.y, mapSize.z/10);

            Debug.Log("Updating Ground Material");
            groundMat.SetVector("_Map_Size", mapSize);
            groundMat.SetTexture("_Ground_Texture", groundTexture);
            groundMat.SetTexture("_Height_Texture", topoTexture);
        }

        if (updateHex) {
            updateHex = false;
            hexCam.orthographicSize = hexCamSize;
            groundMat.SetFloat("_Hexcam_Size", hexCamSize);
            groundMat.SetVector("_Hexcam_Pos", hexCam.transform.position);
        }
    }
}
