using (var connection = new SqlConnection(get_cs()))
{
	connection.Open();
	using (var cmd = new SqlCommand("SELECT запрос", connection))
	{
		using (var rd = cmd.ExecuteReader())
		{
			if (rd.Read())
			{                           
				переменная_куда_вы_хотите_загнать_данные_из_БД = rd.GetValue(номер_столбца).ToString()
			}
		}
	}
	connection.Close();
}
