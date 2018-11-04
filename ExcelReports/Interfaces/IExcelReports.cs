using Core.Types;
using System.Collections.Generic;

namespace ExcelReports.Interfaces
{
    public interface IExcelReports
    {
        string CreateVisitLogReport(IEnumerable<VisitLog> visits, Pacient pacient);

        string CreateVisitLogReport(IEnumerable<VisitLog> visits, Doctor doctor);
    }
}