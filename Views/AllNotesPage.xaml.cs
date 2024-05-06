using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Notes.Models;

namespace Notes.Views;

public partial class AllNotesPage : INotifyPropertyChanged
{
     ObservableCollection<Note> Notes { get; set; }

    public ICommand DeleteCommand => new Command<Note>(DeleteSwipe);

    public AllNotesPage()
	{
		InitializeComponent();
        CreateSwipeView();
        BindingContext = new AllNotes();
        Notes = new ObservableCollection<Note>();
        //DeleteCommand = new Command<Note>(DeleteSwipe);

    }
    //public ICommand DeleteCommand { get; private set; }

    //private void DeleteNote(Note note)
    //{
    //    Console.WriteLine("is it deleted?");

    //    if (note != null && Notes.Contains(note))
    //    {
    //        // Delete the file
    //        if (File.Exists(note.Filename))
    //            File.Delete(note.Filename);

    //        // Remove the note from the ObservableCollection
    //        Notes.Remove(note);
    //    }
    //}

    protected override void OnAppearing()
    {
        ((AllNotes)BindingContext).LoadNotes();
    }

    private async void Add_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(NotePage));
    }

    private void CreateSwipeView()
    {

        SwipeItem deleteSwipeItem = new SwipeItem
        {
            Text = "Delete",
            BackgroundColor = Colors.Red,
        };
        deleteSwipeItem.Invoked += OnDeleteSwipeItemInvoked;

        List<SwipeItem> swipeItems = new List<SwipeItem>() { deleteSwipeItem };

        // SwipeView content
        Grid grid = new Grid
        {
            HeightRequest = 60,
            WidthRequest = 300,
            BackgroundColor = Colors.LightGray
        };
        grid.Add(new Label
        {
            Text = "Swipe right",
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        });

        SwipeView = new SwipeView
        {
            LeftItems = new SwipeItems(swipeItems),
            Content = grid
        };
    }

    async void OnDeleteSwipeItemInvoked(object sender, EventArgs e, Note note)
    {
        Console.WriteLine("is it deleted?");

        //if (File.Exists(note.Filename)) File.Delete(note.Filename);
        //Console.WriteLine("is it working?");

        if (Notes.Contains(note))
        {
            Console.WriteLine("deleted????");
            Notes.Remove(note);
        }
        Console.WriteLine("deleted");
        //if (BindingContext is Note note)
        //{
        //    System.Diagnostics.Debug.WriteLine("is it doing something?");

        //    // Delete the file.
        //    if (File.Exists(note.Filename)) File.Delete(note.Filename);
        //    Console.WriteLine("is it working?");

        //    // Remove the note from the ObservableCollection
        //    if (Notes.Contains(note))
        //    {
        //        Console.WriteLine("deleted????");
        //        Notes.Remove(note);
        //    }
        //    Console.WriteLine("deleted");
        //}
        await Shell.Current.GoToAsync("..");
    }

    string _fileName = Path.Combine(FileSystem.AppDataDirectory, "notes.txt");


    public ICommand Delete_clicked => new Command((obj) =>
    {
        if (obj is Note note && Notes.Contains(note))
        {
            Notes.Remove(note);
        }
    });


    void DeleteSwipe(Note note)
    {
        if (Notes.Contains(note))
        {
            Notes.Remove(note);
        }
    }

    private SwipeView _swipeView;

    public SwipeView SwipeView
    {
        get => _swipeView;
        set
        {
            _swipeView = value;
            OnPropertyChanged(nameof(SwipeView));
        }
    }


    private async void notesCollection_SelectionChanged(object sender,
    SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count != 0)
        {
            // Get the note model
            var note = (Note)e.CurrentSelection[0];

            // Should navigate to "NotePage?ItemId=path\on\device\XYZ.notes.txt"
            await Shell.Current.GoToAsync($"{nameof(NotePage)}?{nameof(NotePage.
                ItemId)}={ note.Filename}");

            // Unselect the UI
            notesCollection.SelectedItem = null ;
        }
    }


    private void VibrateStartButton_Clicked(object sender, EventArgs e)
    {
        double secondsToVibrate = 0.2;
        TimeSpan vibrationLength = TimeSpan.FromSeconds(secondsToVibrate);

        Vibration.Default.Vibrate(vibrationLength);
        Console.WriteLine("vibrating");

    }

    private void VibrateStopButton_Clicked(object sender, EventArgs e) =>
        Vibration.Default.Cancel();

}
