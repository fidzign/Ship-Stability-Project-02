using UnityEngine;
using System.Collections;

public class onClick5 : MonoBehaviour {

    public GameObject[] chart;



    void Start()
    {

        chart[0].active = false;
        chart[1].active = false;
        chart[2].active = false;
        chart[3].active = false;
        chart[4].active = false;

    }

    void Disable()
    {
        chart[0].active = false;
        chart[1].active = false;
        chart[2].active = false;
        chart[3].active = false;
    }

	// Use this for initialization
    void OnClick()
    {

        Disable();
        chart[4].active = !chart[4].active;


    }
}
