using SurveyApp.Shared.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyApp.Shared.Dto_s
{
    public class UserForLoginAccessDto : IDto
    {
        public string Role { get; set; }
    }
}
