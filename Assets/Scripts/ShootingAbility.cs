using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingAbility : MonoBehaviourPunCallbacks
{
    [SerializeField] private float delayTime = 0.3f;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private Transform gunPosition;
    private GameObject bullet;
    private float currentTimer;

    public GameObject Bullet
        { set { bullet = value; } }

    private void Start()
    {
        currentTimer = delayTime;
    }

    public void ShootPress()
    {
        if (currentTimer > 0)
            return;
        else
        {
            GameObject newBullet = PhotonNetwork.Instantiate(bullet.name, gunPosition.position, gunPosition.rotation);
            newBullet.TryGetComponent(out DamageDealer damageDealer);
            damageDealer.ShooterID = photonView.ControllerActorNr;
            Rigidbody bulletBody = newBullet.GetComponent<Rigidbody>();
            Vector3 direction = transform.forward;
            direction.y = 0;
            bulletBody.AddForce(direction * bulletSpeed, ForceMode.Impulse);
            currentTimer = delayTime;
            Debug.Log(photonView.ControllerActorNr + " photonView.ControllerActorNr");
        }    
    }

    private void Update()
    {
        if (currentTimer >= 0)
        {
            currentTimer -= Time.deltaTime;
        }
    }
}
