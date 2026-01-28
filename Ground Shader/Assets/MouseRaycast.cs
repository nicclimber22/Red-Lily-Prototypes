using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class MouseRaycast : MonoBehaviour {
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float raycastDist = 32f;
    public int divsPerFirstPass = 64;
    public int divsPerPass = 16;
    public int numPasses = 3;

    public GameObject mouseSphere;

    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        Camera cam;
        Vector3 pos;
        Vector3 dir;
        #if UNITY_EDITOR
            cam = SceneView.lastActiveSceneView.camera;
            pos = cam.transform.position;
            dir = cam.transform.forward;
            // HandleUtility.GUIPointToWorldRay()
        #else
            cam = Camera.main;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            pos = ray.origin;
            dir = ray.dir.normalized;
        #endif

        float left = 0f;
        float right = raycastDist;

        mouseSphere.transform.position = pos + dir * right;


        if (pos.y < Map.GetHeightAt(pos)) return; // check if camera is below ground and dont raycast
        
        // Multi-scale ray march towards the ground. 
        // Stops when hits the ground and backtracks to refine estimate
        for (int j = 0; j < numPasses; j++) {
            float divisor = j == 0 ? divsPerFirstPass : divsPerPass;
            for (int i = 0; i < divisor; i++) {
                float dist = Mathf.Lerp(left, right, (i+1) / divisor);
                Vector3 point = pos + dist * dir;
                if (point.y < Map.GetHeightAt(point)) {
                    left = Mathf.Lerp(left, right, i / divisor);
                    right = dist;
                    break;
                }
            }
        }


        // // linear interpolate for more accuracy


        // move the mouse object
        if (right != raycastDist) {
            float dist = (left + right) / 2;
            Vector3 hitPos = pos + right * dir;
            mouseSphere.transform.position = hitPos;
            Debug.Log($"Raycast hit {hitPos}");
        }


    }   
}
