using UnityEngine;

public class PortalManager : MonoBehaviour
{
    [SerializeField]
    private GameObject playerPrefab;

    [SerializeField]
    private GameObject bluePortal;
    [SerializeField]
    private GameObject orangePortal;

    [SerializeField]
    private Transform blueSpawnPoint;
    [SerializeField]
    private Transform orangeSpawnPoint;

    private Controller2D controller2D;

    private float enterDirection;
    private float exitDirection;

    void Start()
    {
    }

    private void Update()
    {
        //Debug.Log("entered direction: " + enterDirection + " exit direction: " + exitDirection);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        controller2D = GameObject.FindGameObjectWithTag("Player").GetComponent<Controller2D>();

        enterDirection = controller2D.info.faceDirection;

        if (collision.gameObject.name == "Player")
        {
            //if player enters blue portal
            if (gameObject.name == "Blue Portal")
            {
                //disable orange collider
                DisableCollider(orangePortal.GetComponent<Collider2D>());

                //create clone at orange portal
                CreateClone(orangeSpawnPoint.position);
            }
            //if player enters orange portal
            else if (gameObject.name == "Orange Portal")
            {
                //disable blue collider
                DisableCollider(bluePortal.GetComponent<Collider2D>());

                //create clone at blue portal
                CreateClone(blueSpawnPoint.position);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        exitDirection = controller2D.info.faceDirection;

        //if player exit portal without teleporting
        if (enterDirection != exitDirection)
        {
            Destroy(GameObject.Find("Clone"));
        }
        //if player teleports with portal
        else
        {
            Destroy(collision.gameObject);
            EnableColliders();
            GameObject.Find("Clone").name = "Player";
        }
    }

    public void CreateClone(Vector3 spawnPoint)
    {
        var clone = Instantiate(playerPrefab, spawnPoint, Quaternion.identity);
        clone.name = "Clone";
    }

    public void DisableCollider(Collider2D collider)
    {
        collider.enabled = false;
    }

    public void EnableColliders()
    {
        bluePortal.GetComponent<Collider2D>().enabled = true;
        orangePortal.GetComponent<Collider2D>().enabled = true;
    }

}
