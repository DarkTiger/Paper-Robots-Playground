using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] Text lblHealth;
    [SerializeField] Text lblRepair;
    [SerializeField] Text lblAmmo;
    [SerializeField] Image imgCollectorObject;
    [SerializeField] Image imgWeapons;
    [SerializeField] GameObject objectSpawn;
    [SerializeField] GameObject[] shipModelVariants;
    [SerializeField] Image[] victory;
    
    public int playerIndex;
    public int health;
    public int maxHealth = 100;
    public int repair;

    PlayerAttack playerAttack;
    PlayerAudio playerAudio;
    int currentShipVariant = 0;

    public float waitTime = 1f;


    public bool isDead
    {
        get
        {
            return health <= 0;
        }
    }

    private void Awake()
    {
        playerAudio = GetComponent<PlayerAudio>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    void Start()
    {
        UpdateAmmoRemain(playerAttack.currentAmmo);
        health = maxHealth;
        repair = 0;
        lblHealth.GetComponent<Text>().text = "Life: " + health + "%";
        lblRepair.GetComponent<Text>().text = "Repair: " + repair + "%";
        UpdateWeaponsHUD();
        imgCollectorObject.GetComponent<Image>().enabled = false;
    }

    public void Damage(int damage, Bullet bullet)
    {
        if (isDead) { return; }
        health -= damage;
        if (health<=0)
        {
            Die(1, bullet);
        }
        else
        {
            playerAudio.damageAudio.Play();
        }
        lblHealth.GetComponent<Text>().text = "Life: "+health+"%";
    }

    void Die(int i, Bullet bullet)
    {
        Vector3 posTemp = transform.position + transform.up;

        playerAudio.deathAudio.Play();

        StartCoroutine(Respawn());

        if (!GameManager.objOnScene && GameManager.objOwner != this && bullet.owner == this) { return; }

        if (!GameManager.objOnScene && ((GameManager.objOwner == this) || GameManager.objOwner == null))
        {
            Vector3 planetPos = FindObjectOfType<Planet>().transform.position;
            Transform objectInstantiated = Instantiate(objectSpawn, posTemp, Quaternion.identity).transform;
            objectInstantiated.up = (objectInstantiated.position - planetPos).normalized;
            GameManager.objOnScene = true;
            GameManager.objOwner = null;
        }
    }

    IEnumerator Respawn()
    {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;

        yield return new WaitForSeconds(3f);
        
        health = maxHealth;
        lblHealth.GetComponent<Text>().text = "Life: " + health + "%";
        transform.localPosition = Vector3.zero;
        imgCollectorObject.GetComponent<Image>().enabled = false;
        playerAttack.currentBullet = playerAttack.defaultBullet;
        UpdateAmmoRemain(0);
        UpdateWeaponsHUD();

        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<CapsuleCollider>().enabled = true;
    }

    public void activeGear(bool active)
    {
        if (active)
        {
            imgCollectorObject.GetComponent<Image>().enabled = true;
        }
        else
        {
            imgCollectorObject.GetComponent<Image>().enabled = false;
        }

        GameManager.objOnScene = false;
        GameManager.objOwner = this;
    }

    private void OnCollisionEnter(Collision other)
    {
        bool repairObject = imgCollectorObject.GetComponent<Image>().enabled;
        if (other.gameObject.CompareTag("ShipP"+playerIndex)&&repairObject)
        {
            GetComponent<AudioSource>().Play();
            repair += 25;                    

            switch (repair)
            {
                case 50:
                    currentShipVariant = 1;
                    shipModelVariants[0].SetActive(false);
                    shipModelVariants[1].SetActive(true);
                    break;
                case 100:
                    currentShipVariant = 2;
                    shipModelVariants[1].SetActive(false);
                    shipModelVariants[2].SetActive(true);
                    Victory();
                    break;
            }

            lblRepair.GetComponent<Text>().text = "Repair: " + repair + "%";
            imgCollectorObject.GetComponent<Image>().enabled = false;
            GameManager.objOnScene = false;
            GameManager.objOwner = null;
        }
    }

    public void UpdateWeaponsHUD()
    {
        imgWeapons.sprite = playerAttack.currentBullet.weaponSprite;
    }

    public void UpdateAmmoRemain(int ammoRemain)
    {
        string ammoRemainString = ammoRemain.ToString();
        if (playerAttack.currentBullet.type == Bullet.Type.Default)
        {
            ammoRemainString = "∞";
            lblAmmo.text = "Ammo: " + ammoRemainString + "/" + ammoRemainString;
        }
        else
        {
            lblAmmo.text = "Ammo: " + ammoRemainString + "/" + playerAttack.currentBullet.startAmmo;
        }
        UpdateWeaponsHUD();
    }
    
    void Victory()
    {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<PlayerMovement>().enabled = false;

        FindObjectOfType<Planet>().GetComponent<AudioSource>().Stop();

        StartCoroutine(VictoryCoroutine());
    }

    IEnumerator VictoryCoroutine()
    {
        yield return new WaitForSeconds(waitTime);

        playerAudio.victoryAudio.Play();

        while(victory[playerIndex - 1].color.a < 1)
        {
            Color newColor = victory[playerIndex - 1].color;
            newColor.a += 0.5f * Time.deltaTime;
            victory[playerIndex - 1].color = newColor;
            yield return null;
        }

        yield return new WaitForSeconds(5f);

        SceneManager.LoadScene(1);
    }
}
