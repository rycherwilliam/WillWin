using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Shooting : MonoBehaviour
{

    [SerializeField]
    Camera fpsCamera;


    public float fireRate = 2f;
    float fireTimer;


    void Start()
    {
        
    }


    void Update()
    {
        if (fireTimer < fireRate)
        {
            fireTimer += Time.deltaTime;
        }

        if (Input.GetButton("Fire2") && fireTimer < fireRate)
        {
            GetComponent<Animator>().SetBool("isAtacking", false);
        }
        

        if (Input.GetButton("Fire2") && fireTimer > fireRate)
        {
            fireTimer = 0.0f;
            GetComponent<Animator>().SetBool("isWalking", false);            
            GetComponent<Animator>().SetBool("isAtacking", true);
            
            //GetComponent<Animator>().SetBool("isAtacking", true);

            RaycastHit _hit;
            Ray ray = fpsCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            if (Physics.Raycast(ray, out _hit, 100))
            {
                Debug.Log(_hit.collider.gameObject.name);

                if (_hit.collider.gameObject.CompareTag("Player") && !_hit.collider.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    _hit.collider.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, 0f);
                }
            }
        }
    }
}
