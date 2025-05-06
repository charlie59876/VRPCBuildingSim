using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ComponentSnapper : MonoBehaviour
{

    [SerializeField] private string componentTag = "Null";
    [SerializeField] private Transform componentSnapPosition;
    [SerializeField] private float rotationLeniance = 20f;
    [SerializeField] private float positionLeniance = .2f;
    [SerializeField] private int checkID = 0;
    [SerializeField] private bool snapSelf = false;
    [SerializeField] private int[] requiredChecks;
    [SerializeField] private Clips[] allClips;
    private bool inUse = false;
    //[SerializeField] private int[] requiredChecks;
    private MenuHandler menuHandler;

    [System.Serializable]
    public class Clips
    {
        public GameObject clip;
        public Transform clippedLocation;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menuHandler = GameObject.FindGameObjectWithTag("MenuHandler").GetComponent<MenuHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Prevents you putting multiple components in one slot, such as in the case of RAM where there's multiple slots and components
        if (inUse)
            return;
        for (int i = 0; i < requiredChecks.Length; i++)
        {
            //Debug.Log("number " + i);
            //Debug.Log(requiredChecks.Length);
            //Debug.Log(requiredChecks[i]);
            if (!menuHandler.GetCheck(requiredChecks[i]))
                return;
        }


        if (other.CompareTag(componentTag))
        {

            //Debug.Log("Attempting to snap " + other + " at " + other.transform.parent.position + " to " + this + " at " + componentSnapPosition.position + " (Distance: " + Vector3.Distance(other.transform.parent.position, componentSnapPosition.position) + ")");
            //Debug.Log(other + " Rotation: " + other.transform.parent.rotation + " " + this + " Rotation: " + componentSnapPosition.rotation + " Distance " + Quaternion.Angle(other.transform.parent.rotation, componentSnapPosition.rotation));
            //Debug.Log(positionLeniance + " " + rotationLeniance);
            Debug.Log(other.transform.parent + " " + other.tag + " " + other.gameObject);

            //Makes sure the position and rotation of the componend lines up with the port, so they dont auto snap by just touching them together
            //Setting either value to 0 will ignore that value
            if ((Vector3.Distance(other.transform.parent.position, componentSnapPosition.position) <= positionLeniance || positionLeniance == 0) 
                && (Quaternion.Angle(other.transform.parent.rotation, componentSnapPosition.rotation) <= rotationLeniance || rotationLeniance == 0))
            {
                Debug.Log(other + " component snapped to " + this);

                inUse = true;

                menuHandler.SetCheck(checkID);

                if (!snapSelf)
                {
                    Destroy(other.transform.parent.gameObject.GetComponent<XRGrabInteractable>());
                    Destroy(other.transform.parent.gameObject.GetComponent<Rigidbody>());
                    Destroy(other.transform.parent.gameObject.GetComponent<Collider>());

                    other.transform.parent.parent = componentSnapPosition;

                    other.transform.parent.position = componentSnapPosition.position;
                    other.transform.parent.rotation = componentSnapPosition.rotation;
                }
                else
                {
                    transform.parent = componentSnapPosition;

                    transform.position = componentSnapPosition.position;
                    transform.rotation = componentSnapPosition.rotation;
                }

                for(int i = 0; i < allClips.Length; i++)
                {
                    allClips[i].clip.transform.position = allClips[i].clippedLocation.position;
                    allClips[i].clip.transform.rotation = allClips[i].clippedLocation.rotation;
                }

                //Debug.Log(componentSnapPosition.rotation + " current: " + other.transform.rotation);
            }
        }
    }

}
