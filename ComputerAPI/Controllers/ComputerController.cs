using CmpShopApi.Models.DTOs;
using ComputerAPI.models;
using ComputerAPI.models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComputerAPI.Controllers
{
    [Route("computer")]
    [ApiController]
    public class ComputerController : ControllerBase
    {
        public CmpShopDbContext context = new CmpShopDbContext();
        [HttpGet("getAll")]
        public object GetAllComputer()
        {
            var computers = context.Computers.ToList();
            return new { message = "Sikeres lekérdezés", result = computers };
        }

        [HttpPost]
        public object AddNewComputer(AddNewComputerDto addNewComputerDto)
        {
            var computer = new Computer
            {
                Id = Guid.NewGuid(),
                Brand = addNewComputerDto.Brand,
                Type = addNewComputerDto.Type,
                Display = addNewComputerDto.Display,
                Memory = addNewComputerDto.Memory,
                CreatedTime = DateTime.Now,
                OsysId = addNewComputerDto.OsysId,
            };

            context.Computers.Add(computer);
            context.SaveChanges();

            return StatusCode(201, new { message = "Sikeres felvétel", result = computer });
        }

        [HttpPut]
        public object UpdateComputers([FromQuery] Guid id, [FromBody] UpdateComputerDto updateComputerDto)
        {
            var computer = context.Computers.FirstOrDefault(computer => computer.Id == id);

            if (computer != null)
            {
                computer.Brand = updateComputerDto.Brand;
                computer.Type = updateComputerDto.Type;
                computer.Display = updateComputerDto.Display;
                computer.Memory = updateComputerDto.Memory;
                computer.OsysId = updateComputerDto.OsysId;

                context.Computers.Update(computer);
                context.SaveChanges();

                return StatusCode(200, new { message = "Sikeres frissítés", result = computer });
            }

            return StatusCode(404, new { message = "Sikertlen frissítés", result = computer });
        }

        [HttpDelete]
        public object DeleteComputer([FromQuery] Guid id)
        {
            var computer = context.Computers.Find(id);

            if (computer != null)
            {
                context.Computers.Remove(computer);
                context.SaveChanges();

                return StatusCode(204, new { message = "Sikeres törlés" });
            }

            return StatusCode(404, new { message = "Sikertlen törlés" });
        }
    }
}
