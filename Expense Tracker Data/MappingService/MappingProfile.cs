using AutoMapper;
using Expense_Tracker_Core.Models;
using Expense_Tracker_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Tracker_Data.MappingService
{
    public class MappingProfile : Profile
    {

        public MappingProfile() { 

            //CreateMap<CategoryReq, Category>()
            //    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));

            CreateMap<Category, CategoryRes>();
            CreateMap<TransactionReq, Transaction>();
            CreateMap<Transaction, TransactionRes>();
            CreateMap<User, UserRes>();
        }
    }
}
