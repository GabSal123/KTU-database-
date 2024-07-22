namespace Org.Ktu.Isk.P175B602.Autonuoma.Models;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


public class DarbuotojasList{


    [DisplayName("ID")]
    public int Id{get;set;}

    [DisplayName("Vardas")]
    public string Vardas {get;set;}

    [DisplayName("Pavarde")]
    public string Pavarde {get;set;}

    [DisplayName("Telefono numeris")]
    public string Tel_numeris{get;set;}

    [DisplayName("Elektroninis pastas")]
    [EmailAddress]
    public string El_pastas{get;set;}

    [DisplayName("Pareigos")]
    public string Pareigos {get;set;}

    [DisplayName("Lytis")]
    public string Lytis {get;set;}

    [DisplayName("Parduotuves Id")]
    public int Parduotuve_id {get;set;}
}