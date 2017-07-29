using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public GameObject PrefabToSpawn;
    public LayerMask SnapLayerMask;
    public GameObject shipRoot;

    void Update()
    {
        //Clique Gauche
        if (Input.GetMouseButtonDown(0))
            LeftClick();

        //Clique droit
        if (Input.GetMouseButtonDown(1))
            RightClick();
    }

    Collider  DoRaycast()
    {
        Camera cam = Camera.main;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
            return hitInfo.collider;

        return null;
    }

    void RightClick()
    {

    }

    GameObject FindShipPart(Collider collider)
    {
        Transform curr = collider.transform;

        while (curr != null)
        {
            if (curr.gameObject.tag == "ShipPart")
                return curr.gameObject;

            curr = curr.parent;
        }

        return null;
    }

    void LeftClick()
    {
        Collider col = DoRaycast();

        if (col == null)
            return;

        int maskForThisObject = 1 << col.gameObject.layer;

        if ((maskForThisObject & SnapLayerMask) != 0)
        {
            Vector3 spawnSpot = col.transform.position;

            Quaternion spawnRot = col.transform.rotation;

            GameObject go = (GameObject)Instantiate(PrefabToSpawn, spawnSpot, spawnRot);
            go.transform.SetParent(col.transform);
        }
    }


	
}
