using System;
using UnityEngine;

public class AgentLocation : MonoBehaviour {


    [SerializeField] private float _meridian = 0.5f;
    public float Meridian {
        get {
            return _meridian;
        }
        set { 
            var old = _meridian;
            if(value > 1 || value < -1) return;
            if(value != old) {
                OnMeridianChanged?.Invoke();
            }
        }

    }

    public event Action OnMeridianChanged;

}

