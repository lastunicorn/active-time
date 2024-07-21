﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.ActiveTime.Application.Recording2;
using DustInTheWind.ActiveTime.Domain;
using DustInTheWind.ActiveTime.Ports.DataAccess;
using MediatR;

namespace DustInTheWind.ActiveTime.Application.UseCases.CurrentDate.PresentTimeRecords
{
    internal class PresentTimeRecordsUseCase : IRequestHandler<PresentTimeRecordsRequest, PresentTimeRecordsResponse>, IDisposable
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly CurrentDay currentDay;

        public PresentTimeRecordsUseCase(IUnitOfWork unitOfWork, CurrentDay currentDay)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.currentDay = currentDay ?? throw new ArgumentNullException(nameof(currentDay));
        }

        public Task<PresentTimeRecordsResponse> Handle(PresentTimeRecordsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                DateTime currentDate = currentDay.Date;
                DayRecord dayRecord = CreateDayRecord(currentDate);

                PresentTimeRecordsResponse response = new PresentTimeRecordsResponse
                {
                    Records = dayRecord.AllIntervals.ToArray()
                };

                return Task.FromResult(response);
            }
            finally
            {
                Dispose();
            }
        }

        private DayRecord CreateDayRecord(DateTime currentDate)
        {
            IEnumerable<TimeRecord> timeRecords = unitOfWork.TimeRecordRepository.GetByDate(currentDate);
            return new DayRecord(timeRecords);
        }

        public void Dispose()
        {
            unitOfWork?.Dispose();
        }
    }
}