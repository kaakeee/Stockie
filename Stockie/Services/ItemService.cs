using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Stockie.Models;

namespace Stockie.Services
{
 public class ItemService
 {
 public static IEnumerable<Item> GetAllItems()
 {
 var list = new List<Item>();
 using (var conn = Data.DatabaseHelper.GetStockConnection())
 {
 conn.Open();
 string query = "SELECT id, nombre, tipo, codigo_sn FROM items";
 using (var cmd = new SqlCommand(query, conn))
 using (var reader = cmd.ExecuteReader())
 {
 while (reader.Read())
 {
 list.Add(new Item
 {
 Id = reader.GetInt32(reader.GetOrdinal("id")),
 Nombre = reader.GetString(reader.GetOrdinal("nombre")),
 Tipo = reader.IsDBNull(reader.GetOrdinal("tipo")) ? null : reader.GetString(reader.GetOrdinal("tipo")),
 CodigoSn = reader.IsDBNull(reader.GetOrdinal("codigo_sn")) ? null : reader.GetString(reader.GetOrdinal("codigo_sn"))
 });
 }
 }
 }
 return list;
 }

 public static void AddItem(Item item)
 {
 using (var conn = Data.DatabaseHelper.GetStockConnection())
 {
 conn.Open();
 string insert = "INSERT INTO items (nombre, tipo, codigo_sn) VALUES (@nombre, @tipo, @codigo_sn)";
 using (var cmd = new SqlCommand(insert, conn))
 {
 cmd.Parameters.AddWithValue("@nombre", item.Nombre);
 cmd.Parameters.AddWithValue("@tipo", (object)item.Tipo ?? DBNull.Value);
 cmd.Parameters.AddWithValue("@codigo_sn", (object)item.CodigoSn ?? DBNull.Value);
 cmd.ExecuteNonQuery();
 }
 }
 }

 public static void DeleteItem(int id)
 {
 using (var conn = Data.DatabaseHelper.GetStockConnection())
 {
 conn.Open();
 string delete = "DELETE FROM items WHERE id = @id";
 using (var cmd = new SqlCommand(delete, conn))
 {
 cmd.Parameters.AddWithValue("@id", id);
 cmd.ExecuteNonQuery();
 }
 }
 }
 }
}
