using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameUI : MonoBehaviour
{
    [SerializeField]
    GameObject UI;

    // Start is called before the first frame update
    void Start()
    {
        if (!SaveData.HasName()){ 
        UI.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CloseUI() {
        UI.SetActive(false);
    }
}
