using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Core.interfaces
{
    public interface IEmailService
    {
        Task SendAsync();
    }
}
