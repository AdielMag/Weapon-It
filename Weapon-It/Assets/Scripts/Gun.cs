using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float fireSpeed = 10;
    public float rotRecoilAmount = 150;
    public float posRecoilAmount = 4;

    public Transform slide;
    public float slideRecoilModifier = 20;
    Vector3 slideOrigPos;

    public Transform shellExit;

    private void Start()
    {
        slideOrigPos = slide.localPosition;

        // Set the weapons correct pos and rot.
        targetPos = transform.localPosition;
        slideTargetPos = slideOrigPos;

    }

    private void FixedUpdate()
    {
        targetRot = Vector3.Lerp(targetRot, Vector3.zero, Time.deltaTime * fireSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.Euler(targetRot), Time.deltaTime * 10);

        targetPos = Vector3.Lerp(targetPos, Vector3.zero, Time.deltaTime * fireSpeed);
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * 10);

        slideTargetPos = Vector3.Lerp(slideTargetPos, slideOrigPos, Time.deltaTime * fireSpeed);
        slide.localPosition = Vector3.Lerp(slide.localPosition, slideTargetPos, Time.deltaTime * 10);

        if (Input.GetMouseButtonDown(0))
            Shoot();
    }

    Vector3 targetRot;
    Vector3 targetPos;
    Vector3 slideTargetPos;
    public void Shoot()
    {
        // Set X rot recoil.
        targetRot -= transform.right * Random.Range(rotRecoilAmount * .7f, rotRecoilAmount);
        
        // Set Y rot Recoil.
        targetRot -= transform.up * Random.Range(-rotRecoilAmount, rotRecoilAmount) / 10;

        // Set Z rot Recoil.
        targetRot -= transform.forward * Random.Range(-rotRecoilAmount, rotRecoilAmount) / 8;


        //Set pos recoil.
        targetPos -= transform.parent.forward * Random.Range(posRecoilAmount * .7f, posRecoilAmount);

        // Set slide pos recoil.
        slideTargetPos -= transform.forward * (Random.Range(posRecoilAmount * .85f, posRecoilAmount) / slideRecoilModifier);

    }
}
