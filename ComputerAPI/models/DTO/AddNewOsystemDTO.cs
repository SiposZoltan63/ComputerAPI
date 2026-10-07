using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ComputerAPI.models
{
    [Route("osystem")]
    [ApiController]
    public class AddNewOsystemDTO : ControllerBase
    {
        [Key]
        [MaxLength(36)]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int Version { get; set; }
        public DateTime RegisterTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
