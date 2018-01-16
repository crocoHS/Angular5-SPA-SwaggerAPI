using AutoMapper.Configuration;
using Todo.Api.Models;
using Todo.Model;

namespace Todo.Api
{
    public class MappingProfile : MapperConfigurationExpression
    {
        public MappingProfile()
        {
            CreateMap<Technology, TechnologyDT>();
            CreateMap<TechnologyDT, Technology>();
            CreateMap<Customer, CustomerDT>();
            CreateMap<CustomerDT, Customer>();
            CreateMap<EmailMessage, EmailMessageDT>();
        }
    }
}
