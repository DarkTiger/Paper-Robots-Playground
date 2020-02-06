using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float gravityForce;
    [SerializeField] float jumpForce;
    [SerializeField] float movementSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] SpriteRenderer playerIcon;

    PlayerAttack playerAttack;
    PlayerStats playerStat;
    PlayerAudio playerAudio;
    private int playerIndex;

    private bool isGrouded;
    bool axesPress = false;
    bool notCoroutineIcon = true;


    Vector3 direction = new Vector3(0, 0, 0);

    Planet planet = null;

    Rigidbody rigidBody;


    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        playerAudio = GetComponent<PlayerAudio>();
        playerAttack = GetComponent<PlayerAttack>();
        playerStat = GetComponent<PlayerStats>();
    }

    void Start()
    {       
        planet = FindObjectOfType<Planet>();
        isGrouded = true;
        playerIndex = playerStat.playerIndex;
    }

    void FixedUpdate()
    {
        if (!playerStat.isDead)
        {
            float horizontalMovement = Input.GetAxis("HorizontalP" + playerIndex.ToString());
            float strifeMovement = Input.GetAxis("StrifeP" + playerIndex.ToString());
            float verticalMovement = Input.GetAxis("VerticalP" + playerIndex.ToString());
            Vector3 direction = (planet.transform.position - transform.position).normalized;

            transform.rotation = Quaternion.FromToRotation(transform.up, -direction) * transform.rotation; //mantiene il player orientato verso la superfice

            rigidBody.AddForce(direction * gravityForce, ForceMode.Acceleration);

            rigidBody.AddTorque(transform.up * horizontalMovement * rotationSpeed, ForceMode.VelocityChange);
            rigidBody.AddForce(transform.forward * verticalMovement * movementSpeed, ForceMode.Impulse);
            rigidBody.AddForce(-transform.right * strifeMovement * (movementSpeed / 2), ForceMode.Impulse);

            if (notCoroutineIcon)
            {
                if (horizontalMovement != 0 || verticalMovement != 0 || strifeMovement != 0)
                {
                    StartCoroutine(EnableIconMov());
                }
                else
                {
                    StartCoroutine(DisableIcon());
                }
                
            }
            if (Input.GetAxis("JumpP" + playerIndex) > 0)
            {
                EnableIconJump();
            }
        }
    }

    private void Update()
    {
        if ((Input.GetAxis("JumpP" + playerIndex)>0) && isGrouded && !axesPress)
        {
            axesPress = true;
            rigidBody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            isGrouded = false;
            playerAudio.jumpAudio.Play();
        }
        ReversePressAxe();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<Planet>())
        {
            isGrouded = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!playerStat.isDead)
        {
            if (other.CompareTag("Gear"))
            {
                onPicked(other);
            }
            if (other.CompareTag("Sniper"))
            {
                onPicked(other);
            }
            if (other.CompareTag("Shotgun"))
            {
                onPicked(other);
            }
            if (other.CompareTag("SMG"))
            {
                onPicked(other);
            }
        }
    }
    void onPicked(Collider other)
    {       
        if (other.CompareTag("Gear"))
        {
            playerAudio.gearPickAudio.Play();
            playerStat.activeGear(true);
        }
        else
        {
            playerAudio.weaponPickAudio.Play();

            int i = 0;
            if (other.CompareTag("Sniper"))
            {
                i = 3;
                playerAttack.currentBullet = playerAttack.bullets[3];
            }
            if (other.CompareTag("Shotgun"))
            {
                i = 1;
                playerAttack.currentBullet = playerAttack.bullets[1];
            }
            if (other.CompareTag("SMG"))
            {
                i = 2;
                playerAttack.currentBullet = playerAttack.bullets[2];
            }
            playerAttack.currentAmmo = playerAttack.currentBullet.startAmmo;
            playerStat.UpdateWeaponsHUD();
            playerStat.UpdateAmmoRemain(playerAttack.currentAmmo);
        }
        Destroy(other.gameObject);
    }

    void ReversePressAxe()
    {
        if (Input.GetAxis("JumpP" + playerIndex) > 1)
        {
            axesPress = true;
        }
        if (Input.GetAxis("JumpP" + playerIndex) == 0)
        {
            axesPress = false;
        }
    }

    IEnumerator EnableIconMov()
    {
        notCoroutineIcon = false;
        yield return new WaitForSeconds(0.5f);
        while (playerIcon.color.a<1)
        {
            Color newColor = playerIcon.GetComponent<SpriteRenderer>().color;
            newColor.a += 0.5f * Time.deltaTime;
            playerIcon.color = newColor;
            yield return null;
        }
        notCoroutineIcon = true;
    }
    void EnableIconJump()
    {
        Color newColor = playerIcon.color;
        newColor.a = 1;
        playerIcon.color = newColor;
    }

    IEnumerator DisableIcon()
    {
        notCoroutineIcon = false;
        yield return new WaitForSeconds(0.2f);
        while (playerIcon.color.a > 0)
        {
            Color newColor = playerIcon.color;
            newColor.a -= 1f * Time.deltaTime;
            playerIcon.color = newColor;
            yield return null;
        }
        notCoroutineIcon = true;
    }
}
