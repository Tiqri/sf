using AutoMapper;
using Blissmo.API.Model;
using Blissmo.BookingServiceActor.Interfaces.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blissmo.API.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ShowTime, ApiShowTime>()
              .ForMember(vm => vm.Id, map => map.MapFrom(m => m.Id))
              .ForMember(vm => vm.Name, map => map.MapFrom(m => m.Name));

            CreateMap<Theater, ApiTheater>()
              .ForMember(vm => vm.Id, map => map.MapFrom(m => m.Id))
              .ForMember(vm => vm.Name, map => map.MapFrom(m => m.Name))
              .ForMember(vm => vm.Address, map => map.MapFrom(m => m.Address));

            CreateMap<Movie, ApiMovie>()
              .ForMember(vm => vm.Id, map => map.MapFrom(m => m.Id))
              .ForMember(vm => vm.Title, map => map.MapFrom(m => m.Title))
              .ForMember(vm => vm.ShortDescription, map => map.MapFrom(m => m.ShortDescription))
              .ForMember(vm => vm.LongDescription, map => map.MapFrom(m => m.LongDescription));

            CreateMap<Booking, ApiBooking>()
              .ForMember(vm => vm.Id, map => map.MapFrom(m => m.Id))
              .ForMember(vm => vm.UserId, map => map.MapFrom(m => m.UserId))
              .ForMember(vm => vm.Movie, map => map.MapFrom(m => m.Movie))
              .ForMember(vm => vm.Theater, map => map.MapFrom(m => m.Theater))
              .ForMember(vm => vm.ShowTime, map => map.MapFrom(m => m.ShowTime))
              .ForMember(vm => vm.NumberOfTickets, map => map.MapFrom(m => m.NumberOfTickets));
        }
    }
}
