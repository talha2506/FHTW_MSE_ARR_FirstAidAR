using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAid : MonoBehaviour
{
    private bool isConvulsing = false;
    private RuntimePlatform platform;

    private GameObject startButton;
    private GameObject patient;

    private Vector3 originalPatientPosition;

    // Start is called before the first frame update
    void Start()
    {
        this.platform = Application.platform;
        this.startButton = GameObject.FindWithTag("StartButton");
        this.patient = GameObject.FindWithTag("Patient");
        if (this.patient != null)
        {
            this.originalPatientPosition = this.patient.transform.position;
        }
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

        if (this.startButton != null)
        {
            this.startButton.SetActive(!isConvulsing);
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
                if (!isConvulsing)
                {
                    ToggleConvulsions();
                }
                Debug.Log("Scenario started.");
            }
            else if (hitinfo.collider.tag == "Benzo")
            {
                if (isConvulsing)
                {
                    ToggleConvulsions();

                    if (this.patient != null && this.originalPatientPosition != null)
                    {
                        this.patient.transform.position = this.originalPatientPosition;
                    }
                }
                Debug.Log("Medication was clicked. Patient stopped having convulsions.");
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
        if (this.patient != null)
        {
            Vector3 originalPosition = this.patient.transform.position;

            // Apply small random translation to the x-coordinate of the GameObject's position
            float randomX = Random.Range(-0.25f, +0.25f);
            float randomY = Random.Range(-0.1f, +0.1f);
            float newX = originalPosition.x + randomX;
            float newY = originalPosition.y + randomY;

            // Move the GameObject smoothly towards the new x-coordinate
            Vector3 newPosition = new Vector3(newX, newY, this.patient.transform.position.z);
            this.patient.transform.position = Vector3.Lerp(this.patient.transform.position, newPosition, 3.0f * Time.deltaTime);

            Debug.Log("Patient is having convulsions!");
        }
    }
}
