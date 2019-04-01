using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AutoEval : MonoBehaviour {
    public GameObject choices_group;
    public GameObject button_prefab;
    public List<GameObject> choice_buttons;
    public Button ok;
    public Image smiley;
    public GameObject perfect_meal;
    public Text infos;
    public Button back;
    public GameObject loadMenu;

    string filePath;
    AutoEvalQuestions list = new AutoEvalQuestions();
    int nextQuestion = 0;
    List<int> choice_save = new List<int>();

    // Use this for initialization
    void Start () {
        filePath = Application.dataPath + "/Saves/AutoEval" + ".txt"; //mettre un autre nom!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        string dataAsJson = File.ReadAllText(filePath);
        list = JsonUtility.FromJson<AutoEvalQuestions>(dataAsJson);

        //loadNextQuestion();
    }
	
    public void loadNextQuestion(){ //utilisé par le bouton "ok"
        ++nextQuestion;
        loadQuestion();
    }

    public void loadLastQuestion(){
        if(smiley.gameObject.activeSelf == true){
            choice_save.RemoveAt(choice_save.Count - 1);
            loadQuestion();
        }
        else if(nextQuestion>=1){
            --nextQuestion;
            //stocker les choix de l'utilisateur
            userChoice(choice_save[choice_save.Count-1]);
        }
    }

    public void loadQuestion(){
        perfect_meal.SetActive(false);
        MealManager.Instance().plateau.gameObject.SetActive(false);
        if (nextQuestion < list.questions.Count)
        {
            if(nextQuestion>=1){
                back.interactable = true;
            }
            else{
                back.interactable = false;
            }
            infos.text = MedicalAppManager.Instance().setUserDataInText(list.questions[nextQuestion].Text);
            smiley.gameObject.SetActive(false);
            SetChoices();
            ok.gameObject.SetActive(false);
        }
        else
        {
            //charger texte de fin
            infos.text = MealManager.Instance().EndGameMessage;
            loadMenu.SetActive(true);
            MealManager.Instance().AutoEvaluation.SetActive(false);
        }
    }

    public void userChoice(int choice){
        choice_save.Add(choice);
        back.interactable = true;
        ClearChoices();
        //réponse (list.questions[nextQuestion].Answer)
        //image (couleur smiley dépendante d'un switch et des données du repas)
        smiley.gameObject.SetActive(true);
        switch(nextQuestion){
            case 0:
                int result = 2;
                List<string> mealtypes = MealManager.Instance().getMealTypes();
                if(mealtypes.Count<3){
                    result -= 1;
                }
                if(!mealtypes.Contains("plat principal")){
                    result -= 1;
                }

                if(result == 0)
                {
                    smiley.sprite = Resources.Load<Sprite>("UI/Image/SmileyRouge");
                }
                else if(result == 1)
                {
                    smiley.sprite = Resources.Load<Sprite>("UI/Image/SmileyOrange");
                }
                else if(result == 2)
                {
                    smiley.sprite = Resources.Load<Sprite>("UI/Image/SmileyVert");
                }
                
                Debug.Log(choice + " " + result);
                Debug.Log(list.questions[nextQuestion].Answers.Count);

                infos.text = MedicalAppManager.Instance().setUserDataInText(list.questions[nextQuestion].Answers[choice * 3 + result]);

                perfect_meal.SetActive(true);

                MealManager.Instance().plateau.gameObject.SetActive(true);
                MealManager.Instance().plateau.transform.localScale = Vector3.one* 2.7f;
                MealManager.Instance().plateau.transform.localPosition = new Vector3(0.0f, -0.28f, 1.0f);
                MealManager.Instance().plateau.Clear();
                MealManager.Instance().loadAllAlimentsFromList(MealManager.Instance().EatenAliments);
                MealManager.Instance().plateau.CanInteractWithAliments(false);



                break;
            case 1:
                //couleurs en fonction du nombre de plats ( < 4 --> rouge / =4 --> orange / >= 5 --> vert ==> si nombres portions incorrectes --> orange)
                int nbAliments = MealManager.Instance().EatenAliments.Count;
                if (nbAliments >= 5)
                {
                    smiley.sprite = Resources.Load<Sprite>("UI/Image/SmileyVert");
                    infos.text = MedicalAppManager.Instance().setUserDataInText(list.questions[nextQuestion].Answers[choice*3+2]);
                }
                else if (nbAliments == 4)
                {
                    smiley.sprite = Resources.Load<Sprite>("UI/Image/SmileyOrange");
                    infos.text = MedicalAppManager.Instance().setUserDataInText(list.questions[nextQuestion].Answers[choice * 3 + 1]);
                }
                else
                {
                    smiley.sprite = Resources.Load<Sprite>("UI/Image/SmileyRouge");
                    infos.text = MedicalAppManager.Instance().setUserDataInText(list.questions[nextQuestion].Answers[choice * 3 + 0]);
                }
                break;
            case 2:
                //couleurs en fonction du nombre de portions (rouge si < à 20 ou > à 40  orange si entre 20 et 25 et entre 35 et 40 vert  si entre 26 et 34)
                float nbPortions = MealManager.Instance().totalPortions;
                if(nbPortions>=26&&nbPortions<=34){
                    smiley.sprite = Resources.Load<Sprite>("UI/Image/SmileyVert");
                    infos.text = MedicalAppManager.Instance().setUserDataInText(list.questions[nextQuestion].Answers[choice * 5 + 2]);
                }
                else if (nbPortions >= 20 && nbPortions <= 40)
                {
                    smiley.sprite = Resources.Load<Sprite>("UI/Image/SmileyOrange");
                    if(nbPortions<26){
                        infos.text = MedicalAppManager.Instance().setUserDataInText(list.questions[nextQuestion].Answers[choice * 5 + 1]);
                    }
                    else{
                        infos.text = MedicalAppManager.Instance().setUserDataInText(list.questions[nextQuestion].Answers[choice * 5 + 3]);
                    }
                }
                else{
                    smiley.sprite = Resources.Load<Sprite>("UI/Image/SmileyRouge");
                    if (nbPortions < 20)
                    {
                        infos.text = MedicalAppManager.Instance().setUserDataInText(list.questions[nextQuestion].Answers[choice * 5 + 0]);
                    }
                    else
                    {
                        infos.text = MedicalAppManager.Instance().setUserDataInText(list.questions[nextQuestion].Answers[choice * 5 + 4]);
                    }
                }
                break;
        }
        //bouton "OK" (charge load next question)
        ok.gameObject.SetActive(true);
    }

    public void ClearChoices()//and delete old demo
    {

        for (int i = 0; i < choice_buttons.Count; ++i)
        {
            Destroy(choice_buttons[i]);
        }
        choice_buttons.Clear();
    }

    public void SetChoices()
    {
        for (int i = 0; i < list.questions[nextQuestion].Choices.Count; ++i)
        {
            GameObject temp_button = Instantiate(button_prefab, choices_group.transform);
            
            //positionnement
            temp_button.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, temp_button.GetComponent<RectTransform>().sizeDelta.y);//TODO régler la taille en fonction du nombre de ligne
            temp_button.GetComponent<RectTransform>().position = new Vector3(Screen.width / 2, (i + 0.5f) * temp_button.GetComponent<RectTransform>().sizeDelta.y);
            //ajout données nécessaires au bon fonctionnement
            int temp = i;
            temp_button.GetComponent<Button>().onClick.AddListener(() => userChoice(temp));
            
            temp_button.transform.GetChild(0).transform.GetComponent<Text>().text = MedicalAppManager.Instance().setUserDataInText(list.questions[nextQuestion].Choices[i]);

            choice_buttons.Add(temp_button);
        }
    }
}
