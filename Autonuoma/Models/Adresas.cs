namespace Org.Ktu.Isk.P175B602.Autonuoma.Models;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;



public class AdresasList{

    [DisplayName("ID")]
    public int Id{get;set;}

    [DisplayName("Gatve")]
    public string Gatve{get;set;}

    [DisplayName("Miestas")]
    public string Miestas {get;set;}

    [DisplayName("Pasto kodas")]
    public string Pasto_kodas {get;set;}

    [DisplayName("Salis")]
    public string Salis {get;set;}

}