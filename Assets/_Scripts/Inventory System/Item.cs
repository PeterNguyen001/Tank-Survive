using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Attribute which allows right click->Create
[CreateAssetMenu(fileName = "New Item", menuName = "Items/New Item")]
public class Item : ScriptableObject //Extending SO allows us to have an object which exists in the project, not in the scene
{
    public Sprite icon;
    [TextArea]
    public string description = "";
    public int recipeGridSize = 9;
    public bool isConsumable = false;
    public bool isCraftable = false;
    public string ID = " ";// use for crafting
    public List<Item> recipe;
    public int recipeMutiplier= 1;// a mutiplier apply to the ammount of this item you get from crafting
    public ScriptableObject tankPart;

    public void Use()
    {
        Debug.Log("This is the Use() function of item: " + name + " - " + description);
    }

    public void Init()
    {
        icon = tankPart.GetComponent<Sprite>();
    }

}
