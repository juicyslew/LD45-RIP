using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;
public class movement_controller : MonoBehaviour {

    float grav;
	Vector2 move;
	Vector2 vel;
	float walkspd = 6f;
    float accspd = 40f;
    float jumpstr = 7f;
    bool canjump = false;
    Vector3 play_center;
    float halfheight;
    float halfwidth;
    LayerMask laymask;
    GameController gc;
    bool grounded = false;
    float airAcc = .3f;
    float groundAcc = .8f;
    float SpawnWait = .5f;
    float SpawnTimer = 0f;

    float termVel = 16f;

    // Use this for initialization
    void Start () {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        grav = gc.grav;
        laymask = ~gameObject.layer;
		vel = new Vector2 (0f, 0f);
        halfwidth = GetComponent<BoxCollider2D>().size.x / 2f;
        halfheight = GetComponent<BoxCollider2D>().size.y / 2f;
        play_center = transform.position + new Vector3(0f, halfheight, 0f);
    }
	
    int PseudoSign(float n, float eps = 1e-1f)
    {
        if (n < -eps){
            return -1;
        }else if (n > eps)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
	// Update is called once per frame
	void Update () {
        if (SpawnTimer < SpawnWait)
        {
            SpawnTimer += Time.deltaTime;
            return;
        }

		move = new Vector2 (Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		float intendmovex = move.x * accspd;
		float accy = -grav;
        float accx = 0f;

        
        if (move.x == Mathf.Sign(vel.x))
        {
            if (walkspd < Mathf.Abs(vel.x))
            {
                accx = 0;
            }else
            {
                accx = intendmovex*Time.deltaTime;
            }
        }
        else 
        {
            accx = -Mathf.Sign(vel.x) * accspd * Time.deltaTime;
            if (Mathf.Abs(vel.x) < Mathf.Abs(accx))
            {
                accx = -vel.x;
            }
            
            //Debug.Log(accx);
            if (move.x == -Mathf.Sign(vel.x)){
                accx += intendmovex * Time.deltaTime;
            }
        }
        grounded = false;
        CollisionCheck(5, 2, .05f);
        accx *= grounded ? groundAcc : airAcc;
        if (canjump && move.y==1){
            vel = new Vector2(vel.x, jumpstr);
        }


		vel = new Vector2 (vel.x + accx, Mathf.Max(vel.y + accy * Time.deltaTime, -termVel));
		transform.position = (Vector2)transform.position + vel * Time.deltaTime;
        
        play_center = transform.position + new Vector3(0f, halfheight, 0f);
        canjump = false;

	}

    void CollisionCheck(int checkresohori, int checkresovert, float skinwidth)
    {
        halfheight = GetComponent<BoxCollider2D>().size.y / 2f;
        halfwidth = GetComponent<BoxCollider2D>().size.x / 2f;
        float diry = vel.y >= 0 ? 1f : -1f;
        float dirx = vel.x >= 0 ? 1f : -1f;

        float hori_interval = (2f * halfheight) / checkresohori;
        Vector2 firstpos = (Vector2)play_center - new Vector2(0f, halfheight - hori_interval / 2f);
        // Horizontal Collision
        for (int j = 0; j < checkresohori; j++)
        {
            Vector2 checkori = firstpos + new Vector2(0f, j * hori_interval);
            RaycastHit2D[] hitx = Physics2D.RaycastAll(play_center, new Vector2(dirx, 0f), skinwidth + halfwidth, laymask);
            Debug.DrawRay(checkori, (skinwidth + halfwidth) * new Vector3(dirx, 0f, 0f), Color.green);

            if (hitx.Length > 0)
            {
                CollisionControl(hitx, dirx, true);
            }
        }


        float vert_interval = (2f * halfwidth) / checkresovert;
        firstpos = (Vector2)play_center - new Vector2(halfwidth - vert_interval/2f, 0f);

        for (int i = 0; i < checkresovert; i++)
        {
            Vector2 checkori = firstpos + new Vector2(i * vert_interval, 0f);
            RaycastHit2D[] hity = Physics2D.RaycastAll(checkori, new Vector2(0f, diry), halfheight + skinwidth, laymask);
            Debug.DrawRay(checkori, (skinwidth + halfheight) * new Vector3(0f, diry, 0f), Color.red);
            if (hity.Length>0)
            {
                CollisionControl(hity, diry, false);
            }
        }
        
    }


    void CollisionControl(RaycastHit2D[] hitlist, float dir, bool hori)
    {
        foreach (RaycastHit2D hit in hitlist)
        {
            switch (hit.collider.gameObject.tag)
            {
                case "Terrain":
                case "Destructibles":
                    if (hori)
                    {
                        vel = new Vector2(0f, vel.y);
                        transform.position = new Vector3(hit.point.x - dir * halfwidth, transform.position.y, transform.position.z);
                    }
                    else
                    {
                        vel = new Vector2(vel.x, 0f);
                        if (dir == -1f)
                        {
                            transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
                            canjump = true;
                            grounded = true;
                        }
                        else
                        {
                            transform.position = new Vector3(transform.position.x, hit.point.y - halfheight * 2, transform.position.z);
                        }
                    }
                    break;
                case "Spike":
                    Death();
                    break;
                case "Returner":
                    Capture();
                    break;
                default:
                    break;
            }
        }
    }
    void CollisionControl(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Terrain":
            case "Destructibles":
                break;
            case "Spike":
                Death();
                break;
            case "Returner":
                Capture();
                break;
            case "Explosion":
                Vector2 diff = play_center - other.transform.position;
                float force = other.GetComponent<ExplosionProperties>().getForce(diff.magnitude);
                Vector2 addvel = diff.normalized * force;
                if (addvel.y > 0) { vel.y = Mathf.Max(vel.y, 0); } 
                vel += addvel;
                break;
            default:
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        CollisionControl(other);
    }

    void Death()
    {
        Debug.Log("Death");
        gc.EndLevel();
    }

    void Capture()
    {
        Debug.Log("Captured!");
        gc.Captured();
    }

    public Vector2 GetVel()
    {
        return vel;
    }
    public float GetWalkspd()
    {
        return walkspd;
    }
    public bool GetGrounded()
    {
        return grounded;
    }
    public Vector3 GetCenter()
    {
        return play_center;
    }
}
