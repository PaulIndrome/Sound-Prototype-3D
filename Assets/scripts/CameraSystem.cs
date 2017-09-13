using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour {

    private BoxCollider boxCol;
    private Rigidbody rigBody;

    private Camera mainCam;
    private GameObject player;
    private Vector3 oldPlayerPos;
    private int lastMovingDirection = 0;
    public bool hasToMove = true;
    public Vector3 offsets = new Vector3(1, 0, 0);

    public float targetFoV;
    public float interpVelocity;
    public Vector3 targetPos;
    public Vector3 eliminateZ = new Vector3(1, 1, 0);

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        oldPlayerPos = player.transform.position;
        mainCam = Camera.main;
        boxCol = GetComponent<BoxCollider>();
        rigBody = GetComponent<Rigidbody>();
    }
	
	
	void Update () {
        Vector3 currentPlayerPos = player.transform.position;
        currentPlayerPos.y += 1;
        
        changeFieldOfView(targetFoV);
    
       if ((currentPlayerPos - oldPlayerPos).magnitude > 1 && !hasToMove)
        {
            hasToMove = true;
            targetFoV = 90;
        }
            

       if (player && hasToMove)
        {
            Vector3 targetDirection = currentPlayerPos - transform.position;
            targetDirection.Scale(eliminateZ);

            if (targetDirection.y > 2f || targetDirection.y < -2f)
            {
               interpVelocity = targetDirection.magnitude * 33f;
            } else
            {
                interpVelocity = targetDirection.magnitude * 12.5f;
            }

            targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);

            if ((targetPos.x > transform.position.x && lastMovingDirection == -1) || (targetPos.x < transform.position.x && lastMovingDirection == 1) || lastMovingDirection == 0)
            {
                transform.position = Vector3.Lerp(transform.position, targetPos, 0.25f);
                lastMovingDirection = 0;
            }
                

            if (interpVelocity < 0.5f)
            {
                targetFoV = 60;
                hasToMove = false;
                oldPlayerPos = currentPlayerPos;
            }
        }

    }

    void changeFieldOfView(float target)
    {
        float currentFOV = mainCam.fieldOfView;
        mainCam.fieldOfView = Mathf.Lerp(currentFOV, target, (currentFOV <= target) ? 1.75f * Time.deltaTime : 0.75f * Time.deltaTime);
    }

    void OnCollisionEnter(Collision col)
    {
        lastMovingDirection = (Input.GetAxis("Horizontal") > 0) ? 1 : -1;
        hasToMove = false;
        //Debug.Log("Camera rigidbody has collided. Last moving direction was " + lastMovingDirection);
    }


}
