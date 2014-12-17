using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace WeatherApp.Models.Web_Services
{
	public class YrService
	{
		private string weatherIconURL = "http://api.yr.no/weatherapi/weathericon/1.1/?symbol={0};content_type=image/png";
		private string weatherDataURL = "http://api.yr.no/weatherapi/locationforecast/1.9/?lat={0};lon={1}";

		public string getWeatherIcon(int symbolId)
		{
			return String.Format(weatherIconURL, symbolId);
		}

		public List<Weather> GetWeatherData(Position position, int days = 3)
		{
			// Create a list containing weathers with the correct
			// number of days which will later be returned.
			var weatherData = new List<Weather>(days);

			// Build the request string to make the request to yr.no
			var requestString = String.Format(weatherDataURL,
				position.Lat.ToString().Replace(",", "."),
				position.Lng.ToString().Replace(",", ".")
			);

			// Load the XML to XDocument.
			var xdoc = XDocument.Load(requestString);

			// Loop through all the time nodes to retrieve a forecast
			// for the set number of days.
			for (int i = 0; i < days; i++)
			{
				var weatherEntry = (from node in xdoc.Descendants("product").Elements("time")
									// Set some local variables to make it easier as we move on.
									let dateTimeValidFrom = DateTime.Parse(node.Attribute("from").Value)
									let dateTimeExpires = DateTime.Parse(node.Attribute("to").Value)

									// Look for time elements that have the same day of the year
									// as today + the current FOR-loop iteration.
									where dateTimeValidFrom.DayOfYear == DateTime.Today.AddDays(i).DayOfYear

									// Make sure that we only get either period 2 or 3.
									&& (dateTimeValidFrom.Hour == 12 || dateTimeValidFrom.Hour == 14 || dateTimeValidFrom.Hour == 18)

									// And that it's a time element that contains the temperature.
									&& node.Elements("location").Elements("temperature").Any()

									// Create a Weather object from this.
									select new Weather
									{
										PositionId = position.Id,
										Temperature = Double.Parse(node.Element("location").Element("temperature").Attribute("value").Value, CultureInfo.InvariantCulture),
										City = position.Name,

										// We need to look for the symbol in an adjecent time tag which has the identical timestamp.
										SymbolUrl = getWeatherIcon(Convert.ToInt16((from s in xdoc.Descendants("product").Elements("time")
													  let dT = DateTime.Parse(s.Attribute("from").Value)
													  where dT == dateTimeValidFrom
													  && s.Element("location").Elements("symbol").Any()
													  select s.Element("location").Element("symbol").Attribute("number").Value).First())),

										// Set the valid and expires timestamp.
										ValidTime = dateTimeValidFrom,
										NextUpdate = dateTimeExpires
									}).First();
				weatherData.Add(weatherEntry);
			}

			// Return the prognosis for this position.
			return weatherData;
		}

		private List<Weather> ProcessXML(XDocument xdoc)
		{
			var data = new List<Weather>();
			return data;
		}
	}
}