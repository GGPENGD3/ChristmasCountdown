using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseButtonSounding : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void OnMouseOver()
	{
        FindObjectOfType<AudioManager>().Play("ui ", "ui_Shift");
    }
}
