using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.DTOs
{
    public class ModelConverter : IModelConverter
    {
        public Alert ConvertAlertFromDto(AlertModel dto)
        {
            Alert output = new Alert
            {
                Message = dto.Message,
                Cron = dto.Cron
            };
            return output;
        }

        public Schedule ConvertScheduleFromDTO(ScheduleModel dto)
        {
            List<Alert> alerts = new List<Alert>();
            foreach (AlertModel alert in dto.Alerts)
            {
                alerts.Add(ConvertAlertFromDto(alert));
            }
            Schedule output = new Schedule
            {
                Name = dto.Name,
                Alerts = alerts
            };
            return output;
        }
    }
}
