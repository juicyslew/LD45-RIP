using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive_Projectile : MonoBehaviour
{
    float grav;
    GameController gc;
    Vector2 vel;
    int state = 0; //0 - in air; 1 - stuck to object;
    float explosiveInterval = .2f;
    float explosiveTimer = 0f;
    public GameObject Explosion;
    float spin;
    Color origCol;
    public Color explodeCol;
    SpriteRenderer sprend;
    public AudioSource explodeSnd;

    void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        grav = gc.grav;
        spin = Random.Range(360, -360);
        sprend = GetComponent<SpriteRenderer>();
        origCol = sprend.color;
    }
    void Update()
    {
        explosiveTimer += Time.deltaTime;
        if (explosiveTimer > explosiveInterval)
        {
            sprend.color = explodeCol;
        }

        switch (state)
        {
            
            case 0:
                transform.Rotate(0f, 0f, spin*Time.deltaTime);
                float accy = -grav;
                CollisionCheck();

                vel = new Vector2(vel.x, vel.y + accy * Time.deltaTime);
                transform.position = (Vector2)transform.position + vel * Time.deltaTime;
                break;
            case 1:
                break;
            default:
                break;
        }
    }

    void CollisionCheck()
    {
        RaycastHit2D[] hitlist = Physics2D.RaycastAll(transform.position, vel, vel.magnitude * Time.deltaTime);
        Debug.DrawRay(transform.position, vel * Time.deltaTime, Color.cyan, 1f);
        foreach (RaycastHit2D hit in hitlist) {
            if (hit)
            {
                switch (hit.collider.gameObject.tag)
                {
                    case "Terrain":
                    case "Destructibles":
                        if (hit)
                        {
                            transform.position = new Vector3(hit.point.x, hit.point.y, transform.position.z);
                            state = 1;
                        }
                        break;
                    case "Spike":
                    case "Explosion":
                    default:
                        break;
                }
            }
        }
        
    }
    void CollisionControl(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Terrain":
            case "Destructibles":
            case "Spike":
                break;
            case "Explosion":
                Vector2 diff = transform.position - other.transform.position;
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

    public bool Explode()
    {
        if (explosiveTimer > explosiveInterval)
        {
            Instantiate(Explosion, transform.position, transform.rotation);
            Instantiate(explodeSnd.gameObject, transform.position, transform.rotation);
            Destroy(gameObject);
            Debug.Log("Heya");
            return true;
        }
        else
        {
            return false;
        }
    }

    public void setVel(Vector2 newvel)
    {
        vel = newvel;
    }
}
