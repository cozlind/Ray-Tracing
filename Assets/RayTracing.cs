using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RayTracing : MonoBehaviour {

    public ComputeShader cs;
    private RenderTexture texture;
    private int groupsX;
    private int groupsY;

    private void Start () {
        texture = new RenderTexture (Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
        texture.filterMode = FilterMode.Point;
        texture.enableRandomWrite = true;
        texture.Create ();

        uint threadsX, threadsY, threadsZ;
        cs.GetKernelThreadGroupSizes (0, out threadsX, out threadsY, out threadsZ);
        groupsX = (int)((float)texture.width / threadsX);
        groupsY = (int)((float)texture.height / threadsY);

        cs.SetTexture (0, "result", texture);
    }

    private void Update () {
        cs.SetFloat ("time", Time.time);
        cs.Dispatch (0, groupsX, groupsY, 1);
    }

    private void OnGUI () {
        GUI.DrawTexture (new Rect (0, 0, texture.width, texture.height), texture, ScaleMode.ScaleToFit);
    }
}
