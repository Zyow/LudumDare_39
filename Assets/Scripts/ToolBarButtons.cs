using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ToolBarButtons : MonoBehaviour
{
    public GameObject buttonPrefab;
    public List<ShipParts> shipPartsList = new List<ShipParts>();


    private void Start()
    {

        EditorManager mouseManager = GameObject.FindObjectOfType<EditorManager>();

        for (int i = 0; i < shipPartsList.Count; i++)
        {
            GameObject shipPart = shipPartsList[i].prefab;

            GameObject btnGO = (GameObject)Instantiate(buttonPrefab, this.transform);
            Text buttonLabel = btnGO.GetComponentInChildren<Text>();
            Debug.Log(shipPartsList[i].name);
            buttonLabel.text = shipPartsList[i].name;

            Button theButton = btnGO.GetComponent<Button>();

            theButton.onClick.AddListener(() =>
            {
                mouseManager.PrefabToSpawn = shipPart;
            });
        }
        
    }


}

[System.Serializable]
public class ShipParts
{
    public string name;
    public float number;
    public GameObject prefab;
}