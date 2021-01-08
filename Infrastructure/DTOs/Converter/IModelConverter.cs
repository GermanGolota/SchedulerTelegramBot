using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.DTOs
{
    public interface IModelConverter
    {
        Schedule ConvertScheduleFromDTO(ScheduleModel dto);
        Alert ConvertAlertFromDto(AlertModel dto);
    }
}
