using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// InvertCamera script by Joachim Ante ported to C#

[ExecuteAlways]
[RequireComponent(typeof(Camera))]
public class UpsideDownCamera : MonoBehaviour
{
    private Camera cam;
    private void Awake()
    {
        cam = GetComponent<Camera>();
    }
    // Start is called before the first frame update
    private void OnPreCull()
    {
        cam.ResetWorldToCameraMatrix();
        cam.ResetProjectionMatrix();
        cam.projectionMatrix *= Matrix4x4.Scale(new Vector3(1, -1, 1));
    }

    private void OnPreRender()
    {
        GL.invertCulling = true;
    }

    private void OnPostRender()
    {
        GL.invertCulling = false;
    }
}
