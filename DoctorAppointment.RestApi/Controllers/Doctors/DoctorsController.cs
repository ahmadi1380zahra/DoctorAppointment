using DoctorAppointment.Services.Doctors.Contracts;
using DoctorAppointment.Services.Doctors.Contracts.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoctorAppointment.RestApi.Controllers.Doctors
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly DoctorService _service;
        public DoctorsController(DoctorService service)
        {
            _service = service;   
        }
        [HttpPost]
        public async Task Add([FromBody] AddDoctorDto dto)
        {
            await _service.Add(dto);
        }
        [HttpDelete("{id}")]
        public async Task Delete([FromRoute]int id)
        {
            await _service.Delete(id);
        }
        [HttpPut("{id}")]
        public async Task Update([FromRoute] int id,[FromBody]UpdateDoctorDto dto)
        {
            await _service.Update(id, dto);
        }
        [HttpGet]
        public List<GetDoctorDto> GetAll()
        {
            return _service.GetAll();
        }  
    }
}
