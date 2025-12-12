using System;
using System.Collections.Generic;
using UnityEngine;

namespace Adventurer
{
    [CreateAssetMenu(fileName = "New DialogueData", menuName = "Data/Dialogue Data", order = 51)]
    public class DialogueData : ScriptableObject
    {
        [SerializeField] private List<Label> _label;
        [SerializeField] private string _name;
        public List<Label> Label
        { get { return _label;} }
        public string Name
        { get { return _name; } }

    }
}
