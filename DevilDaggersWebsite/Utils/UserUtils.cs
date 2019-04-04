using CoreBase.Services;
using DevilDaggersWebsite.Models.User;
using NetBase.Utils;
using System.Collections.Generic;
using System.IO;

namespace DevilDaggersWebsite.Utils
{
	public static class UserUtils
	{
		public static IEnumerable<Ban> GetBans(ICommonObjects commonObjects)
		{
			foreach (string b in FileUtils.GetContents(Path.Combine(commonObjects.Env.WebRootPath, "user", "bans")).Split('\n'))
			{
				if (string.IsNullOrWhiteSpace(b))
					continue;
				
				string ban = b.TrimEnd('\r', '\n');
				string[] props = GetPropsNoSpaces(ban);
				if (props.Length > 2 && int.TryParse(props[2], out int idResponsible))
					yield return new Ban(int.Parse(props[0]), props[1], idResponsible);
				else
					yield return new Ban(int.Parse(props[0]), props[1], null);
			}
		}

		public static IEnumerable<Donator> GetDonators(ICommonObjects commonObjects)
		{
			foreach (string d in FileUtils.GetContents(Path.Combine(commonObjects.Env.WebRootPath, "user", "donators")).Split('\n'))
			{
				if (string.IsNullOrWhiteSpace(d))
					continue;

				string donator = d.TrimEnd('\r', '\n');
				string[] props = GetPropsNoSpaces(donator);

				yield return new Donator(int.Parse(props[0]), props[1], int.Parse(props[2]), char.Parse(props[3]));
			}
		}

		public static IEnumerable<Flag> GetFlags(ICommonObjects commonObjects)
		{
			foreach (string f in FileUtils.GetContents(Path.Combine(commonObjects.Env.WebRootPath, "user", "flags")).Split('\n'))
			{
				if (string.IsNullOrWhiteSpace(f))
					continue;

				string flag = f.TrimEnd('\r', '\n');

				if (flag.EndsWith("?"))
					continue;

				string[] props = GetProps(flag);

				yield return new Flag(int.Parse(props[0]), props[1]);
			}
		}

		public static IEnumerable<PlayerSetting> GetPlayerSettings(ICommonObjects commonObjects)
		{
			foreach (string t in FileUtils.GetContents(Path.Combine(commonObjects.Env.WebRootPath, "user", "settings")).Split('\n'))
			{
				if (string.IsNullOrWhiteSpace(t))
					continue;

				string line = t.TrimEnd('\r', '\n');
				string[] props = GetProps(line);

				yield return new PlayerSetting(int.Parse(props[0]), int.Parse(props[1]), float.Parse(props[2]), int.Parse(props[3]), bool.Parse(props[4]), bool.Parse(props[5]));
			}
		}

		public static IEnumerable<UserTitleCollection> GetTitleCollections(ICommonObjects commonObjects)
		{
			foreach (string t in FileUtils.GetContents(Path.Combine(commonObjects.Env.WebRootPath, "user", "titles")).Split('\n'))
			{
				if (string.IsNullOrWhiteSpace(t))
					continue;

				string line = t.TrimEnd('\r', '\n');
				string[] props = GetPropsNoSpaces(line);

				string[] titles = props[1].Split(',');

				yield return new UserTitleCollection(int.Parse(props[0]), titles);
			}
		}

		private static string[] GetProps(string line)
		{
			while (line.Contains("\t"))
				line = line.Replace("\t", " ");
			while (line.Contains("  "))
				line = line.Replace("  ", " ");
			return line.Split(' ');
		}

		private static string[] GetPropsNoSpaces(string line)
		{
			while (line.Contains("\t\t"))
				line = line.Replace("\t\t", "\t");
			return line.Split('\t');
		}

		public static Dictionary<string, string> TitleImages { get; set; } = new Dictionary<string, string>
		{
			{ "Admin", "eye2" },
			{ "Discord mod", "eye3" },
			{ "Donator", "gem" }
		};

