using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using MovieAPIDemo.Server.Data;
using MovieAPIDemo.Server.Entities;
using MovieAPIDemo.Server.Models;
using System.Net.Http.Headers;

namespace MovieAPIDemo.Server.Controllers
{
    [Route( "api/[controller]" )]
    [ApiController]
    public class MovieController : ControllerBase
    {

        private readonly MovieDbContext dbContext;
        private readonly IMapper mapper;

        public MovieController( MovieDbContext _Context, IMapper _mapper )
        {
            dbContext = _Context;
            mapper = _mapper;
        }

        [HttpGet]
        public IActionResult Get( int pageIndex = 0, int pageSize = 10 ) 
        {
            BaseResponseModel response = new BaseResponseModel();
            try
            {
                var movieCount = dbContext.Movie.Count( );
                // For the given pageIndex will skip the number of records multiplied by the pageSize and return a list of records appropriate for the pageIndex.
                var movieList = mapper.Map<List<MovieListViewModel>>( dbContext.Movie.Include( x => x.Actors ).Skip( pageIndex * pageSize ).Take( pageSize ) ).ToList( );

                response.Status = true;
                response.Message = "Success";
                response.Data = new { Movies = movieList, Count = movieCount };

                return Ok( response );
            }
            catch(Exception ex)
            {
                // TODO: do logging exceptions
                response.Status = false;
                response.Message = "Something went wrong";

                return BadRequest( response );
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetMovieById( int id )
        {
            BaseResponseModel response = new BaseResponseModel( );
            try
            {
                var movie = dbContext.Movie.Include( x => x.Actors ).Where( x => x.Id == id ).FirstOrDefault( );

                if ( movie == null)
                {
                    response.Status = false;
                    response.Message = "Record does not exist.";
                    return BadRequest( response );
                }

                var movieData = mapper.Map< MovieDetailsViewModel > ( movie );

                response.Status = true;
                response.Message = "Success";
                response.Data = movieData;

                return Ok( response );
            }
            catch(Exception ex)
            {
                // TODO: do logging exceptions
                response.Status = false;
                response.Message = "Something went wrong";

                return BadRequest( response );
            }
        }

        [HttpPost]
        public IActionResult Post( CreateMovieViewModel model )
        {
            BaseResponseModel response = new BaseResponseModel( );

            try
            {
                if(ModelState.IsValid) 
                { 
                    var actors = dbContext.Person.Where( x => model.Actors.Contains( x.Id) ).ToList();

                    if(actors.Count != model.Actors.Count) 
                    { 
                        response.Status = false;
                        response.Message = "Invalid Actor assigned.";
                        return BadRequest( response );
                    }

                    var postedModel = mapper.Map<Movie>( model );
                    postedModel.Actors = actors;

                    dbContext.Movie.Add(postedModel);
                    //Also consider SaveChangesAsync
                    dbContext.SaveChanges();

                    var responseData = mapper.Map<MovieDetailsViewModel>( postedModel );

                    response.Status = true;
                    response.Message = "Created Successfully";
                    response.Data = responseData;

                    return Ok( response );
                }
                else
                {
                    response.Status = false;
                    response.Message = "Validation failed.";
                    response.Data = ModelState;

                    return BadRequest( response );
                }
            }
            catch(Exception ex)
            {
                response.Status = false;
                response.Message = "Something went wrong";

                return BadRequest( response );
            }
        }

        [HttpPut]
        public IActionResult Put( CreateMovieViewModel model )
        {
            BaseResponseModel response = new BaseResponseModel( );

            try
            {
                if(ModelState.IsValid)
                {
                    if( model.Id <= 0)
                    {
                        response.Status = false;
                        response.Message = "Invalid Movie Record.";

                        return BadRequest( response );
                    }

                    var actors = dbContext.Person.Where( x => model.Actors.Contains( x.Id ) ).ToList( );

                    if(actors.Count != model.Actors.Count)
                    {
                        response.Status = false;
                        response.Message = "Invalid Actor assigned.";
                        return BadRequest( response );
                    }

                    var movieDetails = dbContext.Movie.Include( x => x.Actors ).Where( x => x.Id == model.Id ).FirstOrDefault( );

                    if( movieDetails == null ){
                        response.Status = false;
                        response.Message = "Invalid Movie Record.";
                        return BadRequest( response );
                    }

                    movieDetails.CoverImage = model.CoverImage;
                    movieDetails.Description = model.Description;
                    movieDetails.Language = model.Language;
                    movieDetails.ReleaseDate = model.ReleaseDate;
                    movieDetails.Title = model.Title;

                    // Find removed actor
                    var removedActors = movieDetails.Actors.Where( x => !model.Actors.Contains( x.Id ) ).ToList( );

                    foreach( var actor in removedActors)
                    {
                        movieDetails.Actors.Remove( actor );
                    }

                    // find added actors
                    var addedActors = actors.Except( movieDetails.Actors ).ToList( );
                    foreach( var actor in addedActors)
                    {
                        movieDetails.Actors.Add( actor );
                    }

                    dbContext.SaveChanges();

                    //var postedModel = new Movie( )
                    //{
                    //    Title = model.Title,
                    //    ReleaseDate = model.ReleaseDate,
                    //    Language = model.Language,
                    //    CoverImage = model.CoverImage,
                    //    Description = model.Description,
                    //    Actors = actors
                    //};

                    //dbContext.Movie.Add( postedModel );
                    ////Also consider SaveChangesAsync
                    //dbContext.SaveChanges( );

                    var responseData = new MovieDetailsViewModel
                    {
                        Id = movieDetails.Id,
                        Title = movieDetails.Title,
                        Actors = movieDetails.Actors.Select( y => new ActorViewModel
                        {
                            Id = y.Id,
                            Name = y.Name,
                            DateOfBirth = y.DateOfBirth,
                        } ).ToList( ),
                        CoverImage = movieDetails.CoverImage,
                        Language = movieDetails.Language,
                        ReleaseDate = movieDetails.ReleaseDate,
                        Description = movieDetails.Description
                    };

                    response.Status = true;
                    response.Message = "Updated Successfully";
                    response.Data = responseData;

                    return Ok( response );
                }
                else
                {
                    response.Status = false;
                    response.Message = "Validation failed.";
                    response.Data = ModelState;

                    return BadRequest( response );
                }
            }
            catch(Exception ex)
            {
                response.Status = false;
                response.Message = "Something went wrong";

                return BadRequest( response );
            }
        }

        [HttpDelete]
        public IActionResult Delete( int id )
        {
            BaseResponseModel response = new BaseResponseModel();

            try
            {
                var movie = dbContext.Movie.Where( x => x.Id == id ).FirstOrDefault( );
                if( movie == null)
                {
                    response.Status = false;
                    response.Message = "Record does not exist.";
                    return BadRequest( response );
                }

                dbContext.Movie.Remove( movie );
                dbContext.SaveChanges();

                response.Status = true;
                response.Message = "Deleted successfully";
                
                return Ok( response );
            }
            catch(Exception ex)
            {
                response.Status = false;
                response.Message = "Something went wrong";

                return BadRequest( response );
            }
        }

        [HttpPost]
        [Route( "upload-movie-poster" )]
        public async Task<IActionResult> UploadMoviePoster( IFormFile imageFile )
        {
            try
            {
                var fileName = ContentDispositionHeaderValue.Parse( imageFile.ContentDisposition ).FileName.TrimStart( '\"' ).TrimEnd( '\"' );
                string newPath = @"E:\to-delete";

                if( !Directory.Exists( newPath ))
                {
                    Directory.CreateDirectory( newPath );
                }

                string[ ] allowedImageExtensions = new string[ ] { ".jpg", ".jpeg", ".png" };

                if(!allowedImageExtensions.Contains( Path.GetExtension( fileName ) )) 
                {
                    return BadRequest( new BaseResponseModel
                    {
                        Status = false,
                        Message = "Only .jpg, .jpeg and .png type files are allowed.",
                    } );
                }

                string newFileName = Guid.NewGuid( ) + Path.GetExtension( fileName );
                string fullFilePath = Path.Combine( newPath, newFileName );

                using(var stream = new FileStream( fullFilePath, FileMode.Create ))
                {
                    await imageFile.CopyToAsync( stream );
                }

                return Ok( new { ProfileImage = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/StaticFiles/{newFileName}" } );
            }
            catch(Exception ex) 
            {
                return BadRequest( new BaseResponseModel
                {
                    Status = false,
                    Message = "Error Occured"
                });
            }
        }
    }
}
