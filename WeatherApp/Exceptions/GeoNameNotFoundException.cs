using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeatherApp.Exceptions
{
	public class GeoNameNotFoundException : Exception
	{
		public GeoNameNotFoundException()
		{
			// Empty.
		}

		public GeoNameNotFoundException(string message)
			:base(message)
		{
			// Empty.
		}

		public GeoNameNotFoundException(string message, Exception inner)
			:base(message, inner)
		{
			// Empty.
		}
	}
}