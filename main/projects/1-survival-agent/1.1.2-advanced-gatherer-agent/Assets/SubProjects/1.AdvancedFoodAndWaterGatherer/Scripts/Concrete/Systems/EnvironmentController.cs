using UnityEngine;
using System.Collections.Generic;

public class EnvironmentController : MonoBehaviour {
    // Referances

    public AdvancedFoodAndWaterGathererAgent EnvironmentAgent;

    public List<GameObject> FoodOrbs;
    public GameObject FoodOrbPrefab;
    public int maxFoodOrbCount;

    public GameObject WaterVolumePrefab;

    public GameObject CampfirePrefab;

    private void Start() {
        SpawnWaterVolume();
        SpawnFoodOrbs();
        SpawnCampfire();
    }

    private void SpawnWaterVolume() {
        int sideSwitch = Random.Range(0, 1);

        float spawnX, spawnZ = 0f;
        GameObject spawnedVolume = null;
        switch (sideSwitch) {
            case 0:
                spawnX = Random.Range(-18, 18);
                spawnZ = Random.Range(0, 1) > 0 ? -22f: 22f; 

                Vector3 spawnLoc = new Vector3(spawnX, 0f, spawnZ);
                spawnedVolume = Instantiate(WaterVolumePrefab, spawnLoc, Quaternion.identity);
                break;
            case 1:
                spawnX = Random.Range(0, 1) > 0 ? -22f : 22f;
                spawnZ = Random.Range(-18, 18);

                spawnLoc = new Vector3(spawnX, 0f, spawnZ);
                spawnedVolume = Instantiate(WaterVolumePrefab, spawnLoc, Quaternion.Euler(0f, 90f, 0f));
                break;
            default:
                break;
        }

        spawnedVolume.transform.SetParent(transform);
    }
    private void SpawnFoodOrbs() {

        for(int i = 0; i < maxFoodOrbCount; i++) {

            Vector3 spawnLoc = new Vector3(Random.Range(-17, 17), 1f, Random.Range(-17, 17));
            GameObject spawnedOrb = Instantiate(FoodOrbPrefab, spawnLoc, Quaternion.identity);
            spawnedOrb.transform.SetParent(transform);

            FoodOrbs.Add(spawnedOrb);
        }
    }
    private void SpawnCampfire() {
        Vector3 spawnLoc = new Vector3(Random.Range(-12, 12), 1f, Random.Range(-12, 12));
        GameObject spawnedCampfire = Instantiate(CampfirePrefab, spawnLoc, Quaternion.identity);
        spawnedCampfire.transform.SetParent(transform);
    }

}
