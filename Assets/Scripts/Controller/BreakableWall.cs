using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BreakableWall : MonoBehaviour
{
    [SerializeField] private int HP = 10;
    private TilemapCollider2D _collider;
    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<TilemapCollider2D>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Attack")
        {
            --HP;
            if (HP == 5)
            {

            }
            else if (HP <= 0)
            {
                
            }
            else
            {

            }
        }
    }
}
