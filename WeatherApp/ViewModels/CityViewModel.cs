using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WeatherApp.ViewModels
{
	public class CityViewModel
	{
		[Required(AllowEmptyStrings = false, ErrorMessage = "Du måste fylla i en stad.")]
		[RegularExpression("[a-zåäöA-ZÅÄÖ]+", ErrorMessage = "Staden du angav innehåller felaktiga tecken.")]
		public string City { get; set; }
	}
}