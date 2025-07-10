using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class EnvironmentController : MonoBehaviour {
    // Referances

    public AdvancedFoodAndWaterGathererAgent EnvironmentAgent;

    public List<GameObject> FoodOrbs;
    public GameObject FoodOrbPrefab;
    public int maxFoodOrbCount;

    public GameObject WaterVolumePrefab;

    public GameObject CampfirePrefab;

    [SerializeField] private TMP_Text txt_CumulativeReward;

    private float episodeTimer;
    public float episodeTime;

    private void Start() {
        ResetEnvironment();
        EnvironmentAgent.OnEpisodeEnd += EnvironmentAgent_OnEpisodeEnd;
    }

    private void EnvironmentAgent_OnEpisodeEnd() {
        episodeTimer = 0;
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
                spawnedVolume = Instantiate(WaterVolumePrefab, transform.position + spawnLoc, Quaternion.identity);
                break;
            case 1:
                spawnX = Random.Range(0, 1) > 0 ? -22f : 22f;
                spawnZ = Random.Range(-18, 18);

                spawnLoc = new Vector3(spawnX, 0f, spawnZ);
                spawnedVolume = Instantiate(WaterVolumePrefab, transform.position + spawnLoc, Quaternion.Euler(0f, 90f, 0f));
                break;
            default:
                break;
        }

        spawnedVolume.transform.SetParent(transform);
    }
    private void SpawnFoodOrbs() {

        for(int i = 0; i < maxFoodOrbCount; i++) {

            Vector3 spawnLoc = new Vector3(Random.Range(-17, 17), 1f, Random.Range(-17, 17));
            GameObject spawnedOrb = Instantiate(FoodOrbPrefab, transform.position + spawnLoc, Quaternion.identity);
            spawnedOrb.transform.SetParent(transform);

            FoodOrb foodOrbComponent = spawnedOrb.GetComponent<FoodOrb>();
            if(foodOrbComponent != null) {
                foodOrbComponent.SetEnvironmentController(this);
            }

            FoodOrbs.Add(spawnedOrb);
        }
    }
    private void SpawnCampfire() {
        Vector3 spawnLoc = new Vector3(Random.Range(-12, 12), 1f, Random.Range(-12, 12));
        GameObject spawnedCampfire = Instantiate(CampfirePrefab, transform.position + spawnLoc, Quaternion.identity);
        spawnedCampfire.transform.SetParent(transform);
    }
    private void ResetAgentPosition() {
        Vector3 center = new Vector3(transform.position.x, 1f, transform.position.z);
        EnvironmentAgent.transform.position = center;
    }
    private void ResetEnvironment() {
        ResetAgentPosition();
        SpawnWaterVolume();
        SpawnFoodOrbs();
        SpawnCampfire();
    }

    public void RespawnFoodOrb(GameObject foodOrb) {
        if(foodOrb != null) {
            Vector3 newSpawnLoc = new Vector3(Random.Range(-17,17), 0f, Random.Range(-17, 17));
            foodOrb.transform.position = transform.position + newSpawnLoc;

            if (foodOrb.activeSelf) return;
            foodOrb.SetActive(true);
        }
    }
    private void Update() {
        txt_CumulativeReward.text = EnvironmentAgent.GetCumulativeReward().ToString("F2");

        episodeTimer += Time.deltaTime;

        if (episodeTimer > episodeTime) { 
            EnvironmentAgent.EndEpisode(); 
        }
    }
}
