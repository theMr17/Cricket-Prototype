using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    [SerializeField] private GameObject striker;
    [SerializeField] private GameObject nonStriker;
    [SerializeField] private float runningSpeed;
    [SerializeField] private GameObject runsContainer;
    [SerializeField] private TMP_Text runsText;

    private bool canBowl = true;
    private bool hasBowled = false;
    private int runs = -1;
    private float xNonStrikerLine = -2.2f;
    private float xStrikerLine = -15.2f;
    private Vector3 strikerPos;
    private Vector3 nonStrikerPos;

    private void Awake() {
        Instance = this;
        strikerPos = striker.transform.position;
        nonStrikerPos = nonStriker.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && canBowl)
        {
            Bowler.Instance.Bowl();
            canBowl = false;
        }

        if(hasBowled && runs == -1)
        {
            GetRun();
            AnimateRun();
        }

        if(striker.transform.position.x >= xNonStrikerLine) 
        {
            striker.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        if(nonStriker.transform.position.x <= xStrikerLine) 
        {
            nonStriker.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        if(runs != -1 && striker.GetComponent<Rigidbody>().velocity.magnitude == 0 && nonStriker.GetComponent<Rigidbody>().velocity.magnitude == 0)
        {
            GameObject temp = striker;
            striker = nonStriker;
            nonStriker = temp;

            if(runs > 0) {
                Run();
            } else {
                ResetDelivery();
            }
        }
    }

    private void GetRun()
    {
        runs = Random.Range(0, 6+1);
        runsText.text = runs.ToString();
    }

    private void AnimateRun() 
    {
        switch (runs)
        {
            case 1: case 2: case 3:
                Run();
                break;

            default: 
                ResetDelivery();
                break;
        }
    }

    private void Run() 
    {
        striker.GetComponent<Rigidbody>().velocity = new Vector3(runningSpeed, 0f, 0f);
        nonStriker.GetComponent<Rigidbody>().velocity = new Vector3(-runningSpeed, 0f, 0f);

        runs--;
    }

    private void ResetDelivery() 
    {
        runs = -1;
        hasBowled = false;

        striker.transform.position = strikerPos;
        nonStriker.transform.position = nonStrikerPos;
        Bowler.Instance.ResetBowler();

        DisplayRuns();
    }

    private void DisplayRuns() 
    {
        runsContainer.SetActive(true);
        StartCoroutine(WaitForFunction());
    }

    IEnumerator WaitForFunction()
    {
        yield return new WaitForSeconds(2);
        runsContainer.SetActive(false);
        canBowl = true;
    }

    public void SetHasBowled(bool hasBowled)
    {
        this.hasBowled = hasBowled;
    }
}
