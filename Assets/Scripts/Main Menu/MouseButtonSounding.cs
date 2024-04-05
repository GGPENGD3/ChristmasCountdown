using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseButtonSounding : MonoBehaviour, IPointerEnterHandler
{
    private Button btn;

	public void OnPointerEnter(PointerEventData eventData)
	{
		((IPointerEnterHandler)btn).OnPointerEnter(eventData);
        FindObjectOfType<AudioManager>().Play("ui", "shift");
    }

	// Start is called before the first frame update
	void Start()
    {
        btn = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
