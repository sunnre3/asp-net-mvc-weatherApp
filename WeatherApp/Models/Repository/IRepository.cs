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
		int Add(Weather weather);
		void DeleteWeathersByPositionId(int positionId);
		void Update(Weather weather);
		IQueryable<Position> GetPositions();
		IQueryable<Weather> GetWeathers();
		IQueryable<Weather> GetWeathersByPositionId(int positionId);
		void Save();
	}
}
