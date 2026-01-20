using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEditor.UI;
using UnityEngine;

public class StageChangeScript : MonoBehaviour
{
    [SerializeField] 
     TextMeshProUGUI stageText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
        
    }
    public void StageUpdate(string stageName)
    {
        if (stageText != null)
        {
            stageText.text = stageName;
        }
        this.gameObject.SetActive(true);
    }
}
