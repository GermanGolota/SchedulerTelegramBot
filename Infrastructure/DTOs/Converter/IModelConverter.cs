using Core.Entities;

namespace Infrastructure.DTOs
{
    public interface IModelConverter
    {
        Schedule ConvertScheduleFromDTO(ScheduleModel dto);
        Alert ConvertAlertFromDto(AlertModel dto);
        ScheduleModel ConvertScheduleToDTO(Schedule schedule);
        AlertModel ConvertAlertToDTO(Alert alert);
    }
}
