using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsScreen : MonoBehaviour
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
    }
}
