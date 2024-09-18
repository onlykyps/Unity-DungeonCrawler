using UnityEngine;

public class ClickToMove : MonoBehaviour
{
    private Vector3 position;
    public float speed;
    public CharacterController playerControl;
    public static bool attack;
    public static bool die;

    public AnimationClip run;
    public AnimationClip idle;

    public static Vector3 cursorPosition;
    public static bool busy;
    public static Vector3 currentPosition;

    void Start()
    {
        transform.position = DataBase.ReadPlayerPosition();
        position = transform.position;
        busy = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!busy)
        {
            LocateCursorPosition();
            if (!attack && !die)
            {
                if (Input.GetMouseButton(0))
                {
                    //Locate where the player clicked on the terrain
                    LocatePosition();
                }

                //move the player to the position
                MoveToPosition();
            }
        }

        currentPosition = transform.position;
    }



    void LocatePosition()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1000))
        {
            if (hit.collider.tag != "Player" && hit.collider.tag != "Enemy")
            {
                position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                //Debug.Log(position);
            }

        }
    }

    void LocateCursorPosition()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1000))
        {
            cursorPosition = hit.point;

        }
    }

    void MoveToPosition()
    {
        //when gameObject is moving
        if (Vector3.Distance(transform.position, position) > 1)
        {
            Quaternion newRotation = Quaternion.LookRotation(position - transform.position);
            newRotation.x = 0f;
            newRotation.z = 0f;
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 10);
            playerControl.SimpleMove(transform.forward * speed);
            GetComponent<Animation>().CrossFade("run");
        }
        //when gameObject is not moving
        else
        {
            GetComponent<Animation>().CrossFade("idle");
        }

    }
}
