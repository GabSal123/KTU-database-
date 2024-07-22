namespace Org.Ktu.Isk.P175B602.Autonuoma.Controllers;

using Microsoft.AspNetCore.Mvc;

using Org.Ktu.Isk.P175B602.Autonuoma.Repositories;


using ContractsReport = Org.Ktu.Isk.P175B602.Autonuoma.Models.Ataiskaitenas;



/// <summary>
/// Controller for producing reports.
/// </summary>
public class AtaiskaitenasController : Controller
{
	
	[HttpGet]
	public ActionResult Contracts(DateTime? dateFrom, DateTime? dateTo, int? apmoketas, decimal? sumaNuo)
	{
		var report = new ContractsReport.ReportAtaskaitenas();
		report.DateFrom = dateFrom;
		report.DateTo = dateTo?.AddHours(23).AddMinutes(59).AddSeconds(59); //move time of end date to end of day

		report.Sutartys = AtaiskaitenasRepo.GetContracts(report.DateFrom, report.DateTo, apmoketas,sumaNuo);

		foreach (var item in report.Sutartys)
		{
			report.VisoSumaMokejimu += item.Kaina;
		}

		return View(report);
	}

}
