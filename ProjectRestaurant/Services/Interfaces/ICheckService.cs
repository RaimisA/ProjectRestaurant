using ProjectRestaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectRestaurant.Services.Interfaces
{
    public interface ICheckService
    {
        void PrintCheck(Order order, bool isClientCheck);
        void SaveCheckToFile(Order order, string filePath, bool isClientCheck);
    }
}
