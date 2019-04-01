using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PassQuestions : MonoBehaviour {
    // ################
    // ## Chargement scene atelier
    // ################
    public void LoadAtelier()
    {
        Debug.Log("Chargement atelier");
        MedicalAppManager.Instance().OnMeal = false;
        SceneManager.LoadScene("Scenes/SceneAtelier", LoadSceneMode.Single);
    }
}
