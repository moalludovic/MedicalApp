using UnityEngine;
using UnityEngine.UI;

public class BoutonsModification : MonoBehaviour {

    private Button btn;

    void Start()
    {
        btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    private void TaskOnClick()
    {
        // suppression de l'aliment du plateau
        if (name == "delete")
        {
            Remove();
        }
        // fermeture de la zone de modification
        if (name == "ok")
        {
            Close();
        }
    }

    void Remove()
    {
        //destruction dans les listes + gestion des portions
        GameObject toDeleate = MedicalAppManager.Instance().selectedAliment;
        
        foreach (Transform temp in MedicalAppManager.Instance().selectedAliment.transform)//je détruit ceux en trop
        {
            temp.parent = null;
            Destroy(temp.gameObject);
        }
        
        AtelierManager.Instance().Plateau.GetComponent<scriptPlateau>().aliments.Remove(toDeleate);
        Destroy(toDeleate);

        //replacer les aliments dans le plateau et réafficher repas
        AtelierManager.Instance().Plateau.GetComponent<scriptPlateau>().SetAlimentsPositions();
        AtelierManager.Instance().updateInfosRepas();

        Close();
    }

    void RemoveAliment(){
        
    }

    void Close(){
        MedicalAppManager.Instance().selectedAliment = null;
        AtelierManager.Instance().showModification(false);
    }
}
