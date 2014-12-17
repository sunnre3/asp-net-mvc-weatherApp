using System;
using System.Collections.Generic;
using System.Data;
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
			var entry = _entities.Position.Add(position);
			return entry.Id;
		}

		public int Add(Weather weather)
		{
			var entry = _entities.Weather.Add(weather);
			return entry.Id;
		}

		public void DeleteWeathersByPositionId(int positionId)
		{
			var weathers = _entities.Weather.Where(w => w.PositionId == positionId);
			foreach (var w in weathers) _entities.Weather.Remove(w);
		}

		public void Update(Weather weather)
		{
			_entities.Entry(weather).State = EntityState.Modified;
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
			return _entities.Weather.Where(w => w.PositionId == positionId).AsQueryable();
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