using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // 1

public class DisplayKeyboard : MonoBehaviour, IPointerClickHandler
{

    private VirtualKeyboard keytest;
    private bool IsOpen = false;
    
	// Use this for initialization
	void Start () {
		keytest = new VirtualKeyboard();
    }

    public void OnPointerClick(PointerEventData eventData) // 3
    {
        OpenKeyboard();
        IsOpen = true;
    }

    // Update is called once per frame
    void Update () {
        //bool State = GetComponent<InputField>().isFocused;

        //if (State)
        //{
        //    if (!IsOpen)
        //    {
        //        OpenKeyboard();
        //        IsOpen = true;
        //    }
        //}
        //else
        if(!GetComponent<InputField>().isFocused && IsOpen)
        {
                CloseKeyboard();
                IsOpen = false;
        }
    }
    
    public void OpenKeyboard()
    {
        keytest.ShowTouchKeyboard();
    }
    public void CloseKeyboard()
    {
        keytest.HideTouchKeyboard();
    }
}
