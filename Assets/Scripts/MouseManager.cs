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

    //Enlever une partie du vaisseau
    void RightClick()
    {
        Collider col = DoRaycast();

        if (col == null)
            return;

        Destroy(col.transform.parent.gameObject);
    }

    //Poser une partie de vaisseau
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

    void SetSnapPointEnabled (Transform t, bool setToActive)
    {
        int MaskforThisHitObject = 1 << t.gameObject.layer;

        if ((MaskforThisHitObject & SnapLayerMask) != 0 )
        {
            //Activation du SnapPoint (au cas où)
            if (setToActive)
                t.gameObject.SetActive(true);
            else
            {
                //Desactivation du SnapPoint si il n'a pas d'enfants
                if (t.childCount == 0)
                {
                    t.gameObject.SetActive(false);
                    return;
                }
            }
        }

        for (int i=0; i < t.childCount; i++)
        {
            SetSnapPointEnabled(t.GetChild(i), setToActive);
        }
    }


	
}
