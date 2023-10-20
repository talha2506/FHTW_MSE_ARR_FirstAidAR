using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAid : MonoBehaviour
{
    [SerializeField]
    private GameObject patient;

    private float moveSpeed = 1f; // Adjust the speed of the convulsions as needed
    private float moveIntensity = 1f; // Adjust the intensity of the convulsions as needed

    private bool isConvulsing = false;
    private RuntimePlatform platform;

    private Vector3 originalPosition;

    // Start is called before the first frame update
    void Start()
    {
        this.platform = Application.platform;
        this.originalPosition = patient.transform.position;
    }

    // Update is called once per frame
    private void Update()
    {

        if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer)
        {
            if (Input.touchCount > 0 && Input.touchCount < 2)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    CheckTouch(Input.GetTouch(0).position);
                }
            }
        }
        else if (platform == RuntimePlatform.WindowsEditor || platform == RuntimePlatform.OSXEditor)
        {
            if (Input.GetMouseButtonDown(0))
            {
                CheckTouch(Input.mousePosition);
            }
        }
    }

    private void FixedUpdate()
    {
        if (isConvulsing)
        {
            StartConvulsions();
        }
    }

    private void CheckTouch(Vector3 pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit hitinfo;
        if (Physics.Raycast(ray, out hitinfo))
        {
            if (hitinfo.collider.tag == "StartButton")
            {
                ToggleConvulsions();
                Debug.Log("Scenario started.");
            }
            else if (hitinfo.collider.tag == "Benzo")
            {
                // Do something
                Debug.Log("Medication was clicked.");
            }
            else if (hitinfo.collider.tag == "Patient")
            {
                // Do something
                Debug.Log("Patient was clicked.");
            }
            else
            {
                Debug.Log($"Game object ({hitinfo.transform.gameObject.name}) was clicked!");
            }
        }
    }

    private void ToggleConvulsions()
    {
        this.isConvulsing = !this.isConvulsing;
    }

    private void StartConvulsions()
    {
        float x = patient.transform.position.x * Mathf.Sin(Time.time * moveSpeed) * moveIntensity;
        float y = patient.transform.position.y;
        float z = patient.transform.position.z;
        patient.transform.position = new Vector3(x, y, z);
        Debug.Log("Patient is having convulsions!");
    }
}
