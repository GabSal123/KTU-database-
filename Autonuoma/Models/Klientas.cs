namespace Org.Ktu.Isk.P175B602.Autonuoma.Models;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


public class KlientasList{


    [DisplayName("ID")]
    [Required]
    public int Id {get;set;}

    [DisplayName("Vardas")]
    [Required]
    public string Vardas {get;set;}


    [DisplayName("Pavarde")]
    [Required]
    public string Pavarde {get;set;}


    [DisplayName("Lytis")]
    [Required]
    public string Lytis{get;set;}


    [DisplayName("Telefono numeris")]
    [Required]
    public string Tel_numeris {get;set;}


    [DisplayName("Elektroninis paštas")]
	[EmailAddress]
	[Required]
    public string El_pastas{get;set;}


    [DisplayName("Lojalumo pozymis")]
    [Required]
    public bool Lojalus {get;set;}


    [DisplayName("Parduotuves ID")]
    [Required]
    public int Parduotuve_id{get;set;}

}