using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Models.Repository
{
	public interface IRepository : IDisposable
	{
		int Add(Position position);
		void Add(Weather weather);
		void DeleteWeatherByPositionId(int positionId);
		void Update(Weather weather);
		IQueryable<Position> GetPositions();
		IQueryable<Weather> GetWeathers();
		IQueryable<Weather> GetWeathersByPositionId(int positionId);
		void Save();
	}
}
