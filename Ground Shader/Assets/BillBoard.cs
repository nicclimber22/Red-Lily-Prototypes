using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class BillBoard : MonoBehaviour {
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public float verticalOffset = 1f;

    void Start() { }

    // Update is called once per frame
    void Update() {
        float elevation = TerrainManager.GetHeightAt(transform) + verticalOffset;
        transform.position = new(transform.position.x, elevation, transform.position.z);

        Camera sceneCam = SceneView.lastActiveSceneView.camera;

        Vector3 toPlayer = transform.position - sceneCam.transform.position;
        float theta = Mathf.Atan2(toPlayer.x, toPlayer.z) * Mathf.Rad2Deg;
        transform.eulerAngles = new(0, theta, 0);
    }
}