		public static Dictionary<string, string> CountryNames { get; set; } = new Dictionary<string, string>
		{
			{ "AD", "Andorra" },
			{ "AE", "United Arab Emirates" },
			{ "AF", "Afghanistan" },
			{ "AG", "Antigua and Barbuda" },
			{ "AI", "Anguilla" },
			{ "AL", "Albania" },
			{ "AM", "Armenia" },
			{ "AO", "Angola" },
			{ "AQ", "Antarctica" },
			{ "AR", "Argentina" },
			{ "AS", "American Samoa" },
			{ "AT", "Austria" },
			{ "AU", "Australia" },
			{ "AW", "Aruba" },
			{ "AX", "Åland Islands" },
			{ "AZ", "Azerbaijan" },
			{ "BA", "Bosnia and Herzegovina" },
			{ "BB", "Barbados" },
			{ "BD", "Bangladesh" },
			{ "BE", "Belgium" },
			{ "BF", "Burkina Faso" },
			{ "BG", "Bulgaria" },
			{ "BH", "Bahrain" },
			{ "BI", "Burundi" },
			{ "BJ", "Benin" },
			{ "BL", "Saint Barthélemy" },
			{ "BM", "Bermuda" },
			{ "BN", "Brunei Darussalam" },
			{ "BO", "Bolivia (Plurinational State of)" },
			{ "BQ", "Bonaire, Sint Eustatius and Saba" },
			{ "BR", "Brazil" },
			{ "BS", "Bahamas" },
			{ "BT", "Bhutan" },
			{ "BV", "Bouvet Island" },
			{ "BW", "Botswana" },
			{ "BY", "Belarus" },
			{ "BZ", "Belize" },
			{ "CA", "Canada" },
			{ "CC", "Cocos (Keeling) Islands" },
			{ "CD", "Congo, Democratic Republic of the" },
			{ "CF", "Central African Republic" },
			{ "CG", "Congo" },
			{ "CH", "Switzerland" },
			{ "CI", "Côte d'Ivoire" },
			{ "CK", "Cook Islands" },
			{ "CL", "Chile" },
			{ "CM", "Cameroon" },
			{ "CN", "China" },
			{ "CO", "Colombia" },
			{ "CR", "Costa Rica" },
			{ "CU", "Cuba" },
			{ "CV", "Cabo Verde" },
			{ "CW", "Curaçao" },
			{ "CX", "Christmas Island" },
			{ "CY", "Cyprus" },
			{ "CZ", "Czechia" },
			{ "DE", "Germany" },
			{ "DJ", "Djibouti" },
			{ "DK", "Denmark" },
			{ "DM", "Dominica" },
			{ "DO", "Dominican Republic" },
			{ "DZ", "Algeria" },
			{ "EC", "Ecuador" },
			{ "EE", "Estonia" },
			{ "EG", "Egypt" },
			{ "EH", "Western Sahara" },
			{ "ER", "Eritrea" },
			{ "ES", "Spain" },
			{ "ET", "Ethiopia" },
			{ "FI", "Finland" },
			{ "FJ", "Fiji" },
			{ "FK", "Falkland Islands (Malvinas)" },
			{ "FM", "Micronesia (Federated States of)" },
			{ "FO", "Faroe Islands" },
			{ "FR", "France" },
			{ "GA", "Gabon" },
			{ "GB", "United Kingdom of Great Britain and Northern Ireland" },
			{ "GD", "Grenada" },
			{ "GE", "Georgia" },
			{ "GF", "French Guiana" },
			{ "GG", "Guernsey" },
			{ "GH", "Ghana" },
			{ "GI", "Gibraltar" },
			{ "GL", "Greenland" },
			{ "GM", "Gambia" },
			{ "GN", "Guinea" },
			{ "GP", "Guadeloupe" },
			{ "GQ", "Equatorial Guinea" },
			{ "GR", "Greece" },
			{ "GS", "South Georgia and the South Sandwich Islands" },
			{ "GT", "Guatemala" },
			{ "GU", "Guam" },
			{ "GW", "Guinea-Bissau" },
			{ "GY", "Guyana" },
			{ "HK", "Hong Kong" },
			{ "HM", "Heard Island and McDonald Islands" },
			{ "HN", "Honduras" },
			{ "HR", "Croatia" },
			{ "HT", "Haiti" },
			{ "HU", "Hungary" },
			{ "ID", "Indonesia" },
			{ "IE", "Ireland" },
			{ "IL", "Israel" },
			{ "IM", "Isle of Man" },
			{ "IN", "India" },
			{ "IO", "British Indian Ocean Territory" },
			{ "IQ", "Iraq" },
			{ "IR", "Iran (Islamic Republic of)" },
			{ "IS", "Iceland" },
			{ "IT", "Italy" },
			{ "JE", "Jersey" },
			{ "JM", "Jamaica" },
			{ "JO", "Jordan" },
			{ "JP", "Japan" },
			{ "KE", "Kenya" },
			{ "KG", "Kyrgyzstan" },
			{ "KH", "Cambodia" },
			{ "KI", "Kiribati" },
			{ "KM", "Comoros" },
			{ "KN", "Saint Kitts and Nevis" },
			{ "KP", "Korea (Democratic People's Republic of)" },
			{ "KR", "Korea, Republic of" },
			{ "KW", "Kuwait" },
			{ "KY", "Cayman Islands" },
			{ "KZ", "Kazakhstan" },
			{ "LA", "Lao People's Democratic Republic" },
			{ "LB", "Lebanon" },
			{ "LC", "Saint Lucia" },
			{ "LI", "Liechtenstein" },
			{ "LK", "Sri Lanka" },
			{ "LR", "Liberia" },
			{ "LS", "Lesotho" },
			{ "LT", "Lithuania" },
			{ "LU", "Luxembourg" },
			{ "LV", "Latvia" },
			{ "LY", "Libya" },
			{ "MA", "Morocco" },
			{ "MC", "Monaco" },
			{ "MD", "Moldova, Republic of" },
			{ "ME", "Montenegro" },
			{ "MF", "Saint Martin (French part)" },
			{ "MG", "Madagascar" },
			{ "MH", "Marshall Islands" },
			{ "MK", "Macedonia, the former Yugoslav Republic of" },
			{ "ML", "Mali" },
			{ "MM", "Myanmar" },
			{ "MN", "Mongolia" },
			{ "MO", "Macao" },
			{ "MP", "Northern Mariana Islands" },
			{ "MQ", "Martinique" },
			{ "MR", "Mauritania" },
			{ "MS", "Montserrat" },
			{ "MT", "Malta" },
			{ "MU", "Mauritius" },
			{ "MV", "Maldives" },
			{ "MW", "Malawi" },
			{ "MX", "Mexico" },
			{ "MY", "Malaysia" },
			{ "MZ", "Mozambique" },
			{ "NA", "Namibia" },
			{ "NC", "New Caledonia" },
			{ "NE", "Niger" },
			{ "NF", "Norfolk Island" },
			{ "NG", "Nigeria" },
			{ "NI", "Nicaragua" },
			{ "NL", "Netherlands" },
			{ "NO", "Norway" },
			{ "NP", "Nepal" },
			{ "NR", "Nauru" },
			{ "NU", "Niue" },
			{ "NZ", "New Zealand" },
			{ "OM", "Oman" },
			{ "PA", "Panama" },
			{ "PE", "Peru" },
			{ "PF", "French Polynesia" },
			{ "PG", "Papua New Guinea" },
			{ "PH", "Philippines" },
			{ "PK", "Pakistan" },
			{ "PL", "Poland" },
			{ "PM", "Saint Pierre and Miquelon" },
			{ "PN", "Pitcairn" },
			{ "PR", "Puerto Rico" },
			{ "PS", "Palestine, State of" },
			{ "PT", "Portugal" },
			{ "PW", "Palau" },
			{ "PY", "Paraguay" },
			{ "QA", "Qatar" },
			{ "RE", "Réunion" },
			{ "RO", "Romania" },
			{ "RS", "Serbia" },
			{ "RU", "Russian Federation" },
			{ "RW", "Rwanda" },
			{ "SA", "Saudi Arabia" },
			{ "SB", "Solomon Islands" },
			{ "SC", "Seychelles" },
			{ "SD", "Sudan" },
			{ "SE", "Sweden" },
			{ "SG", "Singapore" },
			{ "SH", "Saint Helena, Ascension and Tristan da Cunha" },
			{ "SI", "Slovenia" },
			{ "SJ", "Svalbard and Jan Mayen" },
			{ "SK", "Slovakia" },
			{ "SL", "Sierra Leone" },
			{ "SM", "San Marino" },
			{ "SN", "Senegal" },
			{ "SO", "Somalia" },
			{ "SR", "Suriname" },
			{ "SS", "South Sudan" },
			{ "ST", "Sao Tome and Principe" },
			{ "SV", "El Salvador" },
			{ "SX", "Sint Maarten (Dutch part)" },
			{ "SY", "Syrian Arab Republic" },
			{ "SZ", "Eswatini" },
			{ "TC", "Turks and Caicos Islands" },
			{ "TD", "Chad" },
			{ "TF", "French Southern Territories" },
			{ "TG", "Togo" },
			{ "TH", "Thailand" },
			{ "TJ", "Tajikistan" },
			{ "TK", "Tokelau" },
			{ "TL", "Timor-Leste" },
			{ "TM", "Turkmenistan" },
			{ "TN", "Tunisia" },
			{ "TO", "Tonga" },
			{ "TR", "Turkey" },
			{ "TT", "Trinidad and Tobago" },
			{ "TV", "Tuvalu" },
			{ "TW", "Taiwan, Province of China" },
			{ "TZ", "Tanzania, United Republic of" },
			{ "UA", "Ukraine" },
			{ "UG", "Uganda" },
			{ "UM", "United States Minor Outlying Islands" },
			{ "US", "United States of America" },
			{ "UY", "Uruguay" },
			{ "UZ", "Uzbekistan" },
			{ "VA", "Holy See" },
			{ "VC", "Saint Vincent and the Grenadines" },
			{ "VE", "Venezuela (Bolivarian Republic of)" },
			{ "VG", "Virgin Islands (British)" },
			{ "VI", "Virgin Islands (U.S.)" },
			{ "VN", "Viet Nam" },
			{ "VU", "Vanuatu" },
			{ "WF", "Wallis and Futuna" },
			{ "WS", "Samoa" },
			{ "YE", "Yemen" },
			{ "YT", "Mayotte" },
			{ "ZA", "South Africa" },
			{ "ZM", "Zambia" },
			{ "ZW", "Zimbabwe" },
		};
	}
}