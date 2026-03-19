using Newtonsoft.Json;
using SurveyApp.Shared.Abstract;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyApp.Shared.Dto_s
{
    public class JwtDto : IDto
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "nameidentifier")]
        public string NameIdentifier { get; set; }
        [JsonProperty(PropertyName = "role")]
        public string Role { get; set; }
        public static JwtDto GetViewModel(JwtSecurityToken jwtSecurityToken)
        {
            return new JwtDto
            {
                Name = jwtSecurityToken.Claims.FirstOrDefault(s => s.Type.Contains("nameidentifier")).Value,
                NameIdentifier = jwtSecurityToken.Claims.FirstOrDefault(s => s.Type.Contains("name")).Value,
                Role = jwtSecurityToken.Claims.FirstOrDefault(s => s.Type.Contains("role")).Value
            };
        }
    }
}
