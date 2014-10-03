using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeatherApp.Models.Repository
{
	public class WeatherRepository : IRepository
	{
		private bool _disposed = false;
		private WeatherAppEntities _entities = new WeatherAppEntities();

		public int Add(Position position)
		{
			// The stored procedure returns the id of the newly added position.
			var newId = _entities.uspAddPosition(
				position.Name,
				position.Lat,
				position.Lng
			);

			// Convert the row returned from the SP to an int and return it.
			return Convert.ToInt32(newId.FirstOrDefault().Value);
		}

		public void Add(Weather weather)
		{
			_entities.uspAddWeather(
				weather.PositionId,
				weather.Temperature,
				weather.City,
				weather.SymbolUrl,
				weather.ValidTime,
				weather.NextUpdate
			);
		}

		public void DeleteWeatherByPositionId(int positionId)
		{
			_entities.uspDeleteWeatherByPositionId(positionId);
		}

		public void Update(Weather weather)
		{
			_entities.uspUpdateWeather(
				weather.Id,
				weather.PositionId,
				weather.Temperature,
				weather.City,
				weather.SymbolUrl,
				weather.ValidTime,
				weather.NextUpdate
				);
		}

		public IQueryable<Position> GetPositions()
		{
			return _entities.Position.AsQueryable();
		}

		public IQueryable<Weather> GetWeathers()
		{
			return _entities.Weather.AsQueryable();
		}

		public IQueryable<Weather> GetWeathersByPositionId(int positionId)
		{
			return _entities.uspGetWeatherByPositionId(positionId).AsQueryable();
		}

		public void Save()
		{
			_entities.SaveChanges();
		}

		#region IDisposable
		public void Dispose()
		{
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					_entities.Dispose();
				}
			}

			_disposed = true;
		}
		#endregion
	}
}