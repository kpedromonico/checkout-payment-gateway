using AutoMapper;
using Payment.API.Application.Extensions;
using Payment.API.Application.Payloads.Reponses;
using Payment.API.Application.Payloads.Requests;
using Payment.Domain.AggregatesModel.TransactionAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.API.Application.Mappers
{
    public class PaymentMapperProfile : Profile
    {
        public PaymentMapperProfile()
        {
            CreateMap<PaymentAttemptRequest, Transaction>()
                .ForPath(x => x.Card.CardNumber, y => y.MapFrom(z => z.CardNumber))
                .ForPath(x => x.Card.SecurityNumber, y => y.MapFrom(z => z.Cvv))
                .ForPath(x => x.Card.ExpiryMonth, y => y.MapFrom(z => z.ExpiryMonth))
                .ForPath(x => x.Card.ExpiryYear, y => y.MapFrom(z => z.ExpiryYear));

            CreateMap<Transaction, PaymentConfirmationResponse>()
                .ForMember(x => x.CardNumber, y => y.MapFrom(z => z.Card.CardNumber.MaskCardNumber()))
                .ForMember(x => x.Approved, y => y.MapFrom(z => z.Sucess))
                .ForMember(x => x.TransactionDate, y => y.MapFrom(z => z.CreationDate))
                .ForMember(x => x.TransactionId, y => y.MapFrom(z => z.Id));

            CreateMap<Transaction, PaymentDetailsResponse>()
                .ForMember(x => x.CardNumber, y => y.MapFrom(z => z.Card.CardNumber.MaskCardNumber()))
                .ForMember(x => x.Approved, y => y.MapFrom(z => z.Sucess))
                .ForMember(x => x.TransactionDate, y => y.MapFrom(z => z.CreationDate));
        }
    }
}
