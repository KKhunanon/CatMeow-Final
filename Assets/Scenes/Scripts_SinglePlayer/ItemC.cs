using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemC : NormalFood_SG
{
    public int duration = 8;

    protected override void Eat()
    {
        FindObjectOfType<GameManager_SG>().ItemCCEaten(this);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Cat"))
        {
            Eat();
        }
    }
}
