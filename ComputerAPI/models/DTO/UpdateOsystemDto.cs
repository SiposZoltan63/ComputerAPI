using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ComputerAPI.models.DTO
{
    [Route("osystem")]
    [ApiController]
    public class UpdateOsystemDto : ControllerBase
    {
        public string? Name { get; set; }
        public int Version { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
