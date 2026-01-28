using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class MouseRaycast : MonoBehaviour {
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float raycastDist = 32f;
    public float raycastFirstPassSamples = 16f;
    public float raycastSecondPassSamples = 16f;

    public GameObject mouseSphere;

    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        Camera cam;
        #if UNITY_EDITOR
            cam = SceneView.lastActiveSceneView.camera;
        #else
            cam = Camera.main;
        #endif
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Vector3 rayPos = ray.origin;
        Vector3 rayDir = ray.direction.normalized;

        float left = 0f;
        float right = raycastDist;


        if (rayPos.y < Map.GetHeightAt(rayPos)) return; // check if camera is below ground and dont raycast
        
        // narrow search bounds
        for (int i = 0; i < raycastFirstPassSamples; i++) {
            float dist = Mathf.Lerp(left, right, (i+1) / raycastFirstPassSamples);
            Vector3 point = rayPos + dist * rayDir;
            if (point.y < Map.GetHeightAt(point)) {
                left += i * right / raycastFirstPassSamples;
                right = dist;
                left = right - (right - left) / raycastFirstPassSamples;
            }
        }

        // refine search
        for (int i = 0; i < raycastSecondPassSamples; i++) {
            float dist = Mathf.Lerp(left, right, (i+1) / raycastSecondPassSamples);
            Vector3 point = rayPos + dist * rayDir;
            if (point.y < Map.GetHeightAt(point)) {
                left += i * right / raycastSecondPassSamples;
                right = dist;
                left = right - (right - left) / raycastSecondPassSamples;
            }
        }


        // linear interpolate for more accuracy


        // move the mouse object
        if (right != raycastDist) {
            float dist = (left + right) / 2;
            Vector3 hitPos = rayPos + dist * rayDir;
            mouseSphere.transform.position = hitPos;
            Debug.Log(hitPos);
        }


    }   
}
