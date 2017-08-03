using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorManager : MonoBehaviour
{
    public GameObject PrefabToSpawn;
    public GameObject rootToSpawn;
    public Transform tfmRootSpawn;
    public LayerMask SnapLayerMask;
    public GameObject shipRoot;

    public CameraManager camManager;


    private void Start()
    {
        camManager = FindObjectOfType<CameraManager>();

        FirstRoot();
    }

    public void FirstRoot()
    {
            var root = Instantiate(rootToSpawn, tfmRootSpawn);
            root.transform.parent = null;
            camManager.NewRoot(root.transform.position);
            Debug.Log(rootToSpawn + " a été créé la premiere fois");
    }


    public void CreateRoot()
    {
        GameObject goRoot = FindObjectOfType<Root>().gameObject;

        if (goRoot == null)
        {
            var root = Instantiate(rootToSpawn, tfmRootSpawn);
            root.transform.parent = null;
            Debug.Log(rootToSpawn + " a été créé");
        }
    }

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
            if ((curr.gameObject.tag == "ShipPart") || (curr.gameObject.tag == "Root"))
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

        if ((col.gameObject.tag == "ShipPart") || (col.gameObject.tag == "Root"))
        {
            if (col.GetComponent<Root>() != null)
                Debug.Log("Vous ne pouvez pas supprimer le coeur!");
            else
                Destroy(col.transform.parent.gameObject);
        }
        else
        {
            Debug.Log("Vous ne pouvez pas supprimer ça");

        }


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
