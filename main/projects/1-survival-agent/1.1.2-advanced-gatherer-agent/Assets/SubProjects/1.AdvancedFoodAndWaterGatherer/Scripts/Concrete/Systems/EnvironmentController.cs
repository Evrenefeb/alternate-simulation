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
    [SerializeField] private TMP_Text txt_EpisodeTime;

    [SerializeField] private float episodeTimer;

    public float episodeTime;
    private float initialEpisodeTime;


    [Header("Instances")]
    [SerializeField] private List<GameObject> prefabInstanceList = new List<GameObject>();

    private void Awake() {
        initialEpisodeTime = episodeTime;
        ExperimentController.OnExperimentStart += ExperimentController_OnExperimentStart;
    }

    private void Start() {
        ResetEnvironment();
        EnvironmentAgent.OnEpisodeEnd += EnvironmentAgent_OnEpisodeEnd;
    }

    private void ExperimentController_OnExperimentStart() {
        ExperimentController.Instance.Subscribe_EnvironmentCameraLocationList(this);
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
        prefabInstanceList.Add(spawnedVolume);
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
            prefabInstanceList.Add(spawnedOrb);
        }
    }
    private void SpawnCampfire() {
        Vector3 spawnLoc = new Vector3(Random.Range(-12, 12), 1f, Random.Range(-12, 12));
        GameObject spawnedCampfire = Instantiate(CampfirePrefab, transform.position + spawnLoc, Quaternion.identity);
        spawnedCampfire.transform.SetParent(transform);

        prefabInstanceList.Add(spawnedCampfire);
    }
    private void ResetAgentPosition() {
        Vector3 center = new Vector3(transform.position.x, 1f, transform.position.z);
        EnvironmentAgent.transform.position = center;
    }
    private void ResetEnvironment() {
        ClearInstances(prefabInstanceList);
        ResetAgentPosition();
        SpawnWaterVolume();
        SpawnFoodOrbs();
        SpawnCampfire();
        episodeTimer = 0;
    }
    private void ClearInstances(List<GameObject> instanceList) {
        if (instanceList == null && FoodOrbs == null) return;

        CleanUpFoodOrbs();
        foreach (GameObject instance in instanceList) { 
            Destroy(instance);
        }

        instanceList.Clear();
    }
    private void CleanUpFoodOrbs() {
        FoodOrbs.RemoveAll(item => item == null);
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
        txt_EpisodeTime.text = $"{episodeTimer.ToString("F2")}/{episodeTime.ToString("F2")}";

        if (episodeTimer > episodeTime) { 
            EnvironmentAgent.EndEpisode();
            ResetEnvironment();
        }
    }
}
