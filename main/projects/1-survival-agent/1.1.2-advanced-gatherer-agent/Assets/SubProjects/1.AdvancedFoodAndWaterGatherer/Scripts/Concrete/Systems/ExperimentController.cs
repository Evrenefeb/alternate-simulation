using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

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

    [Header("Control")]
    [SerializeField] private TMP_Text txt_EnvironmentIndex;
    [SerializeField] private Transform allEnvironmentsCameraTransform;

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
        Control();
    }

    public void Subscribe_EnvironmentCameraLocationList(EnvironmentController environmentController) {
        EnvironmentCameraLocationList.Add(environmentController.transform.GetChild(0).transform);
    }

    private void Control() {
        

        if (Input.GetKeyDown(KeyCode.X)) {
            if (currentEnvironmentIndex == 0) return; 
            currentEnvironmentIndex = (currentEnvironmentIndex + 1) % numberOfEnvironements;
            ChangeCameraLocation(EnvironmentCameraLocationList[Mathf.Abs(currentEnvironmentIndex)]);
        }else if (Input.GetKeyDown(KeyCode.Z)) {
            if (currentEnvironmentIndex == EnvironmentCameraLocationList.Count - 1) return;
            currentEnvironmentIndex = (currentEnvironmentIndex - 1) % numberOfEnvironements;
            ChangeCameraLocation(EnvironmentCameraLocationList[Mathf.Abs(currentEnvironmentIndex)]);
        }else if (Input.GetKeyDown(KeyCode.Space)) {
            ChangeCameraLocation(allEnvironmentsCameraTransform);
        }
        
        txt_EnvironmentIndex.text = $"Environment: {-currentEnvironmentIndex}";
    }

    private void ChangeCameraLocation(Transform newCameraLocation) {
        MainCamera.transform.position = newCameraLocation.position;
    }

    

    private void Update() {
        Control();
    }
}
