using Core.Entities;
using System.Collections.Generic;

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

        public ScheduleModel ConvertScheduleToDTO(Schedule schedule)
        {
            List<AlertModel> alerts = new List<AlertModel>();
            foreach (var alert in schedule.Alerts)
            {
                alerts.Add(ConvertAlertToDTO(alert));
            }
            ScheduleModel output = new ScheduleModel
            {
                Alerts = alerts,
                Name = schedule.Name
            };
            return output;
        }
        public AlertModel ConvertAlertToDTO(Alert alert)
        {
            return new AlertModel
            {
                Cron = alert.Cron,
                Message = alert.Message
            };
        }

    }
}
