using UnityEngine;

[ExecuteAlways]
public class UICamPos : MonoBehaviour {
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform cameraTransform;
    public Material groundMaterial;

    // Update is called once per frame
    void Update() {
        groundMaterial.SetVector("_Camera_Pos", cameraTransform.position);
    }
}
