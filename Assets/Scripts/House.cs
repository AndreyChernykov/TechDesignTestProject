using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;
    BoxCollider2D boxCollider;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnMouseDown()
    {
        if (playerManager.isAlive)
        {
            boxCollider.enabled = false;
            playerManager.Run();
        }
    }
}
