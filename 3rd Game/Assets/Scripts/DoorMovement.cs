using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMovement : MonoBehaviour
{
    public LayerMask GroundLayer;
    [HideInInspector] public DoorsObstBehavior DrObs;

    private Vector3 Target;
    private float LeftX, RightX;
    [HideInInspector] public float OffsetX;     

    public Direction Side { get; set; }

    private MeshRenderer[] Meshes;
    private Material Mat;

    void Start()
    {
        RightX = transform.position.x + OffsetX;
        LeftX = transform.position.x - OffsetX;

        //Preparing The Meshes to reassign there material later
        Meshes = new MeshRenderer[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            Meshes[i] = transform.GetChild(i).GetComponent<MeshRenderer>();
        }

        Mat = Meshes[0].material;
    }    

    void FixedUpdate()
    {
        if (!PlayerInteractions.Dead)
        {
            if (Side == Direction.Right)
            {
                Target = new Vector3(RightX, transform.position.y, transform.position.z);

                transform.position = Vector3.MoveTowards(transform.position, Target, DrObs.Speed);

                if ((transform.position - Target).magnitude <= .2f)
                {
                    Side = Direction.Left;
                    AudioManager.AudMan.Play("Doors");
                }

            }
            else if (Side == Direction.Left)
            {
                Target = new Vector3(LeftX, transform.position.y, transform.position.z);

                transform.position = Vector3.MoveTowards(transform.position, Target, DrObs.Speed);

                if ((transform.position - Target).magnitude <= .2f)
                {
                    Side = Direction.Right;
                    AudioManager.AudMan.Play("Doors");
                }
            }
        }               
    }  

    public void SetPosition()
    {
        //Set the Doors Position
        Physics.Raycast(transform.position, Vector3.right, out RaycastHit hitR, 10);
        Physics.Raycast(transform.position, Vector3.left, out RaycastHit hitL, 10);
        float rdis = hitR.transform.position.x - hitR.point.x;
        float ldis = Mathf.Abs(hitL.transform.position.x - hitL.point.x);

        hitR.transform.position = new Vector3(transform.position.x + (DrObs.PassSize / 2) + rdis, hitR.transform.position.y, hitR.transform.position.z);
        hitL.transform.position = new Vector3(transform.position.x - (DrObs.PassSize / 2) - ldis, hitL.transform.position.y, hitL.transform.position.z);        

    }

    public void ResetMats()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Meshes[i].material = Mat;
        }
    }
}
