namespace Org.Ktu.Isk.P175B602.Autonuoma.Models;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


public class ParduotuveList{

    [DisplayName("ID")]
    public int Id {get;set;}


    [DisplayName("Ikurimo data")]
	[DataType(DataType.Date)]
	[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
	public DateTime? Ikurimo_data { get; set; }


    [DisplayName("Adresas")]
    public string Adresas {get;set;}

}