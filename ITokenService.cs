using HospitalAppointment.Models;
//using webapi.Models;

namespace HospitalAppointment.Auth
{
    public interface ITokenService
    {
        string CreateToken(User us);
    }
}
