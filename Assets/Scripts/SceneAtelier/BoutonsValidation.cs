using UnityEngine;
using UnityEngine.UI;

public class BoutonsValidation : MonoBehaviour {

    private Button btn;

	void Start () { 
        btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
	}
	
    private void TaskOnClick()
    {
        // si bouton oui ajouter aliement au repas
        if(name == "YES")
        {
            AtelierManager.Instance().addAlimentToMeal(MedicalAppManager.Instance().selectedAliment, (int)GameObject.Find("SliderAjout").GetComponent<Slider>().value);
        }
 
        MedicalAppManager.Instance().selectedAliment = null;
        AtelierManager.Instance().showValidation(false);
        //Camera.main.GetComponent<MouseLook>().setCameraFree(true);
    }
}
