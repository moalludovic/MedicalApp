using System.Collections.Generic;
using UnityEngine;


public struct Meal
{
    public List<PlateBlocAliment> listAliments;
    public float totalPortions;
}

public struct Plate
{
    public Aliment aliment;
    public int nbSlice;
}

public enum Operation
{
    sleeve,
    by_pass
}

public enum Gender
{
    man,
    woman
}

public class MedicalAppManager : MonoBehaviour {
    private static MedicalAppManager _instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.

    //profile 
    //(pt)
    public string userFirstName;
    public string userLastName;
    public Operation userOperation = Operation.sleeve;
    public Gender userGender = Gender.woman;
    private string sleeve = "Sleeve";
    private string by_pass = "By Pass";
    public bool UserDefine = false;

    //pout afficher les données de l'utilisateur
    public string toReplaceByFirstName = "firstName";
    public string toReplaceByLastName = "lastName";

    public GameObject selectedAliment;
    public List<Plate> exportablePlateau = new List<Plate>();//récupérés dans SceneMeal
    public Scenario theScenario;

    public float offset = 0.1f;

    public int xMaxPlateau = 4;//nombre l'aliements en hauteur et en largeur dans le plateau
    public int yMaxPlateau = 2;//nombre l'aliements en hauteur et en largeur dans le plateau
    public float xStepPlateau = 0.1f;//nombre l'aliements en hauteur et en largeur dans le plateau
    public float yStepPlateau = 0.08f;//nombre l'aliements en hauteur et en largeur dans le plateau

    public bool OnMeal = false;
    public bool PortionsMax = false;
    
    public float alimentSize = 2.0f;

    public float TestRatioScale = 0.75f;

    // intialisation de l'instance du manager
    void Awake()
    {
        //Check if instance already exists
        if (_instance == null)

            //if not, set instance to this
            _instance = this;

        //If instance already exists and it's not this:
        else if (_instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    // instialisation des objets
    private void Start()
    {
        //if (repas.listAliments == null)
        //    repas.listAliments = new List<PlateBlocAliment>();
        if (GetComponent<LoadAliment>() == null)
        {
            gameObject.AddComponent<LoadAliment>();
        }
    }

    // ################
    // ## Recuperation de l'instance
    // ################
    public static MedicalAppManager Instance()
    {
        return _instance;
    }

    // ################
    // ## mise en place des valeurs liées au profile
    // ################
    public void SetProfile(string firstName,string lastName, string operation, string gender)
    {
        userFirstName = firstName;
        userLastName = lastName;
        if(operation == sleeve){
            userOperation = Operation.sleeve;
        }
        else if (operation == by_pass)
        {
            userOperation = Operation.by_pass;
        }
        if (gender == "Homme")
        {
            userGender = Gender.man;
        }
        else if (gender == "Femme")
        {
            userGender = Gender.woman;
        }
        UserDefine = true;
    }

    // ################
    // ## affichage des noms et prénoms de l'utilisateur dans le texte
    // ################
    public string setUserDataInText(string txt){
        txt = txt.Replace(toReplaceByFirstName,userFirstName);
        txt= txt.Replace(toReplaceByLastName, userLastName);
        return txt;
    }

    // ################
    // ## changer le nombre de tranches de l'aliment sélectionné à parti du slider
    // ################
    public void setAlimentSlices(){
        selectedAliment.GetComponent<BlocAliment>().SetVisibleSliceWithSlider();
    }

    // ################
    // ## quitter l'application
    // ################
    public void CloseApplication()
    {
        Debug.Log("Application Quit");
        Application.Quit();
    }

    public bool IsSelectedDrink(){

        if(MedicalAppManager.Instance().selectedAliment.GetComponent<PlateBlocAliment>() != null){ 
            foreach (string type in MedicalAppManager.Instance().selectedAliment.GetComponent<PlateBlocAliment>().al.types)
            {
                if (type == "boisson")
                {
                    return true;
                }
            }
        }
        else if(MedicalAppManager.Instance().selectedAliment.GetComponent<BlocAliment>() != null){
            foreach (string type in MedicalAppManager.Instance().selectedAliment.GetComponent<BlocAliment>().aliment.types)
            {
                if (type == "boisson")
                {
                    return true;
                }
            }
        }
        return false;
    }

}
