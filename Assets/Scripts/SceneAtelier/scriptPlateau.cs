using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scriptPlateau : MonoBehaviour {

    public List<GameObject> aliments;
    public bool modification = false;
    public bool stopModification = false;
    public GameObject Fond;
    public GameObject closeButton;

    // Use this for initialization
    void Start () {
        aliments = new List<GameObject>();
    }

    public void SetAlimentsPositions()
    {
        int nbAl = aliments.Count;
        int currentAl = 0;
        MedicalAppManager at = MedicalAppManager.Instance();
        for (int y = 0; y < at.yMaxPlateau; y++)
        {
            for (int x = 0; x < at.xMaxPlateau; x++)
            {
                Vector3 pos = new Vector3((-at.xStepPlateau * (at.xMaxPlateau - 1)) / 2.0f + at.xStepPlateau * x, at.yStepPlateau * (at.yMaxPlateau - 1) / 2.0f - at.yStepPlateau * y, -0.025f);
                if (currentAl < nbAl)
                {
                    aliments[currentAl].transform.localPosition = pos;

                    foreach(Transform child in aliments[currentAl].transform){
                       child.gameObject.GetComponent<BlocAliment>().SetAspectWithSlice();
                    }
                }
                currentAl++;
            }
        }
    }

    // fenetre modif quantité
    public void Modification(bool isActive)
    {
        AtelierManager.Instance().transform.GetChild(0).gameObject.SetActive(!isActive);
        AtelierManager.Instance().activateTypeButtons(!isActive);
        AtelierManager.Instance().showMealValidation(isActive);
        closeButton.SetActive(isActive);
        // scale plateau
        if (isActive)
        {
            transform.localScale *= 3.0f;
            transform.position += new Vector3(0.0f, 0.5f, 0.0f);
            AtelierManager.Instance().showValidation(false);
        }
        else
        {
            transform.localScale /= 3.0f;
            transform.position -= new Vector3(0.0f, 0.5f, 0.0f);
            if (GameObject.Find("SliderModif"))
                GameObject.Find("SliderModif").GetComponent<SliderModif>().CloseAndKeepOldValues();
            AtelierManager.Instance().showModification(false);
        }
        modification = isActive;
        GetComponent<BoxCollider>().enabled = !isActive;
        CanInteractWithAliments(isActive);
        for (int al = 0; al < aliments.Count; al++)
        {
            foreach (Transform child in aliments[al].transform)
            {
                child.gameObject.GetComponent<BlocAliment>().SetAspectWithSlice();
            }
        }
    }

    public void CanInteractWithAliments(bool value){
        for (int al = 0; al < aliments.Count; al++)
        {
            aliments[al].GetComponent<BoxCollider>().enabled = value;
        }
    }

    public void Close()
    {
        //Camera.main.GetComponent<MouseLook>().setCameraFree(true);
        AtelierManager.Instance().Consigne.text = AtelierManager.Instance().Consignes[1];
        Modification(false);
    }

    public void Clear(){
        for (int i = transform.childCount; i > 0; i--)
        {
            if (transform.GetChild(i - 1).name != "Fond")
            {
                Destroy(transform.GetChild(i - 1).gameObject);
            }
        }
        aliments.Clear();
    }

    public void OnMouseDown()
    {
        if (!MedicalAppManager.Instance().OnMeal)
        {
            AtelierManager.Instance().Consigne.text = AtelierManager.Instance().Consignes[2];
            Modification(true);
        }
    }
}
