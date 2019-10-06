using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    Animator animator;
    movement_controller mc;
    SpriteRenderer sprend;
    float randInterval = 1f;
    float randTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        mc = GetComponentInParent<movement_controller>();
        sprend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        randTimer += Time.deltaTime;
        if (randTimer > randInterval)
        {
            randTimer = 0f;
            animator.SetBool("UncommonIdle", Random.value > .9f);
        }
        sprend.flipX = mc.GetVel().x > 0f ? false : true;
        animator.SetBool("IsRunning", Mathf.Abs(mc.GetVel().x) / mc.GetWalkspd() > .2f);
        animator.SetBool("IsGrounded", !mc.GetGrounded());
    }
}
