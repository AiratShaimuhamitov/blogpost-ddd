using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Blogpost.Application.Common.Behaviours;
using FluentValidation;
using Blogpost.Application.Repositories;

namespace Blogpost.Application
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));

            services.AddTransient(typeof(PostsRepository));
            services.AddTransient(typeof(CommentsRepository));
            services.AddTransient(typeof(ProfilesRepository));
        }
    }
}