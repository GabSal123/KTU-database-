namespace Org.Ktu.Isk.P175B602.Autonuoma.Models;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

public class MokejimasList{

    [DisplayName("ID")]
    public int Id{get;set;}

    [DisplayName("Darbuotojas")]
    public string Darbuotojas{get;set;}

    [DisplayName("Klientas")]
    public string Klientas{get;set;}

    [DisplayName("Suma")]
    public float Suma{get;set;}

    [DisplayName("Mokejimo data")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime? Mokejimo_data {get;set;}

    [DisplayName("Filmai")]
    public string Filmai {get;set;}


    [DisplayName("Apmoketas")]
    public bool Apmoketas {get;set;}
}


public class MokejimasCE{

    

    public class MokejimasM{

    [DisplayName("ID")]
    public int Id{get;set;}

    [DisplayName("Mokejimo data")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    [Required]
    public DateTime? Mokejimo_data {get;set;}


    [DisplayName("Darbuotojas")]
    [Required]
    public int Darbuotojo_id {get;set;}


    [DisplayName("Klientas")]
    [Required]
    public int Kliento_id {get;set;}

    [DisplayName("Suma")]
    [Required]
    public decimal Kaina{get;set;}


    [DisplayName("Apmoktas")]
    [Required]
    public string Apmoketas{get;set;}


    public bool ApmoketasB{get;set;}
    }


    public class PirktasFilmas{

        public int InListID{get;set;}

        [DisplayName("Filmas")]
		[Required]
		public string Filmas { get; set; }

        [DisplayName("Kaina")]
		[Required]
		public decimal Kaina { get; set; }
        [DisplayName("Inventoriaus Id")]
        [Required]
        public int Inventorius{get;set;}

        [DisplayName("Kiekis")]
        [Range(1,60)]
        [Required]
        public int Kiekis{get;set;}
    }


    public class ListsM
	{
		public IList<SelectListItem> Klientai{ get; set; }
		public IList<SelectListItem> Darbuotojai { get; set; }
        public IList<SelectListItem> Filmai {get;set;}
        public IList<SelectListItem> Apmokejimas{get;set;}

	}

    public MokejimasM mokejimas{get;set;} = new MokejimasM();

    public IList<PirktasFilmas> uzsakytiFilmai{get;set;} = new List<PirktasFilmas>();

    public ListsM Lists { get; set; } = new ListsM();

    public IList<PirktasFilmas> istrintiFilmai{get;set;} = new List<PirktasFilmas>();



    public void RemoveNull(){
        uzsakytiFilmai = uzsakytiFilmai.Where(item => item.Filmas != null).ToList();
    }

    public decimal CalculatePrice(){
        decimal price = 0;
        foreach(var item in uzsakytiFilmai){
            price += item.Kaina*item.Kiekis;
        }
        return price;
    }

}