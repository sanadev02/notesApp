using Camera.MAUI;

namespace Notes.Views;

[QueryProperty(nameof(ItemId), nameof(ItemId))]

public partial class NotePage : ContentPage
{
    string _fileName = Path.Combine(FileSystem.AppDataDirectory, "notes.txt");

    public NotePage()
    {
        InitializeComponent();

        string appDataPath = FileSystem.AppDataDirectory;
        string randomFileName = $"{Path.GetRandomFileName()}.notes.txt";        

        LoadNote(Path.Combine(appDataPath, randomFileName));

        if (File.Exists(_fileName))
        {
            TextEditor.Text = File.ReadAllText(_fileName);
        }

    }
    public string ItemId
    {
        set { LoadNote(value); }
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is Models.Note note)
        {
            // Save both name and text to the file
            string content = $"{note.Name}\n{note.Text}";
            File.WriteAllText(note.Filename, content);
        }

        await Shell.Current.GoToAsync("..");
    }
    private async void DeleteButton_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is Models.Note note)
        {
            // Delete the file.
            if (File.Exists(note.Filename)) File.Delete(note.Filename);
        }
        await Shell.Current.GoToAsync("..");
    }


    private void LoadNote(string fileName)
    {
        Models.Note noteModel = new Models.Note();
        noteModel.Filename = fileName;
        if (File.Exists(fileName))
        {
            noteModel.Date = File.GetCreationTime(fileName);
            string[] lines = File.ReadAllLines(fileName);
            if (lines.Length > 0)
            {
                noteModel.Name = lines[0];
                if (lines.Length > 1)
                {
                    noteModel.Text = string.Join("\n", lines.Skip(1));
                }
            }
        }
        BindingContext = noteModel;
    }
}