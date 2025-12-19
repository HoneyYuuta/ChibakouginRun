using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsUI : MonoBehaviour
{
    [SerializeField]
    GameObject UI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPressed()
    {
        UI.SetActive(true);
        this.gameObject.SetActive(false);
        Time.timeScale = (Time.timeScale == 0) ? 1 : 0;
    }
}
