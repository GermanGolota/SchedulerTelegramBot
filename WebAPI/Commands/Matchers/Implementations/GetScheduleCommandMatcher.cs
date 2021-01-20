using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Commands.Verifiers
{
    public class GetScheduleCommandMatcher:StandardMatcherBehaviour<GetScheduleCommand>
    {
        public GetScheduleCommandMatcher():base(CommandNames.GetSchedule)
        {

        }
    }
}
