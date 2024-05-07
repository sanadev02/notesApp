using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Notes.Models;

namespace Notes.Views;

public partial class AllNotesPage : INotifyPropertyChanged
{
    public ICommand DeleteCommand { get; }

    public AllNotesPage()
    {
        InitializeComponent();
        BindingContext = new AllNotes();
        DeleteCommand = new Command<Note>(DeleteNote);
    }

    protected override void OnAppearing()
    {
        ((AllNotes)BindingContext).LoadNotes();
    }

    private async void Add_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(NotePage));
    }

    private void DeleteNote(Note note)
    {
        if (note == null)
            return;

        // Remove the note from the collection
        ((AllNotes)BindingContext).Notes.Remove(note);

        // Delete the note file from storage
        var filePath = Path.Combine(FileSystem.AppDataDirectory, note.Filename);
        if (File.Exists(filePath))
            File.Delete(filePath);
    }

    private async void notesCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count != 0)
        {
            var note = (Note)e.CurrentSelection[0];
            await Shell.Current.GoToAsync($"{nameof(NotePage)}?{nameof(NotePage.ItemId)}={note.Filename}");
            notesCollection.SelectedItem = null;
        }
    }
}