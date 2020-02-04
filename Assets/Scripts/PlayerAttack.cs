using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Bullet defaultBullet;
    public Bullet[] bullets;
    public Bullet currentBullet;
    public int currentAmmo { get; set;} = 0;
    int playerIndex;
    float lastFire = 0;
    bool axesPress = false;
    Rigidbody rigidbody;
    PlayerAudio playerAudio;
    PlayerStats playerStats;


    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        playerAudio = GetComponent<PlayerAudio>();
        rigidbody = GetComponent<Rigidbody>();
        playerIndex = GetComponent<PlayerStats>().playerIndex;
        currentAmmo = currentBullet.startAmmo;
    }

    private void Update()
    {
        if (GameManager.isPaused || playerStats.isDead) { return; }
        if (Time.time > lastFire + currentBullet.fireDelay)
        {
            if (currentBullet.type == Bullet.Type.SMG)
            {
                /*if (currentAmmo > 0 && Input.GetButton("FireP" + playerIndex))
                {
                    Bullet bullet = Instantiate(currentBullet.gameObject, transform.position + transform.forward + transform.up, transform.rotation).GetComponent<Bullet>();
                    bullet.owner = GetComponent<PlayerStats>();

                    lastFire = Time.time;
                    currentAmmo--;
                    

                    if (currentAmmo == 0)
                    {
                        currentBullet = defaultBullet;
                    }
                    GetComponent<PlayerStats>().UpdateAmmoRemain(currentAmmo);
                }
            }
            else
            {
                if ((currentAmmo > 0 || currentBullet.type == Bullet.Type.Default) && Input.GetButtonDown("FireP" + playerIndex))
                {
                    if (currentBullet.type == Bullet.Type.Default || currentBullet.type == Bullet.Type.Sniper)
                    {
                        Bullet bullet = Instantiate(currentBullet.gameObject, transform.position + (transform.forward*1.5f) + transform.up, transform.rotation).GetComponent<Bullet>();
                        bullet.owner = GetComponent<PlayerStats>();
                    }
                    else
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            Bullet bullet = Instantiate(currentBullet.gameObject, transform.position + transform.forward + transform.up, transform.rotation).GetComponent<Bullet>();
                            bullet.owner = GetComponent<PlayerStats>();
                        }
                    }
                    
                    lastFire = Time.time;
                    currentAmmo--;
                    
                    if (currentAmmo == 0)
                    {
                        currentBullet = defaultBullet;
                    }
                    GetComponent<PlayerStats>().UpdateAmmoRemain(currentAmmo);

                }*/
                if (currentAmmo > 0 && (Input.GetAxis("FireP" + playerIndex)>0))
                {
                    Bullet bullet = Instantiate(currentBullet.gameObject, transform.position + transform.forward + transform.up, transform.rotation).GetComponent<Bullet>();
                    bullet.owner = GetComponent<PlayerStats>();
                    
                    GetComponent<PlayerStats>().UpdateAmmoRemain(currentAmmo);
                    rigidbody.velocity -= transform.forward * bullet.recoil;

                    lastFire = Time.time;
                    currentAmmo--;

                    if (currentAmmo == 0)
                    {
                        currentBullet = defaultBullet;
                    }
                }
            }
            else
            {
                if (((currentAmmo > 0 || currentBullet.type == Bullet.Type.Default) && Input.GetAxis("FireP" + playerIndex) > 0) && !axesPress)
                {
                    axesPress = true;
                    if (currentBullet.type == Bullet.Type.Default || currentBullet.type == Bullet.Type.Sniper)
                    {
                        Bullet bullet = Instantiate(currentBullet.gameObject, transform.position + (transform.forward * 1.5f) + transform.up, transform.rotation).GetComponent<Bullet>();
                        bullet.owner = GetComponent<PlayerStats>();
                        rigidbody.velocity -= transform.forward * bullet.recoil;
                    }
                    else
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            Bullet bullet = Instantiate(currentBullet.gameObject, transform.position + transform.forward + transform.up, transform.rotation).GetComponent<Bullet>();
                            bullet.owner = GetComponent<PlayerStats>();
                            rigidbody.velocity -= transform.forward * bullet.recoil;
                        }
                    }

                    lastFire = Time.time;
                    currentAmmo--;

                    if (currentAmmo == 0)
                    {
                        currentBullet = defaultBullet;
                    }

                    GetComponent<PlayerStats>().UpdateAmmoRemain(currentAmmo);
                }
            }
        }
        ReversePressAxe();
    }
    public void changeWeaponsAmmo(int i)
    {
        currentBullet = GetComponent<PlayerAttack>().bullets[i];
    }
    void ReversePressAxe()
    {
        if (Input.GetAxis("FireP" + playerIndex) == 1)
        {
            axesPress = true;
        }
        if (Input.GetAxis("FireP" + playerIndex) == 0)
        {
            axesPress = false;
        }
    }
}
