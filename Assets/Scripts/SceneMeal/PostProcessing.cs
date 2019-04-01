using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class PostProcessing : MonoBehaviour {
    public Material postProcessingMaterial;
    public float minScramble;
    public float maxScramble;
    public float speed;

    public float whiteness = 0;
    bool ScrambleUp = true;
    bool dumpingSyndrome = false;

    private void Start()
    {
        SetWhiteness(whiteness);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, postProcessingMaterial);
    }

    private void Update()
    {
        if (dumpingSyndrome)
        {
            if (whiteness > maxScramble)
            {
                ScrambleUp = false;
            }
            else if (whiteness < minScramble)
            {
                ScrambleUp = true;
            }

            if (ScrambleUp)
            {
                whiteness += speed * Time.deltaTime;
            }
            else
            {
                whiteness -= speed * Time.deltaTime;
            }
            SetWhiteness(whiteness);
        }
    }

    public void SetWhiteness(float whiteness)
    {
        postProcessingMaterial.SetFloat("_Whiteness", whiteness);
    }

    public void SetDumpingSyndrome(bool isActive)
    {
        dumpingSyndrome = isActive;
        if (!isActive)
        {
            whiteness = 0;
            SetWhiteness(0);
        }
    }
}
