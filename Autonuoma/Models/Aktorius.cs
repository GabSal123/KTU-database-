namespace Org.Ktu.Isk.P175B602.Autonuoma.Models;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

public class AktoriusList{

    [DisplayName("ID")]
    public int Id{get;set;}

    [DisplayName("Vardas")]
    public string Vardas{get;set;}

    [DisplayName("Pavarde")]
    public string Pavarde{get;set;}

    [DisplayName("Gimimo metai")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime Gimimo_metai {get;set;}

    [DisplayName("Lytis")]
    public string Lytis{get;set;}
}