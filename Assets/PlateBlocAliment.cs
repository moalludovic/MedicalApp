using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateBlocAliment : MonoBehaviour {
    public Aliment al;
    public int nbSlices;
    private LoadAliment loader;

    private const int maxNbAlimentInLine = 5;
    public float xStep;
    public float zStep;
       
	// Use this for initialization
	void Start () {
        //al = new Aliment();
        //al.type = "Test";
        //al.name = "pain";
        //al.portions = 4;
        //al.slices = 5;
        //al.sugary = true;
        //al.greasy = false;
        //al.cold = false;
        //al.hot = false;
        //SetAlimentQuantity();
        //SetAlimentsPositions();
        loader = MedicalAppManager.Instance().gameObject.GetComponent<LoadAliment>();
    }
	
	// Update is called once per frame
	void Update () {
        //SetAlimentQuantity();
        //SetAlimentsPositions();
    }

    private void OnMouseEnter()
    {
        transform.localScale *= 1.2f;
        updateAliments();
    }
    private void OnMouseExit()
    {
        transform.localScale /= 1.2f;
        updateAliments();
    }

    private void OnMouseUp()
    {
        if (!MedicalAppManager.Instance().OnMeal)
        {
            //remettre l'ancien
            if (GameObject.Find("SliderModif") && MedicalAppManager.Instance().selectedAliment != null)
                GameObject.Find("SliderModif").GetComponent<SliderModif>().CloseAndKeepOldValues();
            // ajout aliment selectionne dans le manager
            MedicalAppManager.Instance().selectedAliment = gameObject;
            // zone de modification
            AtelierManager.Instance().showModification(true);
        }
        else
        {
            if (MealManager.Instance().MangerPortion != null)
            {
                // affichage manger portion
                MedicalAppManager.Instance().selectedAliment = gameObject;
                MealManager.Instance().updateInfos();
                MealManager.Instance().MangerPortion.SetActive(true);
            }
        }
    }

    public void SetAlimentQuantity()
    {
        int nbAliments = Mathf.CeilToInt(nbSlices / (float)al.slices);
        for (int i = transform.childCount; i > nbAliments; --i)//je détruit ceux en trop
        {
            GameObject temp = transform.GetChild(i - 1).gameObject;
            temp.transform.parent = null;
            Destroy(temp);
            //Destroy(transform.GetChild(i-1).gameObject);
        }

        while(transform.childCount< nbAliments){//j'en ajoute tant qu'il n'y en a pas assez
            if (loader == null)
            {
                loader = MedicalAppManager.Instance().gameObject.GetComponent<LoadAliment>();
            }
            loader.LoadAndSetParent(al, transform);
        }

        for (int i = 0;i<transform.childCount;++i){//tous sauf le dernier
            if(i!=transform.childCount-1 || nbSlices % al.slices == 0)
            {
                transform.GetChild(i).GetComponent<BlocAliment>().nbSlices = al.slices;//al est le même aliment que celui dans les enfants
            }
            else{//dernier
                transform.GetChild(i).GetComponent<BlocAliment>().nbSlices = nbSlices % al.slices;
            }
        }
    }

    public void SetAlimentsPositions()
    {
        int nbAl = transform.childCount;
        int nbAlimentInLine = Mathf.Min(nbAl, maxNbAlimentInLine);

        for (int i = 0;i<nbAl;++i){
            //taille
            Mesh mesh;
            if (transform.GetChild(i).GetComponent<BlocAliment>().aliment.multiMesh)
            {
                mesh = transform.GetChild(i).GetComponent<BlocAliment>().Meshs[transform.GetChild(i).GetComponent<BlocAliment>().nbSlices-1];
            }
            else
            {
                mesh = transform.GetChild(i).GetComponent<MeshFilter>().mesh;
            }
            float scale = (GetComponent<MeshFilter>().mesh.bounds.size.x-MedicalAppManager.Instance().offset)/ Mathf.Max((nbAlimentInLine * (mesh.bounds.size.x*(1+xStep))- mesh.bounds.size.x*xStep), Mathf.Ceil(nbAl/(float)maxNbAlimentInLine) * (mesh.bounds.size.z*(1+zStep)) - mesh.bounds.size.z*zStep, mesh.bounds.size.y);
            transform.GetChild(i).transform.localScale = scale * Vector3.one;

            //position
            float x = ((i % nbAlimentInLine) - (nbAlimentInLine - 1) / 2.0f ) * (mesh.bounds.size.x*scale * (1 + xStep)) ;
            float z = (i/maxNbAlimentInLine - (Mathf.Ceil(nbAl / (float)maxNbAlimentInLine)-1) / 2.0f) * (mesh.bounds.size.z * scale * (1 + zStep));

            transform.GetChild(i).transform.localPosition = new Vector3(x, 0, z);
            if (MedicalAppManager.Instance().OnMeal)
            {
                transform.GetChild(i).transform.localPosition += new Vector3(0.25f, 0, 0);
            }

            //maj
            transform.GetChild(i).GetComponent<BlocAliment>().SetAspectWithSlice();
        }
    }

    public void updateAliments(){
        int nbAl = transform.childCount;

        for (int i = 0; i < nbAl; ++i)
        {
            //maj
            transform.GetChild(i).GetComponent<BlocAliment>().SetAspectWithSlice();
        }
    }
}
