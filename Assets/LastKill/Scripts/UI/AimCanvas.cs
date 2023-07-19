using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimCanvas : MonoBehaviour
{
    [SerializeField] private GameObject simple, scope;
    public void SimpleCanvas(bool state)
    {
        simple.SetActive(state);
    }
    public void ScopeCanvas(bool state)
    {
        scope.SetActive(state);
    }
}
