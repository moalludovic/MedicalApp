using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MealManager : MonoBehaviour {

    private static MealManager _instance = null;

    public string EndGameMessage;

    public GameObject Man;
    public GameObject Woman;
    public Animator AnimationMan;
    public Animator AnimationWoman;

    public GameObject MangerPortion;
    public GameObject DumpingToggle;
    public GameObject DumpingWarning;
    public GameObject AnalyseActivator;
    public GameObject AutoEvalActivator;
    public GameObject AutoEvaluation;
    public GameObject TimerUI;
    public GameObject EndMealActivator;
    public Text Infos;
    public GameObject StomachAche;
    public List<PlateBlocAliment> EatenAliments;

    public Text Consigne;
    public string[] Consignes = new string[1];

    //stockage du repas
    public List<PlateBlocAliment> listAliments;
    public float totalPortions;
    
    public scriptPlateau plateau;
    public GameObject plateBlocAliment;

    private GameObject toEat;
    private float nbPortions;
    private string type, alimentName;


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
    }

    // Use this for initialization
    void Start()
    {
        MedicalAppManager.Instance().OnMeal = true;

        //afficher bon modèle
        if (MedicalAppManager.Instance().userGender == Gender.woman) {
            Woman.SetActive(true);
        }
        else if (MedicalAppManager.Instance().userGender == Gender.man)
        {
            Man.SetActive(true);
        }

        // tableau pour l'analyse
        EatenAliments = new List<PlateBlocAliment>();
        //aliment
        listAliments = new List<PlateBlocAliment>();
        totalPortions = 0;

        Consigne.text = Consignes[0];

        /*
        //pour tester
        Aliment tempAliment = new Aliment();
        tempAliment.name = "pain";
        tempAliment.type = "Test";
        tempAliment.name = "pain";
        tempAliment.portions = 4;
        tempAliment.slices = 5;
        tempAliment.sugary = true;
        tempAliment.greasy = false;
        tempAliment.cold = false;
        tempAliment.hot = false;
        Plate temp = new Plate();
        temp.nbSlice = 1;
        temp.aliment = tempAliment;
        MedicalAppManager.Instance().exportablePlateau.Add(temp);
        */
        
        updateInfos();
        //hdri
        GetComponent<LoadHDRI>().LoadTexture(MedicalAppManager.Instance().theScenario.name, MedicalAppManager.Instance().theScenario.skyboxRotation);

        loadAllAlimentsFromExport();
        MangerPortion.SetActive(false);
    }

    // ################
    // ## Recuperation de l'instance
    // ################
    public static MealManager Instance()
    {
        return _instance;
    }

    public void loadAllAlimentsFromExport()
    {
        foreach(Plate p in MedicalAppManager.Instance().exportablePlateau)
        {
            //je rajoute un aliment
            GameObject parent = Instantiate<GameObject>(plateBlocAliment, plateau.transform);
            //GameObject temp = loader.LoadAndSetParent(al.GetComponent<BlocAliment>().aliment, parent.transform);
            parent.transform.position = plateau.transform.position;
            parent.transform.localScale *= 0.06f;
            parent.transform.rotation = Quaternion.AngleAxis(-90, Vector3.right);

            parent.GetComponent<PlateBlocAliment>().al = p.aliment;
            parent.GetComponent<PlateBlocAliment>().nbSlices = p.nbSlice;
            parent.GetComponent<PlateBlocAliment>().SetAlimentQuantity();
            parent.GetComponent<PlateBlocAliment>().SetAlimentsPositions();

            //ajout dans le plateau
            plateau.GetComponent<scriptPlateau>().aliments.Add(parent);
            plateau.GetComponent<scriptPlateau>().SetAlimentsPositions();

        }
    }

    public void loadAllAlimentsFromList(List<PlateBlocAliment> aliments)
    {
        foreach (PlateBlocAliment aliment in aliments)
        {
            //je rajoute un aliment
            GameObject parent = Instantiate<GameObject>(plateBlocAliment, plateau.transform);
            parent.transform.position = plateau.transform.position;
            parent.transform.localScale *= 0.06f;
            parent.transform.rotation = Quaternion.AngleAxis(-90, Vector3.right);

            parent.GetComponent<PlateBlocAliment>().al = aliment.al;
            parent.GetComponent<PlateBlocAliment>().nbSlices = aliment.nbSlices;
            parent.GetComponent<PlateBlocAliment>().SetAlimentQuantity();
            parent.GetComponent<PlateBlocAliment>().SetAlimentsPositions();

            //ajout dans le plateau
            plateau.GetComponent<scriptPlateau>().aliments.Add(parent);
            plateau.GetComponent<scriptPlateau>().SetAlimentsPositions();

        }
    }

    private void Update()
    {

    }

    public void updateInfos()
    {

        if (!MedicalAppManager.Instance().PortionsMax)
        {
            if (MedicalAppManager.Instance().selectedAliment != null)
            {
                List<string> t = MedicalAppManager.Instance().selectedAliment.GetComponent<PlateBlocAliment>().al.types;
                string n = MedicalAppManager.Instance().selectedAliment.GetComponent<PlateBlocAliment>().al.name;
                float p = MedicalAppManager.Instance().selectedAliment.GetComponent<PlateBlocAliment>().nbSlices;

                //types
                Infos.text = "Type(s) : ";
                foreach(string type in t){
                    Infos.text += type + " ";
                }
                //autres
                if (MedicalAppManager.Instance().IsSelectedDrink())
                {
                    Infos.text += "\nAliment : " + n + "\nNombre de gorgée(s) restante(s) : " + p.ToString();
                }
                else
                {
                    Infos.text += "\nAliment : " + n + "\nNombre de tranche(s) restante(s) : " + p.ToString();
                }
            }
            else
            {
                Infos.text = "Veuillez sélectionner un aliment";
            }
        }
    }

    public void Analyse()
    {
        EndMealActivator.SetActive(false);
        Man.SetActive(false);
        Woman.SetActive(false);
        TimerUI.SetActive(false);
        //mise en place du texte
        AnalyseActivator.SetActive(false);
        AutoEvalActivator.SetActive(true);
        
        // affichage liste aliments mangés
        Infos.text = "Vous avez mangé : \n" ;
        Dictionary<string, int> types = new Dictionary<string, int>();
        float totalPortions = 0;

        //bool greasy = false;
        //bool sugary = false;
        //bool hot = false;
        //bool cold = false;

        foreach(PlateBlocAliment al in EatenAliments)
        {
            // recuperation des types differents et de leur nombre
            foreach (string type in al.al.types)
            {
                //typeAliment
                if (!types.ContainsKey(type))
                {
                    types.Add(type, 1);
                }
                else
                {
                    types[type] += 1;
                }
            }
            totalPortions += al.nbSlices * al.al.portions / al.al.slices;
        }
        // affichage infos recuperés
        Infos.text += " - " + types.Count + " type(s) d'aliments différent(s)\n";
        Infos.text += " - " + EatenAliments.Count + " aliment(s) différent(s)\n";        
        Infos.text += "Vous avez mangé au total " + totalPortions + " portion(s).\n\n";


        //duping syndrom
        //if (greasy || sugary || hot || cold)
        //{
        //    if (greasy)
        //    {
        //        Infos.text += "Vous avez mangé un ou des aliments gras\n";
        //    }
        //    if (sugary)
        //    {
        //        Infos.text += "Vous avez mangé un ou des aliments sucré(s)\n";
        //    }
        //    if (hot)
        //    {
        //        Infos.text += "Vous avez mangé un ou des aliments chaud(s)\n";
        //    }
        //    if (cold)
        //    {
        //        Infos.text += "Vous avez mangé un ou des aliments froid(s)\n";
        //    }
        //    Infos.text += "Si vous aviez exagéré, vous auriez pu avoir le dumping syndrom.";
        //    DumpingToggle.SetActive(true);
        //}
        if (MedicalAppManager.Instance().userOperation==Operation.by_pass){
            Infos.text += "Risque de dumping syndrome si trop gras, trop sucré, trop salé, trop chaud, trop froid \nTolérance variable selon les personnes; à tester d’abord en petite quantité et observer les effets \n";
            SetDumpingToggleActive(true);
        }

    }

    public void ShowAutoEvaluation(){
        //trouver un moyen pour que ça se fasse après la partie dumping syndrom
        gameObject.GetComponent<AutoEval>().loadQuestion();
        AutoEvalActivator.SetActive(false);
        SetDumpingToggleActive(false);
        SetDumpingSyndrom(false);
        AutoEvaluation.SetActive(true);
    }
    

    public void ToggleDumpingSyndrom()
    {
        if(StomachAche.activeSelf){
            SetDumpingSyndrom(false);
        }
        else{

            SetDumpingSyndrom(true);
        }
    }

    public void SetDumpingSyndrom(bool state){
        Camera.main.GetComponent<PostProcessing>().SetDumpingSyndrome(state);
        StomachAche.SetActive(state);
    }

    public void AddEatenAliment(){
        bool alreadyAdd = false;
        string testName = MedicalAppManager.Instance().selectedAliment.GetComponent<PlateBlocAliment>().al.name;
        PlateBlocAliment temp;
        foreach (PlateBlocAliment al in EatenAliments){
            if (al.al.name == testName)
            {
                alreadyAdd = true;
                al.nbSlices += 1;
            }
        }
        if (!alreadyAdd )
        {
            temp = new PlateBlocAliment(); 
            temp.al =  MedicalAppManager.Instance().selectedAliment.GetComponent<PlateBlocAliment>().al;
            temp.nbSlices = 1;
            EatenAliments.Add(temp);
        }
    }

    public void SetMouthAnimation(float time)
    {
        MangerPortion.transform.GetChild(0).GetComponent<Button>().interactable = false;
        MangerPortion.transform.GetChild(1).GetComponent<Button>().interactable = false;
        EndMealActivator.GetComponent<Button>().interactable = false;
        if (MedicalAppManager.Instance().userGender == Gender.woman)
        {
            AnimationWoman.SetBool("isActive", true);
        }
        else if (MedicalAppManager.Instance().userGender == Gender.man)
        {
            AnimationMan.SetBool("isActive", true);
        }
        StartCoroutine(MouthAnimationEnd(time));
        TimerUI.SetActive(true);
        TimerUI.transform.GetChild(0).GetComponent<TimerMastication>().initializeTimer(time);
    }
    IEnumerator MouthAnimationEnd(float time)
    {
        yield return new WaitForSeconds(time);
        if (MedicalAppManager.Instance().userGender == Gender.woman)
        {
            AnimationWoman.SetBool("isActive", false);
        }
        else if (MedicalAppManager.Instance().userGender == Gender.man)
        {
            AnimationMan.SetBool("isActive", false);
        }
        MangerPortion.transform.GetChild(0).GetComponent<Button>().interactable = true;
        MangerPortion.transform.GetChild(1).GetComponent<Button>().interactable = true;
        EndMealActivator.GetComponent<Button>().interactable = true;
    }

    public void EndMeal()
    {
        plateau.Clear();

        SetDumpingToggleActive(false);
        MangerPortion.SetActive(false);
        AnalyseActivator.SetActive(true);
        plateau.gameObject.SetActive(false);
        EndMealActivator.SetActive(false);
        Infos.text = "Repas terminé";
        Consigne.transform.parent.gameObject.SetActive(false);
    }

    public void SetDumpingToggleActive(bool isActive){
        DumpingToggle.SetActive(isActive);
        DumpingWarning.SetActive(isActive);
    }

    public List<string> getMealTypes(){
        List<string> mealtypes = new List<string>();
        foreach (PlateBlocAliment al in EatenAliments)
        {
            if (al.al.mealType1 != "")
            {
                if (al.al.mealType2 != "" && al.al.mealTypeThreshold != 0
                && al.al.mealTypeThreshold <= al.al.portions / (float)al.al.slices * al.nbSlices)
                {
                    if (!mealtypes.Contains(al.al.mealType2))
                        mealtypes.Add(al.al.mealType2);
                }
                else
                {
                    if (!mealtypes.Contains(al.al.mealType1))
                        mealtypes.Add(al.al.mealType1);
                }
            }
        }
        return mealtypes;
    }

    //public void Control(){
    //    if (MedicalAppManager.Instance().PortionsMax)
    //    {
    //        if (plateau.transform.childCount > 1)
    //        {
    //            EndMeal();
    //        }
    //        SetDumpingToggleActive(true);
    //        Infos.text = "Vous avez atteint la limite de consommation";
    //        plateau.gameObject.SetActive(false);
    //        AnalyseActivator.SetActive(false);
    //        MangerPortion.SetActive(false);
    //    }
    //    else if (plateau.transform.childCount == 1)
    //    {
    //        SetDumpingToggleActive(false);
    //        MangerPortion.SetActive(false);
    //        AnalyseActivator.SetActive(true);
    //        plateau.gameObject.SetActive(false);
    //        EndMealActivator.SetActive(false);
    //        Infos.text = "Repas terminé";
    //    }
    //}

    // ################
    // ## Chargement scene menu
    // ################
    public void LoadMenu()
    {
        Debug.Log("Chargement menu");
        SceneManager.LoadScene("Scenes/SceneMenu", LoadSceneMode.Single);
    }
}
