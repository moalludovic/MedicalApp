using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Profile{
    public string firstName = "Jean";
    public string lastName = "Duval";
    public int autoEval = 10;
}

[Serializable]
public class ProfileCollection
{
    public List<Profile> profiles = new List<Profile>();
}