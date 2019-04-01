using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Scenario{
    public string name;
    public float skyboxRotation;
    public List<string> aliments;
}

[Serializable]
public class ScenarioCollection
{
    public List<Scenario> scenarios = new List<Scenario>();
}