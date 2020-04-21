using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fight : MonoBehaviour
{
    public KeyCode forward;
    public KeyCode back;

    public KeyCode attack1;
    public KeyCode attack2;
    public KeyCode attack3;
    public KeyCode attack4;

    private Rigidbody rb;
    private Animator anim;

    private int life;

    private Fight player1;
    private Fight player2;

    private Slider lifeBar1;
    private Slider lifeBar2;

    private bool canMove;
    private bool canHit;

    private Fight target;
    private bool contactPlayers;
    private int damage;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        life = 10;
        damage = 1;
        contactPlayers = false;
        anim.SetBool("canMove", true);
        canMove = anim.GetBool("canMove");
        canHit = true;

        player1 = GameObject.FindWithTag("Player1").GetComponent<Fight>();
        player2 = GameObject.FindWithTag("Player2").GetComponent<Fight>();

        lifeBar1 = GameObject.FindWithTag("Player1LifeBar").GetComponent<Slider>();
        lifeBar2 = GameObject.FindWithTag("Player2LifeBar").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        // Know in real time the value of canMove parameter of Animator
        canMove = anim.GetBool("canMove");

        // User is alive
        if (life > 0)
        {
            // Key forward pressed
            if (Input.GetKey(forward) && canMove == true)
            {
                if (canMove == true)
                {
                    MoveForward();
                }
            }
            // Key back pressed
            else if (Input.GetKey(back) && canMove == true)
            {
                if (canMove == true)
                {
                    MoveBackward();
                }
            }
            // Neither pressed
            else
            {
                rb.velocity = new Vector3(0, 0, 0);
                anim.SetBool("forward", false);
                anim.SetBool("back", false);
            }

            // If Attack keys pressed
            if (Input.GetKey(attack1) && (canHit == true))
            {
                StartCoroutine(Attack("atk1"));
            }
            else if (Input.GetKey(attack2) && (canHit == true))
            {
                StartCoroutine(Attack("atk2"));
                this.damage = 2;
            }
            else if (Input.GetKey(attack3) && (canHit == true))
            {
                StartCoroutine(Attack("atk3"));
            }
            else if (Input.GetKey(attack4) && (canHit == true))
            {
                StartCoroutine(Attack("atk4"));
                this.damage = 2;
            }
        }

        // User is dead
        else
        {
            anim.SetBool("canMove", false);
            anim.SetBool("forward", false);
            anim.SetBool("back", false);
            anim.SetBool("dead", true);
        }
    }

    // Check if 2 users have contact
    private void CheckContact()
    {
        if (player2.transform.position.z - player1.transform.position.z <= 1.5) {
            this.contactPlayers = true;

            // get the target player
            if(this.tag == player1.tag)
            {
                this.target = player2;
            } else
            {
                this.target = player1;
            }
        } else
        {
            this.contactPlayers = false;
        }
    }

    // Old method to get target with collision. I changed by my CheckContact method 
    // When direct contact between 2 users objects with collision method
    private void OnCollisionEnter(Collision collision)
    {
        // this.target = collision.gameObject.GetComponent<Fight>();
    }

    // When separation between users objects
    private void OnCollisionExit(Collision collision)
    {
        // this.target = null;
    }

    // Moving forward
    private void MoveForward()
    {
        if (this.name == player1.tag)
        {
            rb.velocity = new Vector3(0, 0, 3);
        }
        else if (this.name == player2.tag)
        {
            rb.velocity = new Vector3(0, 0, -3);
        }

        anim.SetBool("forward", true);
        anim.SetBool("back", false);
    }

    // Moving backward
    private void MoveBackward()
    {
        if (this.name == player1.tag)
        {
            rb.velocity = new Vector3(0, 0, -3);
        }
        else if (this.name == player2.tag)
        {
            rb.velocity = new Vector3(0, 0, 3);
        }

        anim.SetBool("forward", false);
        anim.SetBool("back", true);
    }

    // When user attack target
    public void Hit()
    {
        CheckContact();

        // Target exist and is alive
        if ((this.contactPlayers == true) && (this.target.life > 0))
            {
            this.target.life -= this.damage;

            if (this.target.tag == "Player1")
            {
                lifeBar1.value = this.target.life;
            }
            else if (this.target.tag == "Player2")
            {
                lifeBar2.value = this.target.life;
            }

            Debug.Log(this.target.name + " has " + this.target.GetComponent<Fight>().life + " points of life.");

            this.damage = 1;
        }
    }

    // Attack process to call in coroutine
    IEnumerator Attack(string atk)
    {
        canHit = false;
        anim.SetBool("canMove", false);

        anim.SetBool("atk1", false);
        anim.SetBool("atk2", false);
        anim.SetBool("atk3", false);
        anim.SetBool("atk4", false);
        anim.SetBool(atk, true);

        // To wait for the end of the animation
        yield return new WaitForSeconds(1.2f);

        // Then execution of
        anim.SetBool(atk, false);
        anim.SetBool("canMove", true);
        canHit = true;
    }
}

