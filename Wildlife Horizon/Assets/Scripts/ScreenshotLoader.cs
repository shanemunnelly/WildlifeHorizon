using Firebase.Auth;
using Firebase.Storage;
using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class FirebaseScreenshotLoader : MonoBehaviour
{
    [SerializeField] RawImage[] screenshotDisplay;

    private FirebaseStorage storage;

    private void Awake()
    {
        storage = FirebaseStorage.DefaultInstance;
    }
    private void OnEnable()
    {
        // Initialize Firebase storage

        // Load screenshot
        for (int i = 0; i < 4; i++)
            LoadScreenshot(i);
    }

    private async void LoadScreenshot(int ind)
    {
        // Path to your screenshot in Firebase Storage
        string uid = FirebaseAuth.DefaultInstance.CurrentUser.UserId;

        string screenshotPath = "gs://wildlifehorizon-df5dc.appspot.com/" + uid + "/screenshot_" + ind + ".png";
        print(screenshotPath);
        try
        {
            // Create a reference to the screenshot in Firebase Storage
            StorageReference storageRef = storage.GetReference(screenshotPath);

            // Download the screenshot as a stream
            Stream stream = await storageRef.GetStreamAsync();

            // Load the stream into a texture
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(ReadFully(stream));

            // Display the screenshot.

            screenshotDisplay[ind].texture = texture;
        }
        catch (Exception e)
        {
            Debug.LogError("Error loading screenshot: " + e.Message);
        }
    }

    // Helper function to read stream fully
    private static byte[] ReadFully(Stream input)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            input.CopyTo(ms);
            return ms.ToArray();
        }
    }
}
