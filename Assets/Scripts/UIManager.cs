using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Dropdown DateDropdown; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public IEnumerator InitializeUI()
    {
        DateDropdown.ClearOptions();
        DateDropdown.AddOptions(GameManager.Instance.Dates);
        yield return null; 
    }



    public void ChangeDate(int dateId)
    {
       GameManager.Instance.CurrentDate =  DateDropdown.options[dateId].text; 
    }
}
