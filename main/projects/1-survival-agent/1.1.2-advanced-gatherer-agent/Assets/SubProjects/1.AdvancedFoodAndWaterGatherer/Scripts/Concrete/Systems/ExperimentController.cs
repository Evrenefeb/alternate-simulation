using UnityEngine;
using System.Collections.Generic;
using System;

public class ExperimentController : MonoBehaviour
{
    // Singleton
    public static ExperimentController Instance { get; private set; }

    [Header("Components")]
    public Camera MainCamera;

    [Header("Environments")]
    public List<Transform> EnvironmentCameraLocationList = new List<Transform>();
    [SerializeField] private int currentEnvironmentIndex; 
    [SerializeField] private int numberOfEnvironements; 

    public static event Action OnExperimentStart;

    private void Awake() {

        if (Instance != null && Instance != this) {
            Destroy(this);
        }
        else {
            Instance = this;
        }
    }

    private void Start() {
        Init();
    }

    private void Init() {
        MainCamera = Camera.main;
        OnExperimentStart?.Invoke();
        currentEnvironmentIndex = 0;
        numberOfEnvironements = EnvironmentCameraLocationList.Count;
    }

    public void Subscribe_EnvironmentCameraLocationList(EnvironmentController environmentController) {
        EnvironmentCameraLocationList.Add(environmentController.transform.GetChild(0).transform);
    }

    private void Control() {
        if (Input.GetKeyDown(KeyCode.X)) {
            currentEnvironmentIndex++;
        }else if (Input.GetKeyDown(KeyCode.Z)) {
            currentEnvironmentIndex--;
        }
        Vector3 newCamLoc = EnvironmentCameraLocationList[Mathf.Abs(currentEnvironmentIndex % numberOfEnvironements)].position;
        MainCamera.transform.position = newCamLoc;
    }

    private void Update() {
        Control();
    }
}
