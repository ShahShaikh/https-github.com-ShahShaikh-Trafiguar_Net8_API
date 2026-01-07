using TrafiguraAssessment.Application.AutoMapper;
using TrafiguraAssessment.Application.Interface;
using TrafiguraAssessment.Application.Service;
using TrafiguraAssessment.Infrastructure.Interface;
using TrafiguraAssessment.Infrastructure.Repository;
using TrafiguraAssessment.Infrastructure.UnitOfWork;

namespace Trafigura_Assessment_API.Startup
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {

            services.AddScoped<IPositionService, PositionService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddAutoMapper(typeof(AutoMappingProfile)); //Mapper


        }
    }
}
