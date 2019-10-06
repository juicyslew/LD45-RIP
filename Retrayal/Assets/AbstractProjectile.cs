using UnityEngine;

public class ExplosiveProjectile : MonoBehaviour
{
    float grav;
    GameController gc;
    Vector2 vel;
    int state = 0; //0 - in air; 1 - stuck to object;
    GameObject Explosion;

    public float explosiveInterval;
    float explosiveTimer = 0f;

    void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        grav = gc.grav;
    }
    void Update()
    {
        switch (state)
        {
            case 0:
                float accy = -grav;
                CollisionCheck();

                vel = new Vector2(vel.x, vel.y + accy * Time.deltaTime);
                transform.position = (Vector2)transform.position + vel * Time.deltaTime;
                break;
            case 1:
                explosiveTimer += Time.deltaTime;
                if (explosiveTimer > explosiveInterval)
                {
                    Explode();
                }
                break;
            default:
                break;
        }
    }

    void CollisionCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, vel, vel.magnitude);
        if (hit)
        {
            transform.position = new Vector3(hit.point.x, hit.point.y, transform.position.z);
            state = 1;
        }
    }
    
    void Explode()
    {
        Instantiate(Explosion);
    }

    void setVel(Vector2 newvel)
    {
        vel = newvel;
    }
}
