using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderModif : MonoBehaviour {

    //public List<BlocAliment> sameAliments;
    public Text ModificationText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void InitSlider()
    {
        // recuperation valeurs sliders lors de la selection d'un aliment
        float maxValue = MedicalAppManager.Instance().selectedAliment.GetComponent<PlateBlocAliment>().nbSlices;

        // definition des la valeurs + text en fonction du type d'aliment
        GetComponent<Slider>().maxValue = maxValue;
        GetComponent<Slider>().value = maxValue;
        if (MedicalAppManager.Instance().IsSelectedDrink())
        {
            ModificationText.text = MedicalAppManager.Instance().selectedAliment.GetComponent<PlateBlocAliment>().al.name + "\n" + GetComponent<Slider>().value + " gorgée(s)";
        }
        else
        {
            ModificationText.text = MedicalAppManager.Instance().selectedAliment.GetComponent<PlateBlocAliment>().al.name + "\n" + GetComponent<Slider>().value + " tranche(s)";
        }
    }

    public void UpdatePanel()
    {
        // mise a jour du text en fonction du type d'aliement
        if (MedicalAppManager.Instance().IsSelectedDrink())
        {
            ModificationText.text = MedicalAppManager.Instance().selectedAliment.GetComponent<PlateBlocAliment>().al.name + "\n" + GetComponent<Slider>().value + " gorgée(s)";
        }
        else
        {
            ModificationText.text = MedicalAppManager.Instance().selectedAliment.GetComponent<PlateBlocAliment>().al.name + "\n" + GetComponent<Slider>().value + " tranche(s)";
        }
        UpdateSlider();
    }

    public void UpdateSlider(){
        MedicalAppManager.Instance().selectedAliment.GetComponent<PlateBlocAliment>().nbSlices = (int)this.GetComponent<Slider>().value;
        MedicalAppManager.Instance().selectedAliment.GetComponent<PlateBlocAliment>().SetAlimentQuantity();
        MedicalAppManager.Instance().selectedAliment.GetComponent<PlateBlocAliment>().SetAlimentsPositions();
    }

    public void CloseAndKeepOldValues(){
        this.GetComponent<Slider>().value = this.GetComponent<Slider>().maxValue;
        UpdateSlider();
    }
}
