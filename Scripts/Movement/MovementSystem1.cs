using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* With this movement system, the left hand will be to move the user absolutely around the space, based on how far the left hand
* is from the center (of the play area), and the right hand will be to move the player within the play area more in a fine tuned
* manner.
**/
public class MovementSystem1 : MonoBehaviour
{
    private Transform cameraRig;

    private Transform leftHand;
    private Transform rightHand;

    private Transform mainCamera;

    private Transform trackingSpace;

    private Transform origin;

    private Transform locator;
    // Start is called before the first frame update
    void Start()
    {
        // what do I need?
        // the left and right hand game objects
        leftHand = GameObject.FindGameObjectWithTag("leftHand").transform;
        rightHand = GameObject.FindGameObjectWithTag("rightHand").transform;
        // the camera rig
        cameraRig = GameObject.FindGameObjectWithTag("cameraRig").transform;
        // the tracking space...
        trackingSpace = GameObject.FindGameObjectWithTag("trackingSpace").transform;
        // the head object itself
        mainCamera = GameObject.FindGameObjectsWithTag("MainCamera")[1].transform;
        // the origin
        origin = GameObject.FindGameObjectWithTag("origin").transform;
        // the locator
        locator = GameObject.FindGameObjectWithTag("locator").transform;
        
    }

    /**
    * a function to scale the abs position as you get further away from the origin
    **/
    private float AbsWeightFunction(float value) {
        return 40 * Mathf.Pow(value,3);
    }   

    /**
    * a function to scale the rel position as you get further away from the origin
    **/
    private float RelWeightFunction(float value) {
        return 2 * Mathf.Pow(value,3);
    }

    private Vector3 Round(Vector3 v, int d) {
        float multiplier = 1f;
        for(int i = 0; i < d; i++) {
            multiplier *= 10f;
        }
        return new Vector3(Mathf.Round(v.x * multiplier) / multiplier,
                            Mathf.Round(v.y * multiplier) / multiplier,
                            Mathf.Round(v.z * multiplier) / multiplier);
    }

    private void MoveAbsolute() {

        // what I need to move absolutely:
            // the position I am in relative to the center of the main camera
            Vector3 pos_rel_to_main_camera = Round(locator.InverseTransformPoint(leftHand.position), 6);
            Vector3 pato = origin.TransformPoint(pos_rel_to_main_camera);
            // maybe multiply by natural log of x y z to get larger movement...
            Vector3 pos_abs_to_origin = new Vector3(AbsWeightFunction(pato.x), AbsWeightFunction(pato.y), AbsWeightFunction(pato.z));
            // map that point to the point where it should be large scale, super nova space.
            // may need boundary checking using ovrBoundary
            // maybe clip the vector!
            // move the camera rig to the new location...
            cameraRig.position = pos_abs_to_origin;       
    }

    private void MoveRelative() {
        // get the position the r hand is relative to the left hand.
        Vector3 pos_rel_to_main_camera = mainCamera.InverseTransformPoint(rightHand.position);
        // transform this point based on the camera rig pos.
        Vector3 prtcr = cameraRig.TransformPoint(pos_rel_to_main_camera);
        // possibly clamp and add weight to the new pos...
         Vector3 pos_rel_to_camera_rig = new Vector3(RelWeightFunction(prtcr.x), RelWeightFunction(prtcr.y), RelWeightFunction(prtcr.z));
        // map the tracking space pos to the new transformed position...
        trackingSpace.position = pos_rel_to_camera_rig;
    }


    // Update is called once per frame
    void Update()
    {
        MoveAbsolute();
        //MoveRelative();
    }
}
