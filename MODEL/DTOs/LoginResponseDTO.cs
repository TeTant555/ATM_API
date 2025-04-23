using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.DTOs;

public class LoginResponseDTO
{
    public string? Message { get; set; }
    public object? Data { get; set; }
    public bool Issuccess { get; set; }
}
