using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw_Controller : MonoBehaviour
{
    public float throwstrength = 8f;
    public float throwCoolInterval = 0f;
    float throwCoolTimer = 0f;
    public float throwMaxInterval = .4f;
    float throwMaxTimer = 0f;
    float minStrength = 8f;
    int state = 0; //0 - not throw, 1 - preping, 2 - cooling
    public GameObject Projectile;
    List<GameObject> projList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case 0:
                if (Input.GetMouseButton(0)) state = 1;
                break;
            case 1:
                if (!Input.GetMouseButton(0)) { state = 2; Throw(); throwMaxTimer = 0f; }
                throwMaxTimer += Time.deltaTime;
                break;
            case 2:
                throwCoolTimer += Time.deltaTime;

                if (throwCoolTimer > throwCoolInterval) { state = 0; throwCoolTimer = 0f; }

                break;
            default:
                break;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ExplodeNext();
        }
        
    }

    void ExplodeNext()
    {
        if (projList.Count > 0)
        {
            if (projList[0].GetComponent<Explosive_Projectile>().Explode())
            {
                projList.RemoveAt(0);
            }
        }
    }

    void Throw()
    {
        float strength = throwstrength* Mathf.Min(throwMaxTimer,throwMaxInterval) / throwMaxInterval + minStrength;
        GameObject myproj = Instantiate(Projectile, transform.position, transform.rotation);
        Vector2 mousedir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        myproj.GetComponent<Explosive_Projectile>().setVel(strength*(mousedir.normalized));
        projList.Add(myproj);
    }
}
