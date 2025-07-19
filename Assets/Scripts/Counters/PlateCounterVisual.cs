using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateCounterVisual : MonoBehaviour
{
    [SerializeField] private PlateCounter counter;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform plateVisualPrefab;

    private List<GameObject> platesVisualGameObjectList;
    
    private void Awake()
    {
        platesVisualGameObjectList = new List<GameObject>();
    }
    
    private void Start()
    {
        counter.OnPlateSpawned += CounterOnOnPlateSpawned;
        counter.OnPlateRemoved += CounterOnOnPlateRemoved;
    }

    private void CounterOnOnPlateRemoved(object sender, EventArgs e)
    {
        GameObject plateGameObject = platesVisualGameObjectList[platesVisualGameObjectList.Count - 1];
        platesVisualGameObjectList.Remove(plateGameObject);
        Destroy(plateGameObject);
    }

    private void CounterOnOnPlateSpawned(object sender, EventArgs e)
    {
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);

        float plateOffsetY = .1f;
        plateVisualTransform.localPosition = new Vector3(0f, plateOffsetY * platesVisualGameObjectList.Count, 0f);
        
        platesVisualGameObjectList.Add(plateVisualTransform.gameObject);;
    }
}