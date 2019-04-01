using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.SceneManagement;

public class LoadXMLQuestions : MonoBehaviour {
    public string path_questions;
    string xml_questions;


    public string sleeveQuestion;//Moyen d'initialiser, il y a peut être plus pertinent
    public string byPassQuestion;//Moyen d'initialiser, il y a peut être plus pertinent

    public List<string> saveQuestion = new List<string>();

	// Use this for initialization
	void Start () {
        xml_questions = Resources.Load<TextAsset>(path_questions).text;
        if(MedicalAppManager.Instance().userOperation==Operation.sleeve){
            loadAndSaveQuestion(sleeveQuestion);
        }
        else if (MedicalAppManager.Instance().userOperation == Operation.by_pass)
        {
            loadAndSaveQuestion(byPassQuestion);
        }
    }

    public void loadLastQuestion() {
        if (saveQuestion.Count >= 2) {
            saveQuestion.RemoveAt(saveQuestion.Count - 1);
            loadQuestion(saveQuestion[saveQuestion.Count - 1]);
        }
    }

    public void loadAndSaveQuestion(string next)
    {
        saveQuestion.Add(next);
        loadQuestion(next);
    }


    // ################
    // ## Chargement scene atelier
    // ################
    public void LoadAtelier()
    {
        Debug.Log("Chargement atelier");
        SceneManager.LoadScene("Scenes/SceneAtelier", LoadSceneMode.Single);
    }

    public void loadQuestion(string next)
    {
        if (next == "End")//je quitte le questionnaire pour retourner au menu
        {
            LoadAtelier();
        }
        else
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml_questions);
            //Je récupère la question ou l'info
            XmlNode question = xmlDoc.SelectSingleNode("/Questions/Question[@name='" + next + "']");
        
        //Je m'occupe des boutons
        Questionnaire questionnaire = this.GetComponent<Questionnaire>();
            if (saveQuestion.Count >= 2)
            {
                questionnaire.back.interactable = true;
            }
            else
            {
                questionnaire.back.interactable = false;
            }
        questionnaire.ClearChoices();
        foreach (XmlNode choice in question.SelectSingleNode("Choices"))
        {
            questionnaire.choice_text.Add(MedicalAppManager.Instance().setUserDataInText(choice.InnerXml));
            questionnaire.choice.Add(choice.Attributes["name"].Value);
        }
        questionnaire.SetChoices();
        //texte
        questionnaire.text.text = MedicalAppManager.Instance().setUserDataInText(question.SelectSingleNode("Text").InnerXml);
            //démo éventuelle
            if (question.Attributes["type"].Value == "Demo")
            {
                Debug.Log(question.SelectSingleNode("Demo").InnerXml);
                 questionnaire.AddDemo(question.SelectSingleNode("Demo").InnerXml);
            }
        }
    }
}
