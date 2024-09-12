using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAPIDemo.Server.Data;
using MovieAPIDemo.Server.Entities;
using MovieAPIDemo.Server.Models;
using System.Collections.Generic;
using System.Security.Cryptography.Xml;

namespace MovieAPIDemo.Server.Controllers
{
    [Route( "api/[controller]" )]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly MovieDbContext dbContext;
        private readonly IMapper mapper;

        public PersonController( MovieDbContext _Context, IMapper _mapper )
        {
            dbContext = _Context;
            mapper = _mapper;
        }

        [HttpGet]
        public IActionResult Get( int pageIndex = 0, int pageSize = 10 )
        {
            BaseResponseModel response = new BaseResponseModel( );
            try
            {
                var actorCount = dbContext.Person.Count( );
                // For the given pageIndex will skip the number of records multiplied by the pageSize and return a list of records appropriate for the pageIndex.
                var actorList = mapper.Map< List < ActorViewModel >>( dbContext.Person.Skip( pageIndex * pageSize ).Take( pageSize ) ).ToList( );

                response.Status = true;
                response.Message = "Success";
                response.Data = new { Person = actorList, Count = actorCount };

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

        [HttpGet( "{id}" )]
        public IActionResult GetPersonById( int id )
        {
            BaseResponseModel response = new BaseResponseModel( );
            try
            {
                var person = dbContext.Person.Where( x => x.Id == id ).FirstOrDefault( );

                if(person == null)
                {
                    response.Status = false;
                    response.Message = "Record does not exist.";
                    return BadRequest( response );
                }

                var personData = new ActorDetailsViewModel
                {
                    Id = person.Id,
                    DateOfBirth = person.DateOfBirth,
                    Name = person.Name,
                    Movies = dbContext.Movie.Where( x => x.Actors.Contains( person ) ).Select( x => x.Title ).ToArray( )
                };

                response.Status = true;
                response.Message = "Success";
                response.Data = personData;

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

        [HttpGet]
        [Route("Search/{searchText}")]
        public IActionResult Get( string searchText )
        {
            BaseResponseModel response = new BaseResponseModel( );

            try
            {
                var searchedPerson = dbContext.Person.Where( x => x.Name.Contains( searchText ) ).Select( x => new
                {
                    x.Id,
                    x.Name,
                    
                } ).ToList( );

                response.Status = true;
                response.Message = "Success";
                response.Data = searchedPerson;

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
        public IActionResult Post( ActorViewModel model )
        {
            BaseResponseModel response = new BaseResponseModel( );

            try
            {
                if(ModelState.IsValid)
                {
                    var postedModel = new Person( )
                    {
                        Name = model.Name,
                        DateOfBirth = model.DateOfBirth,
                    };

                    dbContext.Person.Add( postedModel );
                    //Also consider SaveChangesAsync
                    dbContext.SaveChanges( );

                    model.Id = postedModel.Id;

                    response.Status = true;
                    response.Message = "Created Successfully";
                    response.Data = model;

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
        public IActionResult Put( ActorViewModel model ) 
        { 
            BaseResponseModel response = new BaseResponseModel();

            try
            {
                if(ModelState.IsValid)
                {
                    var postedModel = mapper.Map<Person>( model ); 

                    if( model.Id <= 0)
                    {
                        response.Status = false;
                        response.Message = "Invalid Person Data";
                        return BadRequest( response );
                    }

                    var personDetails = dbContext.Person.Where( x => x.Id == model.Id ).AsNoTracking().FirstOrDefault( );
                    if( personDetails == null)
                    {
                        response.Status = false;
                        response.Message = "Invalid person record.";

                        return BadRequest( response );
                    }

                    dbContext.Person.Update( postedModel );
                    dbContext.SaveChanges( );

                    response.Status = true;
                    response.Message = "Updated Successfully";
                    response.Data = postedModel;

                    return Ok( response );
                }
                else
                {
                    response.Status = false;
                    response.Message = "Validation Failed";
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
            BaseResponseModel response = new BaseResponseModel( );

            try
            {
                var person = dbContext.Person.Where( x => x.Id == id ).FirstOrDefault( );
                if(person == null)
                {
                    response.Status = false;
                    response.Message = "Record does not exist.";
                    return BadRequest( response );
                }

                dbContext.Person.Remove( person );
                dbContext.SaveChanges( );

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
    }
}
