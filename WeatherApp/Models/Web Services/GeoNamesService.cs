using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Globalization;
using WeatherApp.Exceptions;

namespace WeatherApp.Models.Web_Services
{
	public class GeoNamesService
	{
		private string apiURL = "http://api.geonames.org/search?name_equals={0}&country=SE&maxRows=1&username=sunnre";

		public Position GetGeoName(string city)
		{
			try
			{
				// Load the XML document from geonames.org
				var xdoc = XDocument.Load(String.Format(apiURL, city));

				// Return the geoname.
				return (from l in xdoc.Descendants("geoname")
						select new Position
						{
							Id = int.Parse(l.Element("geonameId").Value),
							Name = l.Element("name").Value,
							Lat = float.Parse(l.Element("lat").Value, CultureInfo.InvariantCulture),
							Lng = float.Parse(l.Element("lng").Value, CultureInfo.InvariantCulture)
						}).First();
			}

			catch
			{
				throw new GeoNameNotFoundException();
			}
		}
	}
}