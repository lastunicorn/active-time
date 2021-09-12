using System;
using MediatR;

namespace DustInTheWind.ActiveTime.Application.CurrentDate.ChangeDate
{
    public class ChangeDateRequest : IRequest
    {
        public DateTime? Date { get; set; }
    }
}