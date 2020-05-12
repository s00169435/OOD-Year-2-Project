using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for ReviewWindow.xaml
    /// </summary>
    public partial class ReviewWindow : Window
    {
        MyMangaLibraryEntities db = null;
        Manga manga = null;
        public ReviewWindow(Manga m, MyMangaLibraryEntities database)
        {
            InitializeComponent();
            manga = m;
            db = database;
            Title = manga.Title;
            imgManga.Source = new BitmapImage(new Uri(manga.SourceImage, UriKind.Relative));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmBxRating.ItemsSource = new int[] {1,2,3,4,5,6,7,8,9,10 };
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //Check to make sure user has enetered in something for all fields
            if (!(String.IsNullOrEmpty(txtBxReview.Text)) && !(String.IsNullOrEmpty(txtBxUserName.Text)) && cmBxRating.SelectedItem != null)
            {
                //creates a new review
                Review r = new Review()
                {
                    MangaId = manga.MangaId,
                    DatePublished = DateTime.Now,
                    Rating = Convert.ToInt32(cmBxRating.SelectedItem),
                    Text = txtBxReview.Text,
                    UserName = txtBxUserName.Text
                };

                //saves changes and closes the window
                db.Reviews.Add(r);
                db.SaveChanges();
                this.Close();
            }
        }
    }
}
