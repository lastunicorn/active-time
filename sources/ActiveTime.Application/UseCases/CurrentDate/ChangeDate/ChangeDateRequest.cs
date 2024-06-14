using System;
using MediatR;

namespace DustInTheWind.ActiveTime.Application.UseCases.CurrentDate.ChangeDate
{
    public class ChangeDateRequest : IRequest
    {
        public DateTime? Date { get; set; }
    }
}