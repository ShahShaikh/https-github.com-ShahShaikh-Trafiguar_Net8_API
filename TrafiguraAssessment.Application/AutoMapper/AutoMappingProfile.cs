using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TrafiguraAssessment.Application.DTO;
using TrafiguraAssessment.Domain.Entities;

namespace TrafiguraAssessment.Application.AutoMapper
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<TransactionDto, TradeTransaction>();
        }
    }
}
