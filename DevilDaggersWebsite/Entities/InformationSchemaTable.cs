using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevilDaggersWebsite.Entities
{
	public class InformationSchemaTable
	{
		[Column("TABLE_CATALOG")]
		public string TableCatalog { get; set; } = null!;

		[Key]
		[Column("TABLE_NAME")]
		public string TableName { get; set; } = null!;

		[Column("TABLE_SCHEMA")]
		public string TableSchema { get; set; } = null!;

		//[Column("TABLE_ROWS")]
		//public ulong TableRows { get; set; }

		//[Column("avg_row_length")]
		//public int AverageRowLength { get; set; }

		//[Column("data_length")]
		//public int DataLength { get; set; }

		//[Column("index_length")]
		//public int IndexLength { get; set; }
	}
}
