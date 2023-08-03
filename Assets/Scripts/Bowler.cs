using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowler : MonoBehaviour
{
    public static Bowler Instance {get; private set;}

    [SerializeField] private Rigidbody rb;
    [SerializeField] private float runningSpeed;
    [SerializeField] private float runningDeceleration;

    private float xBowlingLine = -2.2f;

    private void Awake() {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x <= xBowlingLine) 
        {
            rb.velocity = Vector3.zero;
            GameManager.Instance.SetHasBowled(true);
        }
    }

    public void Bowl() {
        rb.velocity = new Vector3(-runningSpeed, 0f, 0f);
    }

    public void ResetBowler() 
    {
        transform.position = new Vector3(5f, 1.59f, -1.85f);
    }
}
