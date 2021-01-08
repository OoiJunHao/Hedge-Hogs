using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnCollision : MonoBehaviour
{
    public int Rand_Value;
    public int Player_Turn;
    public bool new_Random;

    // Start is called before the first frame update
    void Start()
    {
        new_Random = false;
        Player_Turn = 1;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject obj in players)
            {
                PlayerMovement script = obj.GetComponent<PlayerMovement>();

                if (script.player_moving)
                {
                    print("Player Moving. Cannot rolled dice");
                    return;
                }
            }

            Rand_Value = Random.Range(1, 6);

            GameObject text = GameObject.Find("Dice_Text");

            Text text_script = text.GetComponent<Text>();

            text_script.text = "Dice rolled: " + Rand_Value.ToString();

            new_Random = true;
        }
    }
}
