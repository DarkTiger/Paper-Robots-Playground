using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour
{
    [SerializeField] GameObject[] envObjects = null;
    [SerializeField] int envCount = 100;
    [SerializeField] GameObject[] bonusItems = null;
    [SerializeField] float bonusSpawnTime = 1;

    private void Start()
    {
        //StartCoroutine(SpawnEnvObjects());
        StartCoroutine(SpawnBonusItems());
    }

    /*IEnumerator SpawnEnvObjects()
    {
        float currentCount = 0;

        while (currentCount < envCount)
        {
            GameObject envObject = envObjects[Random.Range(0, envObjects.Length)];
            Vector3 position = transform.position + new Vector3(Random.Range(-400, 400), Random.Range(-400, 400), Random.Range(-400, 400));
            Vector3 direction = (transform.position - position).normalized;
            RaycastHit hit;

            if (Physics.Raycast(position, direction, out hit, LayerMask.GetMask("Planet")))
            {
                if (hit.collider.CompareTag("Planet"))
                {
                    Transform envObjectInstantiated = Instantiate(envObject, hit.point, Quaternion.identity).transform;
                    envObjectInstantiated.up = (envObjectInstantiated.position - transform.position).normalized;
                    currentCount++;
                }
            }
            yield return null;
        }
    }*/

    IEnumerator SpawnBonusItems()
    {
        while (true)
        {
            yield return new WaitForSeconds(bonusSpawnTime);
            
            GameObject bonusObject = bonusItems[Random.Range(0, bonusItems.Length)];
            Vector3 position = transform.position + new Vector3(Random.Range(-400, 400), Random.Range(-400, 400), Random.Range(-400, 400));
            Vector3 direction = (transform.position - position).normalized;
            RaycastHit hit;

            if (Physics.Raycast(position, direction, out hit, LayerMask.GetMask("Planet")))
            {
                if (hit.collider.CompareTag("Planet"))
                {
                    Transform bonusInstantiated = Instantiate(bonusObject, hit.point, Quaternion.identity).transform;
                    bonusInstantiated.up = (bonusInstantiated.position - transform.position).normalized;
                    bonusInstantiated.position += bonusInstantiated.up * 0.75f; 
                }
            }
        }
    }
}
