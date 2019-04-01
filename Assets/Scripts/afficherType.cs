using UnityEngine;
using UnityEngine.UI;

public class afficherType : MonoBehaviour {

    public string typeName;
    private Button btn;

	void Start () {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }
	
	private void TaskOnClick()
    {
        // affichage aliments en fonction de leur type
        AtelierManager.Instance().showAlimentType(typeName);
    }
}
