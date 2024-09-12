using MovieAPIDemo.Server.Entities;
using System.ComponentModel.DataAnnotations;

namespace MovieAPIDemo.Server.Models
{
    public class CreateMovieViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Title of the movie is required.")]
        public string Title { get; set; }
        public string Description { get; set; }
        
        // List of Actors
        public List<int> Actors { get; set; }

        [Required( ErrorMessage = "Language of the movie is required." )]
        public string Language { get; set; }

        public DateTime ReleaseDate { get; set; }
        public string CoverImage { get; set; }
    }
}
