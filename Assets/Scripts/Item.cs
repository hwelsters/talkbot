using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private PlayerAgent.Action item;

    public PlayerAgent.Action GetItem() { return this.item; }
}
