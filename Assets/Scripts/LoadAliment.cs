using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadAliment : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //Aliment temp = new Aliment();
        //temp.type = "folder 1";
        //temp.name = "pain";
        //temp.portions = 2;
        //temp.slices = 5;
        //temp.sugary = true;
        //temp.greasy = false;
        //temp.cold = false;
        //temp.hot = false;
        //LoadAndSetParent(temp, gameObject.transform);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public GameObject LoadAndSetParent(Aliment loadedAliment, Transform parent)
    {
        GameObject toEat = LoadWithSliceManagement(loadedAliment);
        toEat.transform.SetParent(parent);
        toEat.transform.localPosition = Vector3.zero;
        toEat.transform.localRotation = toEat.transform.rotation;
        return toEat;
    }

    public GameObject Load(Aliment loadedAliment){
        GameObject toEat = new GameObject();
        // ajout script contenant les informations de l'aliments
        if (!toEat.GetComponent<BlocAliment>())
        {
            toEat.AddComponent<BlocAliment>();
        }

        //// aliment
        Mesh mesh;
        if (loadedAliment.multiMesh)
        {
            mesh = Resources.Load<Mesh>("Aliments/Models/" + loadedAliment.name + "/" + loadedAliment.slices);
            for (int j = 1; j <= loadedAliment.slices; ++j)
            {
                toEat.GetComponent<BlocAliment>().Meshs.Add(Resources.Load<Mesh>("Aliments/Models/" + loadedAliment.name + "/" + j));
            }
        }
        else
        {
            mesh = Resources.Load<Mesh>("Aliments/Models/" + loadedAliment.name);
        }

        // ajout type, nom aliment, portion ->> string en minuscule et sans espaces
        toEat.name = loadedAliment.name;
        toEat.GetComponent<BlocAliment>().aliment = loadedAliment;
        toEat.layer = 9;
        toEat.tag = "Aliment";
        toEat.AddComponent<MeshFilter>();
        toEat.GetComponent<MeshFilter>().mesh = mesh;

        toEat.AddComponent<MeshRenderer>();

        // suppression collider si existant + ajout box collider
        if (toEat.GetComponent<Collider>())
        {
            Collider[] cols = toEat.GetComponents<Collider>();
            foreach (Collider col in cols)
                Destroy(col);
        }
        toEat.AddComponent<BoxCollider>();
        toEat.GetComponent<BoxCollider>().isTrigger = true;


        //Taille et parentée
        toEat.GetComponent<BlocAliment>().SetDefaultSize();
        return toEat;
    }

    public GameObject LoadWithSliceManagement(Aliment loadedAliment){

        GameObject toEat = Load(loadedAliment);
        Mesh mesh = toEat.GetComponent<MeshFilter>().mesh;

        if (loadedAliment.multiMesh){
            toEat.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Aliments/Textures/" + loadedAliment.name+loadedAliment.slices);
            
            for (int j = 1; j <= loadedAliment.slices; ++j)
            {
                toEat.GetComponent<BlocAliment>().Materials.Add(Resources.Load<Material>("Aliments/Textures/" + loadedAliment.name + j));
            }
        }
        else
        {
            //création du material découpable
            Material mat = new Material(Resources.Load<Shader>("OnePlaneBSP"));
            mat.SetTexture("_MainTex", Resources.Load<Texture>("Aliments/Textures/CutedAlimentTexture/" + loadedAliment.name));
            if (mesh.bounds.size.x > mesh.bounds.size.y)
            {//on récupère le plus long axe du mesh et on adapte la plaque
                if (mesh.bounds.size.x > mesh.bounds.size.z)
                {
                    toEat.GetComponent<BlocAliment>().normal = Vector3.right;
                }
                else
                {
                    toEat.GetComponent<BlocAliment>().normal = Vector3.forward;
                }
            }
            else
            {
                if (mesh.bounds.size.y > mesh.bounds.size.z)
                {
                    toEat.GetComponent<BlocAliment>().normal = Vector3.up;
                }
                else
                {
                    toEat.GetComponent<BlocAliment>().normal = Vector3.forward;
                }
            }

            mat.SetVector("_PlaneNormal", toEat.GetComponent<BlocAliment>().normal);

            //toEat.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Aliments/Textures/" + loadedAliment.name);
            toEat.GetComponent<MeshRenderer>().material = mat;

            //GameObject plane = Instantiate(planePrefab, transform);//pour tester, après je vais le créer dynamiquement


            //création plaque

            GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Quad);
            plane.transform.localScale = 3 * new Vector3(1, 1, 0);
            plane.transform.SetParent(toEat.transform);
            plane.transform.localRotation = Quaternion.AngleAxis(90 + 90 * toEat.GetComponent<BlocAliment>().normal.z, new Vector3(toEat.GetComponent<BlocAliment>().normal.y, -toEat.GetComponent<BlocAliment>().normal.x - toEat.GetComponent<BlocAliment>().normal.z, 0));
            mat = new Material(Resources.Load<Shader>("StincelledUnlitTexture"));
            Texture loadedTexture = Resources.Load<Texture>("Aliments/Textures/PlaneTexture/" + loadedAliment.name);
            if (loadedTexture != null)
                mat.SetTexture("_MainTex", loadedTexture);
            plane.GetComponent<MeshRenderer>().material = mat;
            toEat.GetComponent<BlocAliment>().plane = plane;

            //découpe et placement plaque
            //toEat.GetComponent<BlocAliment>().SetVisibleSliceWithFloat(1.0f);
            toEat.GetComponent<BlocAliment>().SetAspectWithSlice();
        }
        return toEat;
    }
}
