using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Firebase.Storage;
using UnityEngine.Assertions;
using Firebase.Auth;

public class CaptureScreenShot : MonoBehaviour
{
    // Firebase storage reference
    FirebaseStorage storage;
    StorageReference storageRef;
    [SerializeField] TextMeshProUGUI txt_InfoAboutScreenShot;
    [SerializeField] int maxScreenShots = 4;
    int currentScreenShot = 0;
    private void Awake()
    {
        storage = FirebaseStorage.DefaultInstance;
        currentScreenShot = PlayerPrefs.GetInt("screenshots", 0);
        //  storageReference = storage.GetReferenceFromUrl("gs://wildlifehorizon-df5dc.appspot.com");

    }
    // Start is called before the first frame update
    void Start()
    {
        // Initialize Firebase storage
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == Firebase.DependencyStatus.Available)
            {
                // Firebase is ready
                storageRef = storage.GetReferenceFromUrl("gs://wildlifehorizon-df5dc.appspot.com");

                Debug.Log("Firebase initialized successfully.");
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {task.Result}");
            }
        });
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            TakeScreenshotAndUpload();

        }
    }
    // Function to take a screenshot and upload to Firebase Storage
    public void TakeScreenshotAndUpload()
    {
        // Capture screenshot
        StartCoroutine(CaptureScreenshot());
    }

    IEnumerator CaptureScreenshot()
    {
        yield return new WaitForEndOfFrame();

        Texture2D screenTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenTexture.Apply();

        // Convert texture to bytes
        byte[] bytes = screenTexture.EncodeToPNG();
        Destroy(screenTexture);

        // Generate a unique file name for the screenshot


        string fileName = $"screenshot_" + currentScreenShot + ".png";
        currentScreenShot = (currentScreenShot + 1) % maxScreenShots;
        PlayerPrefs.SetInt("screenshots", currentScreenShot);
        //        string fileName = $"screenshot_{System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}.png";

        // Upload the screenshot to Firebase Storage
        UploadScreenshot(fileName, bytes);
    }

    void UploadScreenshot(string fileName, byte[] bytes)
    {
        if (storageRef == null)
        {
            Debug.LogError("Firebase storage reference is not initialized.");
            return;
        }
        // Get the current user's UID
        string uid = FirebaseAuth.DefaultInstance.CurrentUser.UserId;

        // Create a reference to the directory for the current user
        StorageReference userStorageRef = storageRef.Child(uid);

        // Create a reference to the file to upload
        StorageReference screenshotRef = userStorageRef.Child(fileName);

        // Upload the file
        screenshotRef.PutBytesAsync(bytes).ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError($"Failed to upload {fileName}: {task.Exception}");
            }
            else
            {
                Debug.Log($"Uploaded {fileName} successfully.");
            }
        });
    }
}
