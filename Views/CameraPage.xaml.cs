using System.Reflection.Metadata;
using Camera.MAUI;

namespace Notes.Views;

public partial class CameraPage : ContentPage
{
    public string GalleryFolder { get; private set; }

    public CameraPage()
	{
		InitializeComponent();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {

        if (MediaPicker.Default.IsCaptureSupported)
        {
            FileResult photo = await MediaPicker.Default.CapturePhotoAsync();
            Console.WriteLine("camera working");

            if (photo != null)
            {
                // save the file into local storage
                string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures);

                string localFilePath = Path.Combine(FileSystem.Current.AppDataDirectory, photo.FileName);
                //Console.WriteLine("Photo taken.");

                using Stream sourceStream = await photo.OpenReadAsync();
                using FileStream localFileStream = File.OpenWrite(localFilePath);


                await sourceStream.CopyToAsync(localFileStream);
                //Console.WriteLine("Photo Saved to camera roll.");
                await Shell.Current.DisplayAlert("Success ", localFilePath, "ok");
            }
        }
    }

    //private void LoadImages()
    //{
    //    string folderPath = FileSystem.Current.AppDataDirectory;

    //    if (Directory.Exists(folderPath))
    //    {
    //        var imageFiles = Directory.GetFiles(folderPath)
    //            .Where(file => file.EndsWith(".jpg") || file.EndsWith(".png"))
    //            .ToList();

    //        //ImageCollection.ItemsSource = imageFiles;
    //    }
    //}
    //private async void OnTakePhotoClicked(object sender, EventArgs e)
    //{
    //    if (Microsoft.Maui.Media.MediaPicker.Default.IsCaptureSupported)
    //    {
    //        Microsoft.Maui.Storage.FileResult photo = await Microsoft.Maui.Media.MediaPicker.Default.CapturePhotoAsync();
    //        if (photo != null)
    //        {
    //            // Save the file to the app's data directory
    //            try
    //            {
    //                string localFilePath = Path.Combine(FileSystem.Current.AppDataDirectory, photo.FileName);
    //                using Stream sourceStream = await photo.OpenReadAsync();
    //                using FileStream localFileStream = File.OpenWrite(localFilePath);
    //                await sourceStream.CopyToAsync(localFileStream);

    //                // Display a confirmation message
    //                await DisplayAlert("Photo Taken", "Task Completed successfully. Mission Complete. Return to Game Select", "OK");
    //            }
    //            catch (Exception ex)
    //            {
    //                // Handle any exceptions that occur while saving the photo
    //                await DisplayAlert("Error", $"An error occurred while saving the photo: {ex.Message}", "OK");
    //            }
    //        }
    //        else
    //        {
    //            // Display a message if photo capture was canceled
    //            await DisplayAlert("Photo Capture Canceled or an issue occurred", "Maybe you canceled the photo capture.", "Choose option");
    //        }
    //    }
    //}

    public async void TakePhoto(object sender, EventArgs e)
    {
        if (MediaPicker.IsCaptureSupported)
        { 
            FileResult photo = await MediaPicker.Default.PickPhotoAsync();

            string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures);
            if (photo != null)
            {
                // save the file into local storage
                string localFilePath = Path.Combine(FileSystem.Current.AppDataDirectory, photo.FileName);

                using Stream sourceStream = await photo.OpenReadAsync();
                using FileStream localFileStream = File.OpenWrite(localFilePath);

                await sourceStream.CopyToAsync(localFileStream);
            }
        }
    }
}
