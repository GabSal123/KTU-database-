namespace Org.Ktu.Isk.P175B602.Autonuoma.Models;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

public class FilmasList{

    [DisplayName("ID")]
    public int  Id{get;set;}

    [DisplayName("Pavadinimas")]
    public string Pavadinimas{get;set;}

    [DisplayName("Trukme")]
    [Range(30,300)]
    public int Trukme{get;set;}

    [DisplayName("Kaina")]
    [Range(1,100)]
    public int Kaina{get;set;}


    [DisplayName("Isleidimo data")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime? Isleidimo_data {get;set;}

    [DisplayName("Kategorija")]
    public string Kategorija{get;set;}

    [DisplayName("Vidutinis ivertinimas")]
    [Range(0,5)]
    public float? Vidurkis {get;set;}

    [DisplayName("Filmu kiekis")]
    [Range(0,int.MaxValue)]
    public int Kiekis {get;set;}
}

public class FilmasCE{


    public class FilmasM{
    [DisplayName("ID")]
    public int  Id{get;set;}

    [DisplayName("Pavadinimas")]
    [Required]
    public string Pavadinimas{get;set;}

    [DisplayName("Trukme")]
    [Range(30,300)]
    [Required]
    public int Trukme{get;set;}

    [DisplayName("Kaina")]
    [Range(1,100)]
    [Required]
    public int Kaina{get;set;}


    [DisplayName("Isleidimo data")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    [Required]
    public DateTime? Isleidimo_data {get;set;}

    [DisplayName("Kategorija")]
    [Required]
    public int Kategorija{get;set;}

    [DisplayName("Aprasymas")]
    [Required]
    public string Aprasymas{get;set;}

    [DisplayName("Priedai")]
    [Required]
    public string Priedai {get;set;}

    [DisplayName("Tiekejas")]
    [Required]
    public int Tiekejas {get;set;}

    }


    public class ListsM
	{
		public IList<SelectListItem> Inventoriai { get; set; }
		public IList<SelectListItem> Aktoriai{ get; set; }
		public IList<SelectListItem> Ivertinimai { get; set; }
		public IList<SelectListItem> Tiekejai { get; set; }

        public IList<SelectListItem> Kategorijos { get; set; }
	}

    /// <summary>
	/// Sutartis.
	/// </summary>
	public FilmasM Filmas { get; set; } = new FilmasM();


	/// <summary>
	/// Lists for drop down controls.
	/// </summary>
	public ListsM Lists { get; set; } = new ListsM();

}