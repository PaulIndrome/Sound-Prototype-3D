using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCC : MonoBehaviour
{

    public float speed;
    public float jumpSpeed;
    public float jumpGravityScale;
    [Range(-1,-20)]
    public float characterGravity;

    CharacterController controller;
    Animator anim;
    playerSoundScript plSoScr;
    float yVelocity;
    Vector3 velocity;
    

    // Use this for initialization
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        plSoScr = GetComponentInChildren<playerSoundScript>();
    }

    // Update is called once per frame
    void Update()
    {
        yVelocity += characterGravity * Time.deltaTime * (Input.GetButton("Jump") && yVelocity > 0 ? jumpGravityScale : 1);

        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            yVelocity = jumpSpeed;
            anim.SetTrigger("jumpTrig");
        }
        velocity = Vector3.right * Input.GetAxis("Horizontal") * speed;
        velocity.y = yVelocity;

        Debug.DrawRay(transform.position, velocity, Color.red, 0.0f, false);

        anim.SetFloat("InputX", velocity.x*2);
        checkDirection(velocity.x);
        controller.Move(velocity * Time.deltaTime);

        anim.SetBool("isGrounded", controller.isGrounded);
        if (controller.isGrounded)
        {
            yVelocity = 0;
        }
            

        transform.position -= Vector3.forward * transform.position.z;
    }
    

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.rigidbody)
        {
            hit.rigidbody.velocity = controller.velocity;
        }
    }

    void OnDrawGizmosSelected()
    {
        if(Input.GetButton("Jump"))
            Gizmos.DrawSphere(transform.position, 0.25f);
    }

    private void checkDirection(float x)
    {
        if (x < -0.1)
            transform.forward = Vector3.Lerp(transform.forward, new Vector3(-1, 0, 0), 0.45f);
        else if (x > 0.1)
            transform.forward = Vector3.Lerp(transform.forward, new Vector3(1, 0, 0), 0.45f);
    }

    void playStepSound(int s)
    {
        if(velocity.magnitude > 0.1f && controller.isGrounded)
            plSoScr.evaluateAnimationEvent(s);
    }

}
