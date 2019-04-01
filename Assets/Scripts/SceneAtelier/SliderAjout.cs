using UnityEngine;
using UnityEngine.UI;

public class SliderAjout : MonoBehaviour
{
    private LoadAliment loader;
    public GameObject Aliment1; //pour le feedback visuel lorsqu'on modifie le slider
    public GameObject Aliment2; //pour le feedback visuel lorsqu'on modifie le slider
    
    public void InitAliment(){
        if (Aliment1 != null || Aliment2 != null)
        {
            GameObject.Find("SliderAjout").GetComponent<SliderAjout>().DeleteVisualFeedBack();
        }
        if (loader == null)
        {
            loader = MedicalAppManager.Instance().gameObject.GetComponent<LoadAliment>();
        }
        Aliment1 = loader.LoadWithSliceManagement(MedicalAppManager.Instance().selectedAliment.GetComponent<BlocAliment>().aliment);
        Aliment2 = loader.LoadWithSliceManagement(MedicalAppManager.Instance().selectedAliment.GetComponent<BlocAliment>().aliment);

        Aliment1.transform.position = new Vector3(-1.5f, 3, 0);
        if (Aliment1.GetComponent<BlocAliment>().normal.z == 1)
        {
            Aliment1.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        Aliment2.transform.position = new Vector3(1.5f, 3, 0);
        if (Aliment2.GetComponent<BlocAliment>().normal.z == 1)
        {
            Aliment2.transform.rotation = Quaternion.Euler(0, 90, 0);
        }

        GetComponent<Slider>().maxValue = MedicalAppManager.Instance().selectedAliment.GetComponent<BlocAliment>().aliment.slices * 2;
        if (GetComponent<Slider>().value == MedicalAppManager.Instance().selectedAliment.GetComponent<BlocAliment>().aliment.slices)
            ModificationAliment();
        GetComponent<Slider>().value = MedicalAppManager.Instance().selectedAliment.GetComponent<BlocAliment>().aliment.slices;// en gros, la moitié du slider et l'équivalent d'un aliment plein
            
    }

    public void ModificationAliment()
    {
        int value = (int)gameObject.GetComponent<Slider>().value;
        //_MGR_MedicalApp.instance.selectedAliment.GetComponent<BlocAliment>().nbSlices = value;
        //_MGR_MedicalApp.instance.updateInfosRepas();
        GameObject Txt = gameObject.transform.parent.transform.GetChild(1).gameObject;
        Txt.GetComponent<Text>().text = MedicalAppManager.Instance().selectedAliment.GetComponent<BlocAliment>().aliment.name + "\n" + value.ToString() + " tranche(s)";

        // modification du texte en fonction du type d'aliement
        if (MedicalAppManager.Instance().IsSelectedDrink())
        {
            Txt.GetComponent<Text>().text = MedicalAppManager.Instance().selectedAliment.GetComponent<BlocAliment>().aliment.name + "\n" + value.ToString() + " gorgée(s)";
        }
        else
        {
            Txt.GetComponent<Text>().text = MedicalAppManager.Instance().selectedAliment.GetComponent<BlocAliment>().aliment.name + "\n" + value.ToString() + " tranche(s)";
        }

        //feedback visuel
        if (value <= MedicalAppManager.Instance().selectedAliment.GetComponent<BlocAliment>().aliment.slices)
       {
           Aliment1.GetComponent<BlocAliment>().nbSlices = value;
           Aliment2.GetComponent<BlocAliment>().nbSlices = 0;
       }
       else{
           Aliment1.GetComponent<BlocAliment>().nbSlices = MedicalAppManager.Instance().selectedAliment.GetComponent<BlocAliment>().aliment.slices;
           Aliment2.GetComponent<BlocAliment>().nbSlices = value- MedicalAppManager.Instance().selectedAliment.GetComponent<BlocAliment>().aliment.slices;
       }
       Aliment1.GetComponent<BlocAliment>().SetAspectWithSlice();
       Aliment2.GetComponent<BlocAliment>().SetAspectWithSlice();
       if(Aliment1.GetComponent<BlocAliment>().aliment.multiMesh){
            Aliment1.GetComponent<BlocAliment>().SetDefaultSize();
            Aliment2.GetComponent<BlocAliment>().SetDefaultSize();
        }
    }

    public void DeleteVisualFeedBack(){

        if(Aliment1!=null){
            Destroy(Aliment1);
        }
        if(Aliment2!=null){
            Destroy(Aliment2);
        }
    }
}
