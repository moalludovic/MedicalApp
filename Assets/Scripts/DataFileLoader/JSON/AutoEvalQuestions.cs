using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class AutoEvalQuestion
{
    public string Text = "";
    public List<string> Choices = new List<string>();
    public List<string> Answers = new List<string>();
}

[Serializable]
public class AutoEvalQuestions
{
    public List<AutoEvalQuestion> questions = new List<AutoEvalQuestion>();
}