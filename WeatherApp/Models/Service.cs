using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeatherApp.Models.Repository;
using WeatherApp.Models.Web_Services;

namespace WeatherApp.Models
{
	public class Service
	{
		// Entity framework repository.
		private IRepository _repository;

		// Web API services.
		private GeoNamesService geoNames;
		private YrService yr;

		public Service(IRepository repository)
		{
			_repository = repository;

			// Initiate web services.
			geoNames = new GeoNamesService();
			yr = new YrService();
		}

		public List<Weather> GetForecast(string cityName)
		{
			// Get the position.
			var position = GetPosition(cityName);

			// Get all weather entries belonging to this position.
			var weatherEntries = _repository.GetWeathersByPositionId(position.Id).ToList();

			// Instansiate a list which we will populate soon and return.
			List<Weather> forecast;

			// Make sure that the weather entries found in the database
			// hasn't been expired.
			if (weatherEntries.Count > 0 && weatherEntries.First().NextUpdate >= DateTime.Now)
			{
				forecast = weatherEntries;
			}

			// If they have expired (or there weren't any entries at all in
			// the database) we load them from the Yr.no's web API.
			else
			{
				forecast = LoadForecastFromYr(position);
			}

			return forecast;
		}

		private List<Weather> LoadForecastFromYr(Position position)
		{
			// Delete all current weather entries (if any) belonging
			// to this specific positionId.
			_repository.DeleteWeatherByPositionId(position.Id);

			// Get a new set of weather entries from Yr.no's web API.
			var forecast = yr.GetWeatherData(position);

			// Save them to the repository individually.
			foreach (var entry in forecast)
			{
				_repository.Add(entry);
			}

			// Save repository changes.
			_repository.Save();

			// Return the forecast.
			return forecast;
		}

		private Position GetPosition(string cityName)
		{
			// Get all positions from database.
			var positions = _repository.GetPositions().ToList();

			// If the database contains any position entries we
			// try to find the city we're looking for and if it doesn't
			// exists we set it as null and later try a lookup.
			var position = (positions.Count > 0) ?
				positions.Find(p => p.Name.ToLower() == cityName.ToLower()) : null;

			if (position == null)
			{
				// Using the GeoNames service we try to get the position
				// from the web API.
				position = geoNames.GetGeoName(cityName);

				// Add it to the repository.
				position.Id = _repository.Add(position);

				// Save repository changes.
				_repository.Save();
			}

			return position;
		}
	}
}