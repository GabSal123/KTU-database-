namespace Org.Ktu.Isk.P175B602.Autonuoma.Models.Ataiskaitenas;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


/// <summary>
/// View model for single contract in a report.
/// </summary>
public class Ataiskaitenas
{
    public int kID {get;set;}

	[DisplayName("Mokejimas")]
	public int Nr { get; set; }

	[DisplayName("Data")]
	[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
	public DateTime MokejimoData { get; set; }

	public string VardasPilnas { get; set; }

	public string TelefonoNumeris { get; set; }
    public string Email { get; set; }

	[DisplayName("Mokejimo verte")]
	public decimal Kaina { get; set; }

	public decimal BendraSuma { get; set; }

    public int AtliktaMokejimu {get;set;}

    public string Inicialai {get;set;}

    public int Apmoketas {get;set;}
    
    public decimal Maksimalus{get;set;} 

	


}


public class ReportAtaskaitenas
{
	[DataType(DataType.DateTime)]
	[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
	public DateTime? DateFrom { get; set; }

	[DataType(DataType.DateTime)]
	[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
	public DateTime? DateTo { get; set; }


    public int Apmoketas {get;set;}

    public decimal SumaNuo {get;set;}

	public List<Ataiskaitenas> Sutartys { get; set; }

    public IList<string> Apmokejimai = new List<string>{"Apmoketi","Neapmoketi","Visi"};
	public decimal VisoSumaMokejimu { get; set; }


}



