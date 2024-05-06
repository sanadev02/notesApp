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

                string localFilePath = Path.Combine(path, photo.FileName);
                //Console.WriteLine("Photo taken.");

                using Stream sourceStream = await photo.OpenReadAsync();
                using FileStream localFileStream = File.OpenWrite(localFilePath);


                await sourceStream.CopyToAsync(localFileStream);
                //Console.WriteLine("Photo Saved to camera roll.");
                await Shell.Current.DisplayAlert("Success ", localFilePath, "ok");
            }
        }
    }

    public async void TakePhoto(object sender, EventArgs e)
    {
        if (MediaPicker.IsCaptureSupported)
        {

            //var result = await FilePicker.PickAsync(new PickOptions
            //{
            //    PickerTitle = "Pick Image Please",
            //    FileTypes = FilePickerFileType.Images
            //});

            //if (result == null)
            //    return;

            //var stream = await result.OpenReadAsync();

            //myImage.Source = ImageSource.FromStream(() => stream);

            FileResult photo = await MediaPicker.Default.PickPhotoAsync();

            string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures);
            if (photo != null)
            {
                // save the file into local storage
                string localFilePath = Path.Combine(path, photo.FileName);

                using Stream sourceStream = await photo.OpenReadAsync();
                using FileStream localFileStream = File.OpenWrite(localFilePath);

                await sourceStream.CopyToAsync(localFileStream);
            }
        }
    }
}
