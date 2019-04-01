using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoutonsMangerPortion : MonoBehaviour {
    
    public float AnimationTime;

    public void CloseValidation()
    {
        MealManager.Instance().MangerPortion.SetActive(false);
        MedicalAppManager.Instance().selectedAliment = null;
        MealManager.Instance().updateInfos();
    }
    public void EatPortion()
    {
        if (MealManager.Instance().totalPortions <= 30)
        {
            ////gérer animation
            MealManager.Instance().SetMouthAnimation(AnimationTime);

            ////enlever une portion à la selection courante
            MedicalAppManager.Instance().selectedAliment.GetComponent<PlateBlocAliment>().nbSlices -= 1;
            MedicalAppManager.Instance().selectedAliment.GetComponent<PlateBlocAliment>().SetAlimentQuantity();
            MedicalAppManager.Instance().selectedAliment.GetComponent<PlateBlocAliment>().SetAlimentsPositions();
            float temp = MedicalAppManager.Instance().selectedAliment.GetComponent<PlateBlocAliment>().al.portions / (float)MedicalAppManager.Instance().selectedAliment.GetComponent<PlateBlocAliment>().al.slices;
            
            MealManager.Instance().totalPortions += temp;
            MedicalAppManager.Instance().selectedAliment.GetComponent<PlateBlocAliment>().updateAliments();

            MealManager.Instance().AddEatenAliment();

            if((MedicalAppManager.Instance().selectedAliment.GetComponent<PlateBlocAliment>().al.greasy
            ||MedicalAppManager.Instance().selectedAliment.GetComponent<PlateBlocAliment>().al.sugary) 
            && MedicalAppManager.Instance().userOperation == Operation.by_pass){
                MealManager.Instance().SetDumpingSyndrom(true);
                MealManager.Instance().EndMeal();
            }


            ////mettre à jour meal
            //if (!_MGR_MedicalApp.instance.repas.listAliments.Exists(u => u == _MGR_MedicalApp.instance.selectedAliment.GetComponent<BlocAliment>()))
            //    _MGR_MedicalApp.instance.repas.listAliments.Add(_MGR_MedicalApp.instance.selectedAliment.GetComponent<BlocAliment>());
            //_MGR_MedicalApp.instance.repas.totalPortions += 1;

            ////si il n'y a plus rien, passer au plat suivant
            if (MedicalAppManager.Instance().selectedAliment.GetComponent<PlateBlocAliment>().nbSlices == 0)
            {
                MealManager.Instance().MangerPortion.SetActive(false);
                DestroyImmediate(MedicalAppManager.Instance().selectedAliment);
            }
            MealManager.Instance().updateInfos();
        }
        else
        {
            MealManager.Instance().SetDumpingSyndrom(true);
            MealManager.Instance().DumpingToggle.SetActive(true);
            MealManager.Instance().EndMeal();
            MedicalAppManager.Instance().PortionsMax = true;
        }
    }

    
}
