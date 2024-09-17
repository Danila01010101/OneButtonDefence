using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adventurer
{
    [Serializable]
    public class Label
    {
        public string labelName;
        public List<string> Replic;
        public List<Answer> Answers;
        public int NextLabel = -1;
    }
}
