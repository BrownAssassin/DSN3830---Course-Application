using GameServer001.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer001.DAO
{
    class ResultDAO
    {
        public Result GetResultByUserId(MySqlConnection conn, int userId)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from result where userid=@userid ", conn);
                cmd.Parameters.AddWithValue("userid", userId);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int id = reader.GetInt32("id");
                    int totalcount = reader.GetInt32("totalcount");
                    int wincount = reader.GetInt32("wincount");
                    Result res = new Result(id, userId, totalcount, wincount);
                    return res;
                }
                else
                {
                    Result res = new Result(-1, userId, 0, 0);
                    return res;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                if(reader!=null)
                reader.Close();
            }

            return null;

        }
    }
}
