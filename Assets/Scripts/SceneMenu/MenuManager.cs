using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    private static MenuManager _instance = null;

    public Button btnValider;
    public Button btnQuestionnaire;
    public Button btnAtelier;

    public GameObject ProfileInterface;
    public GameObject MenuInterface;

    public Text Consigne;
    public string[] Consignes = new string[2];

    public GameObject Man;
    public GameObject Woman;

    //data à récupére
    public Text userName;
    public Text userLastName;
    public Text userOperation;
    public Text userGender;
    public Text Error;
    //affichage
    public Text nameMenuDisplay;

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

    private void Start()
    {

        if (!MedicalAppManager.Instance().UserDefine)
        {
            ProfileInterface.SetActive(true);
            MenuInterface.SetActive(false);
            Consigne.text = Consignes[0];

            MajModelWithInterface();
        }
        else
        {
            ProfileInterface.SetActive(false);
            MenuInterface.SetActive(true);
            Consigne.text = Consignes[1];
            nameMenuDisplay.text = "Bonjour " + MedicalAppManager.Instance().userFirstName;

            MajModelWithProfile();
        }

        float ratio = (float)Screen.width / (float)Screen.height;
        if (ratio < (16.0f / 9.0f)) {
            MedicalAppManager.Instance().TestRatioScale = 0.75f;
        }
        else
        {
            MedicalAppManager.Instance().TestRatioScale = 1.0f;
        }     
    }

    // ################
    // ## Recuperation de l'instance
    // ################
    public static MenuManager Instance()
    {
        return _instance;
    }
    
    // ################
    // ## Validation du profil utilisateur
    // ################
    public void validateProfile()
    {
        if (userName.text != "" && userLastName.text != "")
        {
            MedicalAppManager.Instance().SetProfile(userName.text, userLastName.text, userOperation.text, userGender.text);
            nameMenuDisplay.text = "Bonjour " + userName.text;
            Consigne.text = Consignes[1];
            ProfileInterface.SetActive(false);
            MenuInterface.SetActive(true);
            Error.text = "";
        }
        else
        {
            Error.text = "Veuillez entrer un texte valide";
        }
    }

    // ################
    // ## Chargement scene questionnaire
    // ################
    public void LoadQuestionnaire()
    {
        Debug.Log("Chargement questionnaire");
        SceneManager.LoadScene("Scenes/SceneQuestions", LoadSceneMode.Single);
    }    
    
    // ################
    // ## Quitter l'application
    // ################
    public void QuitApplication()
    {
        Debug.Log("Quit Application");
        Application.Quit();
    }


    //affichage bon modèle
    public void MajModelWithInterface(){
        if(userGender.text == "Homme"){
            Man.SetActive(true);
            Woman.SetActive(false);
        }
        else if(userGender.text == "Femme")
        {
            Man.SetActive(false);
            Woman.SetActive(true);
        }
    }

    public void MajModelWithProfile()
    {
        if (MedicalAppManager.Instance().userGender == Gender.man)
        {
            Man.SetActive(true);
            Woman.SetActive(false);
        }
        else if(MedicalAppManager.Instance().userGender == Gender.woman)
        {
            Man.SetActive(false);
            Woman.SetActive(true);
        }
    }
}
