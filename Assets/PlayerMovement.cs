using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    int player_tile;
    Vector2 player_grid_position;

    int tiles_to_move;
    Vector2 next_position;

    bool moving_on_snake_ladder;

    public bool player_moving;
    public int player_id;

    // Start is called before the first frame update
    void Start()
    {
        player_tile = -1;

        player_grid_position.x = -1;
        player_grid_position.y = 0;

        player_moving = false;
        tiles_to_move = 0;

        moving_on_snake_ladder = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Set the direction at the start
        if (player_grid_position.y % 2 == 1)
        {
            transform.localScale = new Vector3(-20, 20, 0); 
        }
        else
        {
            transform.localScale = new Vector3(20, 20, 0);
        }

        // Check if the player is not moving
        if (player_moving == false)
        {
            GameObject dice = GameObject.Find("Dice");

            OnCollision script = dice.GetComponent<OnCollision>();

            if (script.new_Random == false || player_tile == 99 || script.Player_Turn != player_id)
            {
                return;
            }

            script.new_Random = false;
            tiles_to_move = script.Rand_Value;
            player_moving = true;

            SetNextPosition();
        }

        if (transform.position.x != next_position.x || transform.position.y != next_position.y)
        {
            Vector2 diff;
            diff.x = next_position.x - transform.position.x;
            diff.y = next_position.y - transform.position.y;

            // Force update the direction based on movement
            if (diff.x < 0)
            {
                transform.localScale = new Vector3(-20, 20, 0);
            }
            else
            {
                transform.localScale = new Vector3(20, 20, 0);
            }

            if (diff.magnitude < 4)
            {
                transform.position = next_position;
            }
            else
            {
                diff.Normalize();
                Vector3 add = diff * 2;
                transform.position += add;
            }
        }
        else
        {
            if (moving_on_snake_ladder)
            {
                moving_on_snake_ladder = false;
                player_moving = false;

                SwapPlayer();
            }
            else
            {
                --tiles_to_move;
                ++player_tile;

                if (tiles_to_move != 0 && player_tile < 99)
                {
                    SetNextPosition();
                }
                else
                {
                    Check_LadderSnake();
                }
            }
        }
    }

    void SetNextPosition()
    {
        next_position = transform.position;

        // Set the next position
        if (player_grid_position.y % 2 == 0)
        {
            // Means it's on a row to move right
            // Check if it is on the right most tile
            if (player_grid_position.x == 9)
            {
                next_position.y += 100;
                ++player_grid_position.y;
            }
            else
            {
                next_position.x += 100;
                ++player_grid_position.x;
            }
        }
        else
        {
            // Check if it is on the left most tile
            if (player_grid_position.x == 0)
            {
                next_position.y += 100;
                ++player_grid_position.y;
            }
            else
            {
                next_position.x -= 100;
                --player_grid_position.x;
            }
        }
    }

    void Check_LadderSnake()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Snake_Ladder");

        foreach(GameObject obj in gameObjects)
        {
            Ladder_Snake script = obj.GetComponent<Ladder_Snake>();

            if (script.start_tile == player_tile)
            {
                if (script.is_ladder)
                {
                    print("Encountered a ladder!! :)");
                }
                else
                {
                    print("Encountered a snake!! :(");
                }      

                // Set the new player position
                player_grid_position = script.end_grid_position;
                player_tile = script.end_tile;

                // Set the next position to go to
                next_position.x = transform.position.x + script.end_vector_direction.x * 100;
                next_position.y = transform.position.y + script.end_vector_direction.y * 100;

                moving_on_snake_ladder = true;

                return;
            }
        }

        // This means that it did not encounter any ladder or snake on it's tile
        player_moving = false;

        SwapPlayer();
    }

    void SwapPlayer()
    {
        GameObject dice = GameObject.Find("Dice");

        OnCollision script = dice.GetComponent<OnCollision>();

        // Switch Player
        script.Player_Turn = (script.Player_Turn == 1 ? 2 : 1);

        GameObject text = GameObject.Find("Player_Turn_Text");

        Text text_script = text.GetComponent<Text>();

        if (script.Player_Turn == 1)
        {
            text_script.text = "Player's Turn: Pink";
        }
        else
        {
            text_script.text = "Player's Turn: Blue";
        }
    }
}
