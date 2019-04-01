using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Questionnaire : MonoBehaviour {
    public Button back;
    public GameObject choices_group;
    public GameObject button_prefab;
    public Text text;
    public Transform demo;
    bool isDemo;
    public List<string> choice_text;
    public List<string> choice;
    public List<GameObject> choice_buttons;

    // Use this for initialization
    void Start () {
        isDemo = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddDemo(string meshName)
    {
        Instantiate(Resources.Load<GameObject>("OtherModels/" + meshName), demo);
        isDemo = true;
    }

    public void ClearChoices()//and delete old demo
    {
        choice_text.Clear();
        choice.Clear();

        for (int i = 0; i < choice_buttons.Count; ++i)
        {
            Destroy(choice_buttons[i]);
        }
        choice_buttons.Clear();
        if (isDemo)
        {
            Destroy(demo.GetChild(0).gameObject);
            isDemo = false;
        }
    }

    public void SetChoices()
    {
        for (int i = 0; i < choice.Count; ++i)
        {
            GameObject temp_button = Instantiate(button_prefab, choices_group.transform);

            //positionnement
            temp_button.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, temp_button.GetComponent<RectTransform>().sizeDelta.y);//TODO régler la taille en fonction du nombre de ligne
            temp_button.GetComponent<RectTransform>().position = new Vector3(Screen.width / 2, (i + 0.5f) * temp_button.GetComponent<RectTransform>().sizeDelta.y);
            //ajout données nécessaires au bon fonctionnement
            string toLoad = choice[i];
            temp_button.GetComponent<Button>().onClick.AddListener(() => this.GetComponent<LoadXMLQuestions>().loadAndSaveQuestion(toLoad));

            temp_button.transform.GetChild(0).transform.GetComponent<Text>().text = choice_text[i];
            //ajout à la liste des boutons
            choice_buttons.Add(temp_button);
        }
    }
}
