using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blissmo.API.Mapper
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            AutoMapper.ServiceCollectionExtensions.UseStaticRegistration = false;
            AutoMapper.Mapper.Reset();

            AutoMapper.Mapper.Initialize(x =>
            {
                x.AddProfile<MappingProfile>();
            });

            AutoMapper.Mapper.Configuration.AssertConfigurationIsValid();
        }
    }
}
