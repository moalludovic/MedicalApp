using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Aliment{
    public string name;
    public List<string> types;
    public float portions;
    public int slices;

    public string mealType1;
    public string mealType2;
    public int mealTypeThreshold;

    public bool greasy;
    public bool sugary;
    public bool hot;
    public bool cold;

    public bool multiMesh;
}

[Serializable] 
public class AlimentCollection
{
    public List<Aliment> aliments;
}