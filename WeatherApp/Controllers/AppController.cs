using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeatherApp.Exceptions;
using WeatherApp.Models;
using WeatherApp.Models.Repository;
using WeatherApp.ViewModels;

namespace WeatherApp.Controllers
{
    public class AppController : Controller
    {
		private IRepository _repository;

		public AppController()
			:this(new WeatherRepository())
		{
			// Empty.
		}

		public AppController(IRepository repository)
		{
			_repository = repository;
		}

       public ActionResult Index()
        {
            return View();
        }

		public ActionResult ShowForecast(CityViewModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					// Initiate web service.
					var service = new Service(_repository);

					// Get the forecast for the position.
					var forecast = service.GetForecast(model.City);

					// Return the view.
					return View("ShowForecast", forecast);
				}

				catch (GeoNameNotFoundException)
				{
					// Add a message to the validation summary.
					ModelState.AddModelError(String.Empty, "Staden \"" + model.City + "\" hittades inte.");

					// Return the view.
					return View("Index", model);
				}

				catch
				{
					// Add a message to the validation summary.
					ModelState.AddModelError(String.Empty, "Ett okänt fel uppstod vid hämtning av prognos. Var vänlig försök igen.");

					// Return the view.
					return View("Index", model);
				} 
			}

			else
			{
				return View("Index");
			}
		}
    }
}
