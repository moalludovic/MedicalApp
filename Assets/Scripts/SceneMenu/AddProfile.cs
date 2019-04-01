using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddProfile : MonoBehaviour {
    //data à récupére
    public Text userName;
    public Text userLastName;
    public Text userOperation;
    public Text userGender;
    //affichage
    public Text nameMenuDisplay;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void validateProfile(){
        MedicalAppManager.Instance().SetProfile(userName.text, userLastName.text, userOperation.text, userGender.text);
        nameMenuDisplay.text = "Bonjour "+userName.text;
    }
}
