//using Microsoft.EntityFrameworkCore;
//using Microsoft.OpenApi.Models;
//using MovieAPIDemo.Server.Data;

//namespace MovieAPIDemo.Server
//{
//    public class Startup
//    {
//        public Startup( IConfiguration _Configuration )
//        {
//            Configuration = _Configuration;
//        }
//        public IConfiguration Configuration { get; }

//        public void ConfigureServices( IServiceCollection services )
//        {
//            services.AddDbContext<MovieDbContext>( options => options.UseSqlServer( Configuration.GetConnectionString( "Default" ) ) );
//            services.AddControllers( );
//            services.AddSwaggerGen( c =>
//            {
//                c.SwaggerDoc( "v1", new OpenApiInfo { Title = "MovieApiDemo", Version = "v1", } );
//            } );
//        }


//    }
//}
