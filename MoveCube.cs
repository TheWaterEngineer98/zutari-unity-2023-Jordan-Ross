using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCube : MonoBehaviour
{
    private Rigidbody rb;
    private float horizontalInput;
    private float verticalInput;
    private Vector3 movePosition;// this will change when WASD input is given
    private Renderer rend;// call renderer to change rigidbody material when new direction is used
  [SerializeField] private Material red;// [SeriealizeField] allows for external input as to colour to change both during and before initializing build
  [SerializeField] private Material green;
  [SerializeField] private Material orange;
  [SerializeField] private Material blue;
  [SerializeField] private Material black;
  [SerializeField] private float speed;// Player can input cube speed here
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();// object rb is associated to the rigidbody that this script is attached to
         rend = GetComponent<Renderer>();
         rend.enabled = true;
         rend.material = black;// cube will first appear black
    }
    
    void FixedUpdate() // FixedUpdate() gives the advantage to Update as it is called 100x per second whereas Update() calls script every frame 
    {

    float horizontalInput = Input.GetAxis("Horizontal");// built into unity , associated with WASD and up-down-left right keys. It is easier to use these than the Input.GetKey methods although both could be used

    float verticalInput = Input.GetAxis("Vertical");

    transform.Translate(new Vector3(horizontalInput,verticalInput,0)*speed*Time.fixedDeltaTime);// Multiply movement on action with user inputted speed and multiplied by Time.fixedDeltaTime. This does the same job as Time.deltaTime but used when in FixedUpdate.
       if(horizontalInput<0)
         rend.material = red;// cube is red when translating to left 
         if(horizontalInput>0)
         rend.material = blue;// cube is blue translating going to right 
         if(verticalInput<0)
         rend.material=orange;// cube is orange when transalting down
         if(verticalInput>0)
         rend.material=green;//cube is green when translating up
        
        //From main camera view the borders by whuch the cube is out of sight are (-14;14) in the x-direction and (-7;7) in the y direction. Therefore when the cube crosses these threshholds, the following code snaps the respective x or y positions to the other side of the players view. 
        if( transform.position.x <-14)
        transform.position = new Vector3 (14,transform.position.y, 0);
        if( transform.position.x >14)
        transform.position = new Vector3 (-14,transform.position.y, 0);
        if( transform.position.y <-7)
        transform.position = new Vector3 (transform.position.x,7, 0);
        if( transform.position.y >7)
        transform.position = new Vector3 (transform.position.x,-7, 0);
     }

}
