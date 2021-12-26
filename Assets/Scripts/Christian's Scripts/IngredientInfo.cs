using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientInfo : MonoBehaviour
{
    public List<string> customTags = new List<string>(); // This variable is necessary to allow multiple tags to identify what type of item an object is.
                                                         // Necessary because Unity only has one tag per object.
    public string customName; // This variable is necessary to identify prefabs, as prefab names for clones are different from the original prefab.
    public bool isInContainer; // This variable is necessary to identify whether an object is inside a container.
}
