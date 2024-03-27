using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    public MonsterAI monster;
    public Vector3 currentPosition;

    // Start is called before the first frame update
    void Start()
    {
        monster = GameObject.Find("Evilfurby").GetComponent<MonsterAI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            PingMonster();
        }
    }

    public void PingMonster()
    {
        currentPosition = transform.position;

        if (Vector3.Distance(monster.gameObject.transform.position, currentPosition) >= monster.soundDetectionRange)
        {
            Debug.Log("ai heard object " + this.name);
            monster.investigatePoint = currentPosition;
            monster.investigating = true;
        }
    }
}
