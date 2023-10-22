using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAid : MonoBehaviour
{
    private float moveSpeed = 1f; // Adjust the speed of the convulsions as needed
    private float moveIntensity = 1f; // Adjust the intensity of the convulsions as needed

    private bool isConvulsing = false;
    private RuntimePlatform platform;

    private GameObject startButton;
    private GameObject patient;

    // Start is called before the first frame update
    void Start()
    {
        this.platform = Application.platform;
        this.startButton = GameObject.FindWithTag("StartButton");
        this.patient = GameObject.FindWithTag("Patient");
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
                }
                Debug.Log("Medication was clicked. Patient stopped having convulsions.");
            }
            else if (hitinfo.collider.tag == "Patient")
            {
                if (isConvulsing)
                {
                    // Schlechte Vitalparameter einblenden
                }
                else
                {
                    // Gute Vitalparameter einblenden
                }
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
        if (this.patient != null)
        {
            Vector3 originalPosition = this.patient.transform.position;
            Vector3 randomTranslation = Random.insideUnitSphere * moveIntensity;
            Vector3 newPosition = originalPosition + randomTranslation;

            // Move the GameObject smoothly towards the new position
            this.patient.transform.position = Vector3.Lerp(transform.position, newPosition, moveSpeed * Time.deltaTime);

            Debug.Log($"Moving from {originalPosition} to {this.patient.transform.position}");
            Debug.Log("Patient is having convulsions!");
        }
    }
}
