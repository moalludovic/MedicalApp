using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadHDRI : MonoBehaviour {
    public Material mat;

    public void LoadTexture(string theTexture, float rotation)
    {
        Cubemap tex = Resources.Load("HDRI/" + theTexture) as Cubemap;
        mat.SetTexture("_Tex", tex);
        RenderSettings.skybox = mat;
        RenderSettings.skybox.SetFloat("_Rotation", rotation);
    }
}