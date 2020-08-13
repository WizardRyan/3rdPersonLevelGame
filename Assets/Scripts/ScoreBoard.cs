using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class ScoreBoard : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI standardScore;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        standardScore.text = PlayerController.standardScore.ToString();
    }
}
