using (var connection = new SqlConnection(get_cs()))
{
	connection.Open();
	using (var cmd = new SqlCommand(КОМАНДА, connection))
	{
		cmd.ExecuteNonQuery();
	}
	connection.Close();
}   
