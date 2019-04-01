using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlocAliment : MonoBehaviour {

    public Aliment aliment;

    public int nbSlices;//unitée consommable (diminue avec la consommation) (on fait nbPortion/nbSlice pour récupérer l'équivalent d'une Slice en Portions)
    
    public bool OnPlate = false;
    public bool OnMeal = false;
    public bool OnAtelier = false;

    //partie découpe
    public GameObject plane;
    public Vector3 normal;

    //partie multiMesh
    public List<Mesh> Meshs = new List<Mesh>();
    public List<Material> Materials = new List<Material>();

    private void OnMouseEnter()
    {
        if (OnAtelier || OnMeal)
        {
            if (OnAtelier)
            {
                AtelierManager.Instance().setCursor("closed");
            }

            transform.localScale *= 1.2f;
            if (plane != null)
            {
                SetAspectWithSlice();
            }
        }
    }
    private void OnMouseExit()
    {
        if (OnAtelier || OnMeal)
        {
            transform.localScale /= 1.2f;
            if (OnAtelier)
            {
                AtelierManager.Instance().setCursor("open");
            }
            if (plane != null)
            {
                SetAspectWithSlice();
            }
        }
    }
    private void OnMouseUp()
    {
        // ajout aliment selectionne dans le manager
        MedicalAppManager.Instance().selectedAliment = gameObject;
        if (OnPlate)
        {
            // zone de modification
            AtelierManager.Instance().showModification(true);
        }
        else if(OnMeal)
        {
            if (MealManager.Instance().MangerPortion != null)
            {
                // affichage manger portion
                MealManager.Instance().MangerPortion.SetActive(true);
            }
        }
        else if(OnAtelier)
        {
            // zone de validation
            AtelierManager.Instance().showValidation(true);
            //Camera.main.GetComponent<MouseLook>().setCameraFree(false);
        }
    }
    public void SetVisibleSliceWithSlider(){//pour test
        SetVisibleSliceWithFloat(GameObject.Find("SliderAjout").GetComponent<Slider>().value);
    }

    public void SetDefaultSize(){
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        if (mesh != null && Mathf.Max(mesh.bounds.size.x, mesh.bounds.size.y, mesh.bounds.size.z)!=0)
        {
            transform.localScale = MedicalAppManager.Instance().alimentSize / Mathf.Max(mesh.bounds.size.x, mesh.bounds.size.y, mesh.bounds.size.z) * Vector3.one;
        }
    }

    public void SetAspectWithSlice(){
        if (aliment.multiMesh)
        {
            if (nbSlices > 0) {
                Destroy(GetComponent<BoxCollider>());
                Mesh mesh = Meshs[nbSlices - 1];
                GetComponent<MeshFilter>().mesh = Meshs[nbSlices - 1];
                GetComponent<Renderer>().material = Materials[nbSlices - 1];
                gameObject.AddComponent<BoxCollider>();//on le remet pour qu'il soit adapté à la taille du nouveau mesh
            }
            else{
                //Destroy(GetComponent<MeshFilter>().mesh);
                GetComponent<MeshFilter>().mesh=null;
                Destroy(GetComponent<BoxCollider>());
            }
        }
        else
        {
            SetVisibleSliceWithFloat(nbSlices / (float)aliment.slices);
        }
    }

    public void SetVisibleSliceWithFloat(float outOfOne){
        if (outOfOne != 0)
        {
            GetComponent<MeshRenderer>().enabled = true;
            Vector3 rotatedNormal = transform.rotation * normal;
            GetComponent<Renderer>().material.SetVector("_PlaneNormal", rotatedNormal);

            if (normal.x != 0)
            {
                //plane.transform.position = new Vector3(GetComponent<MeshFilter>().mesh.bounds.size.x * transform.localScale.x * (-1f / 2 + outOfOne), 0, 0) + transform.position;
                plane.transform.localPosition = new Vector3(GetComponent<MeshFilter>().mesh.bounds.size.x * (-1f / 2 + outOfOne), 0, 0);
            }
            else if (normal.y != 0)
            {
                plane.transform.localPosition = new Vector3(0, GetComponent<MeshFilter>().mesh.bounds.size.y * (-1f / 2 + outOfOne), 0);
            }
            else
            {
                plane.transform.localPosition = new Vector3(0, 0, GetComponent<MeshFilter>().mesh.bounds.size.z * (-1f / 2 + outOfOne));
            }
            GetComponent<Renderer>().material.SetVector("_PlanePosition", plane.transform.position);
        }
        else{
            GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
