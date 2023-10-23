using UnityEngine;

public class FirstAid : MonoBehaviour
{
    private bool isConvulsing = false;
    private RuntimePlatform platform;

    private GameObject startButton;
    private GameObject bed;
    private GameObject patient;

    private AudioSource audioSource = null;
    [SerializeField]
    private AudioClip clip = null;

    // Start is called before the first frame update
    void Start()
    {
        this.platform = Application.platform;
        this.startButton = GameObject.FindWithTag("StartButton");
        this.bed = GameObject.FindWithTag("Bed");
        this.patient = this.gameObject;
        this.audioSource = FindFirstObjectByType<AudioSource>();
        if (this.audioSource == null)
        {
            Debug.LogWarning("Audio source is null!");
        }
        else
        {
            this.audioSource.clip = this.clip;
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

                    if (this.bed != null)
                    {
                        this.patient.transform.position = new Vector3(this.bed.transform.position.x, this.bed.transform.position.y + 0.4f, this.bed.transform.position.z - 0.4f);
                    }

                    if (this.audioSource != null)
                    {
                        this.audioSource.Stop();
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
            if (this.audioSource != null && !this.audioSource.isPlaying)
            {
                this.audioSource.Play();
            }

            Vector3 originalPosition = this.patient.transform.position;

            // Apply small random translation to the x-coordinate of the GameObject's position
            float randomX = Random.Range(-0.05f, +0.05f);
            float randomY = Random.Range(-0.005f, +0.005f);
            float newX = originalPosition.x + randomX;
            float newY = originalPosition.y + randomY;

            // Move the GameObject smoothly towards the new x-coordinate
            Vector3 newPosition = new Vector3(newX, newY, this.patient.transform.position.z);
            this.patient.transform.position = Vector3.Lerp(this.patient.transform.position, newPosition, 3.0f * Time.deltaTime);

            Debug.Log("Patient is having convulsions!");
        }
    }
}
