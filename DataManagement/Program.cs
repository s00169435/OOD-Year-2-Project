using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1;

namespace DataManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            MangaData db = new MangaData();

            using (db)
            {
                Manga m1 = new Manga()
                {
                    MangaId = 9,
                    Title = "Attack on Titan",
                    Author = "Hajime Isayami",
                    SourceImage = @"Covers\AoT.png",
                    Volumes = 30,
                    Genre = "Action, Mystery, Drama, Fantasy, Horror, Shounen, Super Power, Supernatural" 
                };

                Manga m2 = new Manga()
                {
                    MangaId = 10,
                    Title = "Code Geass",
                    Author = "Gorou Taniguchi (Story), Okouchi, Ichiro",
                    SourceImage = @"Covers\CodeGeass.jpg",
                    Volumes = 8,
                    Genre = "Action, Drama, School, Sci-Fi, Supernatural, Military"
                };

                Review r1 = new Review()
                {
                    ReviewId = 1,
                    Text = "Really good manga. Story and histroy of the world was very captivating.",
                    MangaId = 9,
                    Manga = m1
                };

                Review r2 = new Review()
                {
                    ReviewId = 2,
                    Text = "Very different from the anime adaptation. The source material felt a bit \"meh\" in comparison.",
                    MangaId = 10,
                    Manga = m2
                };

                db.Mangas.Add(m1);
                db.Mangas.Add(m2);

                Console.WriteLine("Added mangas to database");

                db.Reviews.Add(r1);
                db.Reviews.Add(r2);

                Console.WriteLine("Added reviews to database");

                db.SaveChanges();

                Console.WriteLine("Database updated");
            }
        }
    }
}
