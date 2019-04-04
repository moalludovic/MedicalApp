using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class AtelierManager : MonoBehaviour {

    //public string userName;
    //public string userLastName;
    //public bool userOperation;//true for sleeve and false for by pass


    private static AtelierManager _instance = null;

    public float meshSize;
    public Material mat;
    public GameObject BoutonPrefab;
    public GameObject ChoixScenariosParent;
    
    public scriptPlateau Plateau;

    public GameObject FenetreValidation;
    public GameObject FenetreModification;
    public GameObject FenetreValidationRepas;
    public GameObject infos;

    //caméra qui tourne 
    public Image CursorTarget;
    public Sprite[] Hands;
    public Text Consigne;

    public string[] Consignes = new string[3];

    private string _scenarioFilePath;
    private string _alimentFilePath;
    private ScenarioCollection _scenariosCollection;
    private AlimentCollection _alimentsCollection;
    
    private List<GameObject> _TypesButtonList;
    private List<GameObject> _TypesList;

    public GameObject plateBlocAliment;
    public int nbOnLine = 8;
    public float spacing = 2.0f;

    // intialisation de l'instance du manager
    private void Awake()
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

    // instialisation des objets
    private void Start()
    {
        _scenarioFilePath = Application.dataPath + "/Saves/scenarios" + ".txt"; //mettre un autre nom!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        _alimentFilePath = Application.dataPath + "/Saves/aliments" + ".txt";
        
        _TypesButtonList = new List<GameObject>();
        _TypesList = new List<GameObject>();
        
        displayScenarioChoice();

        activateTypeButtons(false);
        Plateau.gameObject.SetActive(false);
        infos.transform.parent.gameObject.SetActive(false);
    }


    // update objets
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Camera.main.GetComponent<MouseLook>().setCameraFree(false);
        }
    }

    // ################
    // ## Recuperation de l'instance
    // ################
    public static AtelierManager Instance()
    {
        return _instance;
    }

    // ################
    // ## charger choix scénarios et crréation d'UIs qui utilisent loadScenario 
    // ################
    public void displayScenarioChoice()
    {
        Consigne.text = Consignes[0];
        if (File.Exists(_scenarioFilePath))
        {
            string dataAsJson = File.ReadAllText(_scenarioFilePath);
            _scenariosCollection = JsonUtility.FromJson<ScenarioCollection>(dataAsJson);
            for (int i = 0; i < _scenariosCollection.scenarios.Count; ++i)
            {
                GameObject temp_button = Instantiate(BoutonPrefab, ChoixScenariosParent.transform);

                //positionnement
                temp_button.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, temp_button.GetComponent<RectTransform>().sizeDelta.y);//TODO régler la taille en fonction du nombre de ligne
                temp_button.GetComponent<RectTransform>().position = new Vector3(Screen.width / 2, (i + 0.5f) * temp_button.GetComponent<RectTransform>().sizeDelta.y);
                string toLoad = _scenariosCollection.scenarios[i].name;
                temp_button.GetComponent<Button>().onClick.AddListener(()=>loadScenarioChoice(toLoad));

                temp_button.transform.GetChild(0).transform.GetComponent<Text>().text = _scenariosCollection.scenarios[i].name;
            }
        }
        else
        {
            _scenariosCollection = new ScenarioCollection();
        }
    }

    // ################
    // ## charger scénario et quitter interface choix scénario (à utiliser dans 
    // ################
    public void loadScenarioChoice(string scenario)
    {
        for (int i = 0; i < _scenariosCollection.scenarios.Count; ++i) {
            if (_scenariosCollection.scenarios[i].name == scenario)
                MedicalAppManager.Instance().theScenario = _scenariosCollection.scenarios[i];
        }
        activateTypeButtons(true);
        Plateau.gameObject.SetActive(true);

        GetComponent<LoadHDRI>().LoadTexture(scenario, MedicalAppManager.Instance().theScenario.skyboxRotation);
        SetAliment();

        foreach (Transform child in ChoixScenariosParent.transform)
        {
            Destroy(child.gameObject);
        }
        //Camera.main.GetComponent<MouseLook>().setCameraFree(true);
    }

    // ################
    // ## chargement des aliments et placement dans la scene
    // ################
    private void SetAliment()
    {
        Consigne.text = Consignes[1];
        // espacement
        // #######
        // Récupération des types d'aliments
        // #######
        // liste des types d'aliments
        infos.transform.parent.gameObject.SetActive(true);
        if (File.Exists(_alimentFilePath))
        {
            string dataAsJson = File.ReadAllText(_alimentFilePath);
            _alimentsCollection = JsonUtility.FromJson<AlimentCollection>(dataAsJson);
            int i = 0;
            // gameObject vide parent
            GameObject parent = new GameObject("Aliments");
            parent.transform.parent = Instance().transform;





            int nb = MedicalAppManager.Instance().theScenario.aliments.Count;
            //int rest = nb % 3;
            //int nbOnLine;//nombre de colonnes
            float y = spacing / 2;
            //int nbSup = nb;
            //// calcul complique pour placer les aliments
            if (nb > nbOnLine)
            {
            //    switch (rest)
            //    {
            //        case 0:
            //            break;
            //        case 1:
            //            nbSup += 2;
            //            break;
            //        case 2:
            //            nbSup += 1;
            //            break;
            //        default:
            //            break;
            //    }
            //    nbOnLine = nbSup / 3;
            //    if (nb > 6)
            //    {
                    y = spacing;
            //    }
            }
            else
            {
            //    nbOnLine = nb;
                y = 0;
            }

            foreach (string alimentName in MedicalAppManager.Instance().theScenario.aliments)
            {
                foreach (Aliment a in _alimentsCollection.aliments) {
                    if(alimentName == a.name) {

                        // #######
                        // Instanciation des modèles en fonction de leur type
                        // #######
                        // tableau contenant les types d'aliments

                        
                        

                        // position
                        Vector3 pos = transform.position;


                        if (i % nbOnLine == 0 && i != 0)
                        {
                            // nouvelle ligne
                            y -= spacing;
                        }
                        // nouvelle colonne
                        if (i >= (int)(nb / nbOnLine) * nbOnLine)//si je suis sur la dernière ligne && si nb n'est pas un multiple de nbOnLine
                        {
                            float tempNbOnLine = nb - (int)(nb / nbOnLine) * nbOnLine;
                            pos.x = (-(tempNbOnLine - 1) / 2.0f + i % tempNbOnLine) * spacing;
                        }
                        else//calcul pour toutes les lignes pleines
                        {
                            pos.x = (-(nbOnLine - 1) / 2.0f + i % nbOnLine) * spacing;
                        }


                        pos.y = y;
                        pos.z = 0;
                        

                        // nom aliment
                        GameObject objName = new GameObject("name-" + a.name);
                        objName.transform.localScale = new Vector3(0.035f, 0.035f, 1.0f);
                        objName.transform.parent = parent.transform;
                        objName.transform.position = pos - Vector3.up * 1.5f;

                        objName.AddComponent<TextMesh>();
                        objName.GetComponent<TextMesh>().text = a.name;
                        objName.GetComponent<TextMesh>().anchor = TextAnchor.MiddleCenter;
                        objName.GetComponent<TextMesh>().fontSize = 100;

                        // aliment i + j

                        GameObject aliment = new GameObject();
                        aliment.AddComponent<MeshFilter>();
                        aliment.AddComponent<MeshRenderer>();
                        //GameObject obj = Resources.Load<GameObject>("Aliments/" + aliment.name);

                        // ajout script contenant les informations de l'aliments
                        if (!aliment.GetComponent<BlocAliment>())
                        {
                            aliment.AddComponent<BlocAliment>();
                        }
                        //chargement de l'aliment en fonction de si il est découpable ou si 
                        if (a.multiMesh)
                        {
                            aliment.GetComponent<MeshFilter>().mesh = Resources.Load<Mesh>("Aliments/Models/" + a.name + "/" + a.slices);
                            for(int j = 1;j<=a.slices;++j){
                                aliment.GetComponent<BlocAliment>().Meshs.Add(Resources.Load<Mesh>("Aliments/Models/" + a.name + "/" + j));
                            }
                            aliment.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Aliments/Textures/" + a.name+ a.slices);
                        }
                        else
                        {
                            aliment.GetComponent<MeshFilter>().mesh = Resources.Load<Mesh>("Aliments/Models/" + a.name);
                            aliment.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Aliments/Textures/" + a.name);
                        }
                        // ajout type, nom aliment, portion ->> string en minuscule et sans espaces
                        aliment.name = a.name;
                        aliment.GetComponent<BlocAliment>().aliment = a;
                        aliment.GetComponent<BlocAliment>().OnAtelier = true;
                        aliment.layer = 9;
                        aliment.tag = "Aliment";

                        //aliment.GetComponent<MeshRenderer>().material.mainTexture = Resources.Load<Texture>("Aliments/Textures/" + folders[folder][i].name);
                        

                        aliment.transform.localScale = meshSize / Mathf.Max(aliment.GetComponent<MeshFilter>().mesh.bounds.size.x, aliment.GetComponent<MeshFilter>().mesh.bounds.size.y, aliment.GetComponent<MeshFilter>().mesh.bounds.size.z) * Vector3.one;

                        // suppression collider si extiant + ajout box collider
                        if (aliment.GetComponent<Collider>())
                        {
                            Collider[] cols = aliment.GetComponents<Collider>();
                            foreach (Collider col in cols)
                                Destroy(col);
                        }
                        aliment.AddComponent<BoxCollider>();
                        aliment.GetComponent<BoxCollider>().isTrigger = true;

                        // Instaciation de l'aliment avec parent le type correspondant
                        //GameObject obj = Instantiate(aliment, pos, transform.rotation, parent.transform);
                        aliment.transform.position = pos;
                        aliment.transform.rotation = transform.rotation;
                        aliment.transform.parent = parent.transform;
                        
                        ++i;
                    }
                }
            }
        }
        else
        {
            Debug.Log("fichier descriptif des aliments non trouvé");
        }
    }

    // ################
    // ## afficher un type d'aliment en fonction de son nom
    // ################
    public void showAlimentType(string type)
    {
        for (int t = 0; t < _TypesList.Count; t++)
        {
            if (_TypesList[t].name == type)
                _TypesList[t].SetActive(true); // affiche le type selectionne
            else
                _TypesList[t].SetActive(false); // masque type non correspondant
        }
    }
    
    // ################
    // ## afficher ou non les boutons de types
    // ################
    public void activateTypeButtons(bool isActive)
    {
        foreach (GameObject button in _TypesButtonList)
        {
            button.SetActive(isActive);
        }
    }
    
    // ################
    // ## afficher /masquer la zone de validation
    // ################
    public void showValidation(bool valid)
    {
        if (valid)
        {
            FenetreValidation.SetActive(valid);
            if (GameObject.Find("SliderAjout"))
                GameObject.Find("SliderAjout").GetComponent<SliderAjout>().InitAliment();
        }
        else
        {
            if (GameObject.Find("SliderAjout"))
            {
                GameObject.Find("SliderAjout").GetComponent<SliderAjout>().DeleteVisualFeedBack();
            }
            FenetreValidation.SetActive(valid);
        }
    }
    
    // ################
    // ## afficher /masquer la zone de modification
    // ################
    public void showModification(bool valid)
    {
        if (valid)
        {
            //récupérer tous les aliments semblables et les utiliser pour initialiser le slider
            FenetreModification.SetActive(true);
            GameObject.Find("SliderModif").GetComponent<SliderModif>().InitSlider();
        }
        else
        {
            //if (GameObject.Find("SliderModif"))
            //    GameObject.Find("SliderModif").GetComponent<SliderModif>().sameAliments.Clear();
            FenetreModification.SetActive(false);
        }
    }

    // ################
    // ## afficher /masquer la zone de validation du repas
    // ################
    public void showMealValidation(bool valid)
    {
        FenetreValidationRepas.SetActive(valid);
    }

    // ################
    // ## ajouter un aliment au repas
    // ################
    public void addAlimentToMeal(GameObject al, int nbSlice){ 
        bool isAlreadyAdded = false;
        foreach(GameObject oldAliment in Plateau.GetComponent<scriptPlateau>().aliments){
            if(oldAliment.GetComponent<PlateBlocAliment>().al== al.GetComponent<BlocAliment>().aliment){
                oldAliment.GetComponent<PlateBlocAliment>().nbSlices += nbSlice;
                oldAliment.GetComponent<PlateBlocAliment>().SetAlimentQuantity();
                oldAliment.GetComponent<PlateBlocAliment>().SetAlimentsPositions();
                isAlreadyAdded = true;
            }
        }
        if (!isAlreadyAdded && nbSlice > 0 && Plateau.GetComponent<scriptPlateau>().aliments.Count < MedicalAppManager.Instance().xMaxPlateau * MedicalAppManager.Instance().yMaxPlateau)
        {
            //je rajoute un aliment
            GameObject parent = Instantiate<GameObject>(plateBlocAliment, Plateau.transform);
            //GameObject temp = loader.LoadAndSetParent(al.GetComponent<BlocAliment>().aliment, parent.transform);
            parent.transform.position = Plateau.transform.position;
            parent.transform.localScale *= 0.06f;
                parent.transform.rotation = Quaternion.AngleAxis(-90, Vector3.right);
    
            parent.GetComponent<PlateBlocAliment>().al = al.GetComponent<BlocAliment>().aliment;
            parent.GetComponent<PlateBlocAliment>().nbSlices = nbSlice;
            parent.GetComponent<PlateBlocAliment>().SetAlimentQuantity();
            //interaction impossible au départ
            parent.GetComponent<BoxCollider>().enabled = false;
    
            //ajout dans le plateau
            Plateau.GetComponent<scriptPlateau>().aliments.Add(parent);
            Plateau.GetComponent<scriptPlateau>().SetAlimentsPositions();
            parent.GetComponent<PlateBlocAliment>().SetAlimentsPositions();

        }
        updateInfosRepas();
    }
    
    // ################
    // ## mise a jour des infos du repas
    // ################
    public void updateInfosRepas()
    {
        float totalPortions = 0.0f;
        foreach (GameObject obj in Plateau.GetComponent<scriptPlateau>().aliments)
        {
            PlateBlocAliment al = obj.GetComponent<PlateBlocAliment>();
            totalPortions += al.al.portions * al.nbSlices / al.al.slices;
        }

        infos.transform.GetChild(0).gameObject.GetComponent<Text>().text = "- nombre d'aliments : " + Plateau.GetComponent<scriptPlateau>().aliments.Count;
        infos.transform.GetChild(1).gameObject.GetComponent<Text>().text = "- nombre de portions : " + totalPortions.ToString();
    }

    // ################
    // ## transfere du plateau a la scene repas
    // ################
    public void LoadMeal()
    {
        MedicalAppManager.Instance().exportablePlateau.Clear();
        foreach (GameObject it in Instance().Plateau.GetComponent<scriptPlateau>().aliments)
        {
            Plate newPlate;
            newPlate.aliment = it.GetComponent<PlateBlocAliment>().al;
            newPlate.nbSlice = it.GetComponent<PlateBlocAliment>().nbSlices;
            MedicalAppManager.Instance().exportablePlateau.Add(newPlate);
        }

        Debug.Log("Chargement Repas");
        SceneManager.LoadScene("Scenes/SceneMeal", LoadSceneMode.Single);
    }

    public void UpdateAlimentPlateau()
    {
        foreach (GameObject it in Plateau.GetComponent<scriptPlateau>().aliments)//je vérifie que je n'ai pas déjà l'aliment dans ma liste
        {
                it.GetComponent<PlateBlocAliment>().updateAliments();
        }
    }

    public void setCursor(string mode = "")
    {
        if (mode == "open")
        {
            CursorTarget.sprite = Hands[0];
        }
        else if (mode == "closed")
        {
            CursorTarget.sprite = Hands[1];
        }
    }
}
