using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.DTOs;

public class UserRequestDTO
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
}
