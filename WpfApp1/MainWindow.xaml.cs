using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Creatiing database, random, lists and obseervables
        MyMangaLibraryEntities db = new MyMangaLibraryEntities();
        List<Synopsis> synopsis = new List<Synopsis>();
        List<Manga> allManga = new List<Manga>();
        List<Manga> tempFilter = new List<Manga>();
        ObservableCollection<Manga> filteredManga = new ObservableCollection<Manga>();
        ObservableCollection<Manga> favouriteManga = null;
        Random random = new Random();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Combobox with a list of genres
            List<string> genres = new List<string>
            { "Demons", "Comedy", "Mystery", "Space", "Psychological", "Romance", "Seinen", "Sports", "Drama", "School","Supernatural", "Adventure", "Military", "Shounen", "Action", "Fantasy", "Sci-Fi", "Super Power", "Horror" };
            genres.Sort();
            genres.Insert(0, "All");
            cmbxGenre.ItemsSource = genres;
            cmbxGenre.SelectedItem = "All";

            //Intiliasing synopsis and average rating of each manga 
            string strJsonSynopsis = File.ReadAllText(@"storedSynopsis.json");
            synopsis = JsonConvert.DeserializeObject<List<Synopsis>>(strJsonSynopsis);

            foreach (Manga m in db.Mangas)
            {
                double sum = 0;
                foreach (Review r in m.Reviews)
                {
                    sum += r.Rating;
                    r.DatePublished = CreateRandomDate(random, r);
                }

                double avg = sum / m.Reviews.Count();
                m.Rating = avg;

                m.Synopsis = (from syn in synopsis
                              where syn.MangaId == m.MangaId
                              select syn.Text).ToList().FirstOrDefault();
            }

            //Creating an ordered lists of all the manga in database
            //from which all manga will be retrieved
            allManga = new List<Manga>(
                from m in db.Mangas
                orderby m.Title ascending
                select m).ToList();

            foreach (Manga m in allManga)
                filteredManga.Add(m);

            foreach (Manga m in filteredManga)
                tempFilter.Add(m);
            //Creating list of all favourite manga and finally
            //setting itemsources of relative listboxes
            favouriteManga = new ObservableCollection<Manga>(
                from manga in db.Mangas
                orderby manga.Title ascending
                where manga.Favourite == true
                select manga);


            lbxFavourite.ItemsSource = favouriteManga;
            lbxManga.ItemsSource = filteredManga;
            lbxManga.SelectedIndex = random.Next(0, db.Mangas.Count());
        }

        private void cmbxGenre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedGenre = cmbxGenre.SelectedItem as string;

            //All manga containing selected genre shall be displayed
            if (selectedGenre != null)
            {
                txtBxSearch.Text = "";
                filteredManga.Clear();
                tempFilter.Clear();
                if (selectedGenre == "All")
                {
                    foreach (Manga m in allManga)
                        filteredManga.Add(m);
                }
                else
                {
                    foreach (Manga m in allManga)
                        if (m.Genre.Contains(selectedGenre))
                            filteredManga.Add(m);
                }
                //Copy of main filter to be used later for text change
                foreach (Manga m in filteredManga)
                    tempFilter.Add(m);
            }
        }

        private void TxtBxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Upon text change, displays all manga titles with the same string
            string text = txtBxSearch.Text;
            filteredManga.Clear();
            foreach (Manga m in tempFilter)
                if (m.Title.ToUpper().Contains(text.ToUpper()))
                    filteredManga.Add(m);
        }

        private void TxtBxFaveSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Displays all favourite manga titles with the same string entered
            favouriteManga.Clear();
            string text = txtBxFaveSearch.Text;
            favouriteManga = new ObservableCollection<Manga>( 
                from m in db.Mangas
                orderby m.Title ascending
                where m.Title.Contains(text) && m.Favourite == true
                select m);
            lbxFavourite.ItemsSource = favouriteManga;
        }

        private void lbxManga_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Manga manga = lbxManga.SelectedItem as Manga;
            
            //Upon selection change, all relative data of selected manga
            //is displayed to the user
            if (manga != null)
            {
                grdSelectedManga.DataContext = manga;

                //Stackpanel cleared of current synopsis and updated
                skPnlReviews.Children.Clear();
                skPnlReviews.Children.Add(new Label() { FontSize = 30, Foreground = Brushes.White, Margin = new Thickness(10, 0, 0, 0), Content = "Synopsis" });
                skPnlReviews.Children.Add(new TextBlock() { FontSize = 15, Foreground = Brushes.White, Margin = new Thickness(10, 8, 10, 0), Text = manga.Synopsis, TextWrapping = TextWrapping.Wrap });
                skPnlReviews.Children.Add(new Label() { FontSize = 30, Foreground = Brushes.White, Margin = new Thickness(10, 0, 0, 0), Content = "Reviews" });

                var query = manga.Reviews.OrderByDescending(d => d.DatePublished);

                //Information of each review gets displayed with all its information
                //and added as a child of the review stackpanel
                foreach (Review r in query)
                {
                 
                    //New grid created with columns and row
                    Grid grd = new Grid();
                    grd.ColumnDefinitions.Add(new ColumnDefinition());
                    grd.ColumnDefinitions.Add(new ColumnDefinition());
                    grd.ColumnDefinitions.Add(new ColumnDefinition());
                    grd.RowDefinitions.Add(new RowDefinition());
                    grd.RowDefinitions.Add(new RowDefinition());

                    //Textblocks created 
                    TextBlock text = new TextBlock() { TextWrapping = TextWrapping.Wrap, Background = Brushes.White, Padding = new Thickness(5, 0, 5, 0), Margin = new Thickness(10, 0, 0, 0), Text = r.Text };
                    TextBlock userName = new TextBlock() { TextWrapping = TextWrapping.Wrap, Background = Brushes.White, Foreground = Brushes.Gray, Text = "User: " +  r.UserName };
                    TextBlock datePubl = new TextBlock() { TextWrapping = TextWrapping.Wrap, Background = Brushes.White, Foreground = Brushes.Gray, Text = "Date: " + r.DatePublished.ToString("dd-MM-yyyy") };
                    TextBlock rating = new TextBlock() { TextWrapping = TextWrapping.Wrap, Background = Brushes.White, Foreground = Brushes.Gray, Text = "Rating: " + r.Rating.ToString() };

                    ScrollViewer scrollViewer = new ScrollViewer() { VerticalScrollBarVisibility = ScrollBarVisibility.Auto, Width = 600, Height = 250, Margin = new Thickness(10, 0, 0, 10) };
                    scrollViewer.Content = text;

                    Border border = new Border() { BorderBrush = Brushes.Gray, BorderThickness = new Thickness(0, 1, 0, 0), Padding  = new Thickness(0,10,0,0) };
                    border.Child = grd;

                    Grid.SetRow(userName, 0);
                    Grid.SetColumn(userName, 0);
                    Grid.SetRow(datePubl, 0);
                    Grid.SetColumn(datePubl, 1);
                    Grid.SetRow(rating, 0);
                    Grid.SetColumn(rating, 2);
                    Grid.SetRow(scrollViewer, 1);
                    Grid.SetColumn(scrollViewer, 0);
                    Grid.SetColumnSpan(scrollViewer, 3);

                    grd.Children.Add(userName);
                    grd.Children.Add(datePubl);
                    grd.Children.Add(rating);
                    grd.Children.Add(scrollViewer);

                    skPnlReviews.Children.Add(border);
                }
            }
            
        }

        private void TextBlock_Favourites_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Clicking on text will either add or remove manga from favourites
            TextBlock txtBlkFave = sender as TextBlock;
            Manga manga = txtBlkFave.DataContext as Manga;

            if (manga != null)
            {

                if (favouriteManga.Contains(manga))
                {
                    txtBlkFave.Text = "Add to favourites...";
                    favouriteManga.Remove(manga);
                }
                else
                {
                    txtBlkFave.Text = "Remove from favourites...";
                    favouriteManga.Add(manga);
                }

                manga.Favourite = !manga.Favourite;
                manga.ReadingState = 0;
                db.SaveChanges();
            }
        }

        private void TextBlock_Favourites_PreviewMouseLeftButtonDown2(object sender, MouseButtonEventArgs e)
        {
            //Will remove relative manga from favourites
            TextBlock txtBlkFave = sender as TextBlock;
            Manga manga = txtBlkFave.DataContext as Manga;
            favouriteManga.Remove(manga);
            manga.Favourite = !manga.Favourite;
            db.SaveChanges();
        }

        private void TextBlock_Favourites_Loaded(object sender, RoutedEventArgs e)
        {
            //Textblock text changes based on whether or not manga
            //is added to favouites
            TextBlock txtBlkFave = sender as TextBlock;
            Manga manga = txtBlkFave.DataContext as Manga;
            txtBlkFave.Text =  manga.Favourite ? "Remove from favourites..." : "Add to favourites...";
        }

        private void txtBlkTitle_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Clicking on the title in the favourites tab will navigate to the
            //same manga in the main tab
            TextBlock txtBlk = sender as TextBlock;
            Manga manga = txtBlk.DataContext as Manga;

            lbxManga.SelectedItem = manga;
            mainTabCtrl.SelectedIndex = 0;
        }

        private void cmbxMangaState_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Updates the reading state of the relative manga
            ComboBox cmbx = sender as ComboBox;
            Manga manga = cmbx.DataContext as Manga;
            manga.ReadingState = cmbx.SelectedIndex;
            db.SaveChanges();
        }

        //Create random publish date for review between date of manga and present date
        private DateTime CreateRandomDate(Random randomFactory, Review review)
        {
            int year = randomFactory.Next(review.Manga.DateReleased.Year, DateTime.Now.Year);
            int month = randomFactory.Next(1, 13);
            int day = randomFactory.Next(1, DateTime.DaysInMonth(year, month));
            DateTime randomDate = new DateTime(year, month, day);

            return randomDate;
        }

        //Navigates to a new window to make review for manga
        private void btnReview_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Manga manga = btn.DataContext as Manga;
            ReviewWindow review = new ReviewWindow(manga, db);
            review.Show();
        }
    }
}