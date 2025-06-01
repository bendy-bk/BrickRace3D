
using UnityEngine;

public class BrickStair : BrickBase
{
    [SerializeField] private GameObject wall;

    public GameObject Wall { get => wall; set => wall = value; }

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
       
        //if (other.CompareTag("Player"))
        //{
        //    PlayerController p = other.GetComponent<PlayerController>();
        //    ChagerColorType(p.ColorType);
        //}
        //else if (other.CompareTag("Enemy"))
        //{
        //    Enemy e = other.GetComponent<Enemy>();

        //    ChagerColorType(e.ColorType);
        //}
    }
}

